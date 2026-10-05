using StudyBuddy.Web.Models;

namespace StudyBuddy.Web.Data;

public static class Seed
{
    public static void Init(StudyDbContext db)
    {
        if (db.Users.Any() == false)
        {
            db.Users.Add(new User { Username = "mario", Password = "password123" });
            db.Users.Add(new User { Username = "giulia", Password = "giulia2008" });
            db.SaveChanges();
        }

        if (db.Decks.Count() > 0) return;

        var storia = new Deck { Name = "Risorgimento", Materia = "Storia", Descrizione = "Date e personaggi dell'unità d'Italia" };
        storia.Cards.Add(new Card { Front = "In che anno è stata proclamata l'Unità d'Italia?", Back = "1861" });
        storia.Cards.Add(new Card { Front = "Chi guidò la spedizione dei Mille?", Back = "Giuseppe Garibaldi" });
        storia.Cards.Add(new Card { Front = "Primo re d'Italia", Back = "Vittorio Emanuele II" });
        storia.Cards.Add(new Card { Front = "Chi era il primo ministro del Regno di Sardegna nel 1852?", Back = "Camillo Benso, conte di Cavour" });
        storia.Cards.Add(new Card { Front = "In che anno Roma diventa capitale?", Back = "1871" });
        storia.Cards.Add(new Card { Front = "Fondatore della Giovine Italia", Back = "Giuseppe Mazzini" });

        var scienze = new Deck { Name = "La cellula", Materia = "Scienze", Descrizione = "Biologia - primo quadrimestre" };
        scienze.Cards.Add(new Card { Front = "Qual è la \"centrale energetica\" della cellula?", Back = "Il mitocondrio" });
        scienze.Cards.Add(new Card { Front = "Dove si trova il DNA nelle cellule eucariote?", Back = "Nel nucleo" });
        scienze.Cards.Add(new Card { Front = "Organulo della fotosintesi", Back = "Cloroplasto" });
        scienze.Cards.Add(new Card { Front = "Cosa produce i ribosomi?", Back = "Il nucleolo" });
        scienze.Cards.Add(new Card { Front = "Cellula senza nucleo", Back = "Procariote" });

        var inglese = new Deck { Name = "Irregular verbs", Materia = "Inglese", Descrizione = "Paradigmi da sapere a memoria" };
        inglese.Cards.Add(new Card { Front = "to go", Back = "go - went - gone" });
        inglese.Cards.Add(new Card { Front = "to bring", Back = "bring - brought - brought" });
        inglese.Cards.Add(new Card { Front = "to catch", Back = "catch - caught - caught" });
        inglese.Cards.Add(new Card { Front = "to teach", Back = "teach - taught - taught" });
        inglese.Cards.Add(new Card { Front = "to choose", Back = "choose - chose - chosen" });
        inglese.Cards.Add(new Card { Front = "to write", Back = "write - wrote - written" });
        inglese.Cards.Add(new Card { Front = "to fly", Back = "fly - flew - flown" });

        var info = new Deck { Name = "Reti", Materia = "Informatica", Descrizione = "TPSIT - modello ISO/OSI e TCP/IP" };
        info.Cards.Add(new Card { Front = "Quanti livelli ha il modello ISO/OSI?", Back = "7" });
        info.Cards.Add(new Card { Front = "A che livello lavora il protocollo IP?", Back = "Livello 3 - Rete" });
        info.Cards.Add(new Card { Front = "Porta di default di HTTPS", Back = "443" });
        info.Cards.Add(new Card { Front = "Cosa fa il DNS?", Back = "Traduce i nomi di dominio in indirizzi IP" });

        db.Decks.AddRange(storia, scienze, inglese, info);
        db.SaveChanges();
    }
}
