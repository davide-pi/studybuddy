checkLogin();

var cards = [];
var index = 0;
var showingBack = false;
var known = 0;
var deckId = getParam("deck");

fetch("/api/decks/" + deckId)
    .then(function (r) { return r.json(); })
    .then(function (deck) {
        document.getElementById("title").innerText = deck.name;
        cards = shuffle(deck.cards);
        show();
    });

function show() {
    showingBack = false;
    var el = document.getElementById("card");
    el.className = "flashcard";
    el.innerHTML = cards[index].front;
    document.getElementById("counter").innerText = "Carta " + (index + 1) + " di " + cards.length;
    document.getElementById("buttons").style.display = "none";
}

function flip() {
    var el = document.getElementById("card");
    if (showingBack) {
        el.className = "flashcard";
        el.innerHTML = cards[index].front;
    } else {
        el.className = "flashcard back";
        el.innerHTML = cards[index].back;
        document.getElementById("buttons").style.display = "block";
    }
    showingBack = !showingBack;
}

function answer(isKnown) {
    var url = "/api/cards/" + cards[index].id + (isKnown ? "/known" : "/unknown");
    fetch(url, { method: "PUT" });
    if (isKnown) known++;
    next();
}

function next() {
    index++;
    if (index >= cards.length - 1) {
        finish();
        return;
    }
    show();
}

function finish() {
    document.getElementById("card").style.display = "none";
    document.getElementById("buttons").style.display = "none";
    document.getElementById("counter").style.display = "none";
    document.getElementById("end").style.display = "block";
    document.getElementById("summary").innerText = "Ne sapevi " + known + " su " + cards.length;
}
