function setLang(lang) {
    if (lang == 'hu') {
        document.getElementById('magyarGomb').innerText = "Magyar";
        document.getElementById('angolGomb').innerText = "Angol";
        document.getElementById('palyaszerkeszto').innerText = "Pályaszerkesztő";
        document.getElementById('szelesseg').innerText = "Szélesség";
        document.getElementById('szelessegLeiras').innerText = "Megadja a generált térkép szélességét (max 30)";
        document.getElementById('magassag').innerText = "Magasság";
        document.getElementById('magassagLeiras').innerText = "Megadja a generált térkép magasságát (max 30)";
        document.getElementById('szoba').innerText = "Szoba";
        document.getElementById('szobaLeiras').innerText = "A kattintott helyre lehelyez egy szobát";
        document.getElementById('torles').innerText = "Törlés";
        document.getElementById('torlesLeiras').innerText = "Kitörli a kattintott elemet";
        document.getElementById('mentesGomb').innerText = "Mentés";
        document.getElementById('mentesLeiras').innerText = "Feldobja a program a mentés ablakot, amelyben .sav és .txt formátumban lehet elmenteni a készített pályát";
        document.getElementById('elemGombok').innerText = "Elem gombok";
        document.getElementById('elemGombokLeiras').innerText = "Az adott elemet lehelyezi a kattintott pozícióra";

        document.getElementById('jatek').innerText = "Játék";
        document.getElementById('hozzaadas').innerText = "Hozzáadás";
        document.getElementById('hozzaadasLeiras').innerText = "A térkép elnevezése után hozzáadja a .sav fájlból kiválasztott térképet a pályákhoz";
        document.getElementById('betoltes').innerText = "Betöltés";
        document.getElementById('betoltesLeiras').innerText = "Betölti a mentett játékállást .sav fájlból";
        document.getElementById('inditas').innerText = "Indítás";
        document.getElementById('initasLeiras').innerText = "A térkép kiválasztása után elindítja a játékot";
        document.getElementById('iranyitas').innerText = "Irányítás";
        document.getElementById('iranyitasLeiras').innerText = "WASD";
        document.getElementById('irany').innerText = "Irány";
        document.getElementById('iranyLeiras').innerText = "Lehetséges irányok";
        document.getElementById('kamra').innerText = "Kamra";
        document.getElementById('kamraLeiras').innerText = "Hátralévő kamrák száma";
        document.getElementById('jatekMentes').innerText = "Mentés";
        document.getElementById('jatekMentesLeiras').innerText = "A játékállás mentése .sav fájlba";
        document.getElementById('nyelvvalaszto').innerText = "Nyelvválasztó";
        document.getElementById('nyelvvalasztoLeiras').innerText = "Angol és magyar nyelv közötti váltás";
        document.getElementById('vissza').innerText = "Vissza";
        document.getElementById('visszaLeiras').innerText = "Visszalépés a menübe";
        document.getElementById('nehezMod').innerText = "Nehéz mód";
        document.getElementById('nehezModLeiras').innerText = "Átváltás láthatatlan térképes módra";
    }

    if (lang == 'en') {
        document.getElementById('magyarGomb').innerText = "Hungarian";
        document.getElementById('angolGomb').innerText = "English";
        document.getElementById('palyaszerkeszto').innerText = "Level Editor";
        document.getElementById('szelesseg').innerText = "Width";
        document.getElementById('szelessegLeiras').innerText = "Sets the width of the generated map (max 30)";
        document.getElementById('magassag').innerText = "Height";
        document.getElementById('magassagLeiras').innerText = "Sets the height of the generated map (max 30)";
        document.getElementById('szoba').innerText = "Room";
        document.getElementById('szobaLeiras').innerText = "Places a room at the clicked position";
        document.getElementById('torles').innerText = "Delete";
        document.getElementById('torlesLeiras').innerText = "Deletes the clicked element";
        document.getElementById('mentesGomb').innerText = "Save";
        document.getElementById('mentesLeiras').innerText = "Opens the save dialog, where the created level can be saved in .sav or .txt format";
        document.getElementById('elemGombok').innerText = "Element buttons";
        document.getElementById('elemGombokLeiras').innerText = "Places the selected element at the clicked position";

        document.getElementById('jatek').innerText = "Game";
        document.getElementById('hozzaadas').innerText = "Add";
        document.getElementById('hozzaadasLeiras').innerText = "After naming the map, adds the selected .sav file to the available levels";
        document.getElementById('betoltes').innerText = "Load";
        document.getElementById('betoltesLeiras').innerText = "Loads a saved game state from a .sav file";
        document.getElementById('inditas').innerText = "Start";
        document.getElementById('initasLeiras').innerText = "Starts the game after selecting a map";
        document.getElementById('iranyitas').innerText = "Controls";
        document.getElementById('iranyitasLeiras').innerText = "WASD";
        document.getElementById('irany').innerText = "Direction";
        document.getElementById('iranyLeiras').innerText = "Possible directions";
        document.getElementById('kamra').innerText = "Chamber";
        document.getElementById('kamraLeiras').innerText = "Number of remaining chambers";
        document.getElementById('jatekMentes').innerText = "Save";
        document.getElementById('jatekMentesLeiras').innerText = "Saves the current game state to a .sav file";
        document.getElementById('nyelvvalaszto').innerText = "Language selector";
        document.getElementById('nyelvvalasztoLeiras').innerText = "Switch between English and Hungarian";
        document.getElementById('vissza').innerText = "Back";
        document.getElementById('visszaLeiras').innerText = "Return to the menu";
        document.getElementById('nehezMod').innerText = "Hard mode";
        document.getElementById('nehezModLeiras').innerText = "Switch to invisible map mode";
    }
}

setLang('hu');