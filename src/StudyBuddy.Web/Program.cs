using Microsoft.EntityFrameworkCore;
using StudyBuddy.Web.Data;
using StudyBuddy.Web.Models;
using StudyBuddy.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// database
Directory.CreateDirectory("data");
builder.Services.AddDbContext<StudyDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Register MVC controllers and application services
builder.Services.AddControllers();
builder.Services.AddScoped<IQuizService, QuizService>();

var app = builder.Build();

// crea il db se non c'e
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StudyDbContext>();
    db.Database.EnsureCreated();
    Seed.Init(db);
}

app.UseDefaultFiles();
app.UseStaticFiles();

// ===================== LOGIN =====================

app.MapPost("/api/login", (StudyDbContext db, LoginRequest req) =>
{
    var u = db.Users.FirstOrDefault(x => x.Username == req.Username && x.Password == req.Password);
    if (u == null) return Results.Unauthorized();
    return Results.Ok(new { u.Id, u.Username });
});

app.MapPost("/api/register", (StudyDbContext db, LoginRequest req) =>
{
    if (db.Users.Any(x => x.Username == req.Username))
        return Results.BadRequest("utente gia esistente");

    var u = new User { Username = req.Username, Password = req.Password };
    db.Users.Add(u);
    db.SaveChanges();
    return Results.Ok(new { u.Id, u.Username });
});

// ===================== MAZZI =====================

app.MapGet("/api/decks", (StudyDbContext db) =>
{
    var decks = db.Decks.ToList();
    var res = new List<object>();
    foreach (var d in decks)
    {
        var n = db.Cards.Count(c => c.DeckId == d.Id);
        res.Add(new { d.Id, d.Name, d.Materia, d.Descrizione, numCarte = n });
    }
    return res;
});

app.MapGet("/api/decks/{id}", (int id, StudyDbContext db) =>
{
    var d = db.Decks.Include(x => x.Cards).FirstOrDefault(x => x.Id == id);
    return d;
});

app.MapPost("/api/decks", (StudyDbContext db, Deck deck) =>
{
    db.Decks.Add(deck);
    db.SaveChanges();
    return deck;
});

app.MapDelete("/api/decks/{id}", (int id, StudyDbContext db) =>
{
    var d = db.Decks.Find(id);
    if (d != null)
    {
        db.Decks.Remove(d);
        try
        {
            db.SaveChanges();
        }
        catch
        {
            // a volte da errore, ignoro
        }
    }
    return Results.Ok();
});

// ===================== CARTE =====================

app.MapPost("/api/decks/{id}/cards", (int id, Card card, StudyDbContext db) =>
{
    card.DeckId = id;
    db.Cards.Add(card);
    db.SaveChanges();
    return card;
});

app.MapDelete("/api/cards/{id}", (int id, StudyDbContext db) =>
{
    var c = db.Cards.Find(id);
    db.Cards.Remove(c!);
    db.SaveChanges();
    return Results.Ok();
});

// la so -> sale di scatola
app.MapPut("/api/cards/{id}/known", (int id, StudyDbContext db) =>
{
    var c = db.Cards.Find(id);
    c!.Box++;
    c.LastReview = DateTime.Now;
    db.SaveChanges();
    return Results.Ok(c);
});

// non la so -> torna alla scatola 1
app.MapPut("/api/cards/{id}/unknown", (int id, StudyDbContext db) =>
{
    var c = db.Cards.Find(id);
    c!.Box = 1;
    c.LastReview = DateTime.Now;
    db.SaveChanges();
    return Results.Ok(c);
});

// ===================== RICERCA =====================

app.MapGet("/api/search", (string q, IConfiguration config) =>
{
    var helper = new DbHelper(config.GetConnectionString("Default")!);
    return helper.Search(q);
});

app.MapControllers();

app.Run();

record LoginRequest(string Username, string Password);
