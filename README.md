# VideoGameManager

Aplicació web desenvolupada amb **ASP.NET Core Razor Pages** per gestionar un catàleg de videojocs. Aquesta pràctica implementa el patró CRUD complet (Create, Read, Update, Delete) i afegeix la capacitat de persistir i exportar la informació en diferents formats de fitxer.

## 🚀 Característiques principals
* **CRUD Complet:** Llistar, afegir, editar i eliminar videojocs amb validacions de model.
* **Persistència Automàtica (JSON i CSV):** Els canvis es desen automàticament cada vegada que es crea, modifica o elimina un joc.
* **Registre d'Activitat (TXT):** Sistema de logs que registra la data, hora i acció realitzada sobre el catàleg.
* **Exportació i Rànquing (XML):** Generació d'un fitxer XML ordenant els videojocs per puntuació (de major a menor).
* **Injecció de Dependències:** Ús de Serveis Singleton per mantenir l'estat i gestionar els fitxers de forma centralitzada.

---

## 📂 Estructura del Projecte

**Nota important sobre els fitxers de dades:** Tot i que la proposta inicial suggeria guardar les dades a `wwwroot/data/`, s'ha implementat la carpeta `Data/` a l'arrel del projecte (usant `ContentRootPath`). Això garanteix que l'aplicació tingui sempre permisos d'escriptura correctes independentment de l'entorn de desenvolupament, evitant la pèrdua de dades per reinicis del servidor.

~~~text
VideoGameManager/
├── Models/
│   └── Game.cs                  # Model de dades amb DataAnnotations
├── Services/
│   ├── GameService.cs           # Lògica central i gestió de memòria/auto-save
│   ├── GameRepository.cs        # Persistència principal (JSON)
│   ├── GamesExporter.cs         # Exportació i importació (CSV)
│   └── RankingExporter.cs       # Generació del rànquing (XML)
├── Pages/
│   ├── Games/
│   │   ├── Index.cshtml         # Llista de jocs
│   │   ├── Details.cshtml       # Veure detalls
│   │   ├── Create.cshtml        # Formulari d'alta
│   │   ├── Edit.cshtml          # Formulari d'edició
│   │   └── Delete.cshtml        # Confirmació d'eliminació
│   ├── Files/                   
│   │   └── Index.cshtml         # Gestió, visualització de logs i exportacions
│   └── Shared/
│       └── _Layout.cshtml       # Plantilla principal
├── Data/                        # 📁 FITXERS DE DADES (Es crea automàticament)
│   ├── activity_log.txt         # Log d'accions
│   ├── games.json               # Base de dades JSON
│   ├── games.csv                # Dades exportades a CSV
│   └── ranking.xml              # Rànquing XML
└── Program.cs                   # Configuració de l'App i injecció de Singletons
~~~

---

## ⚙️ Instruccions d'execució

### Requisits previs
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download) (o superior) instal·lat.
* Visual Studio 2022 o Visual Studio Code.

### Com executar l'aplicació
1. **Clonar o descarregar** el projecte al teu ordinador.
2. Obre un terminal o línia de comandes a la carpeta arrel del projecte (on es troba el fitxer `VideoGameManager.csproj`).
3. Construeix el projecte per assegurar que no hi ha errors executant:
   ~~~bash
   dotnet build
   ~~~
4. Executa l'aplicació:
   ~~~bash
   dotnet run
   ~~~
5. Obre el teu navegador web i ves a la URL que indica la consola (generalment `http://localhost:5000` o `https://localhost:5001`).

*Nota per a Visual Studio:* Si fas servir Visual Studio 2022, només has de prémer el botó **"Run" (F5)**. Per veure la carpeta `Data/` i els seus fitxers a l'Explorador de Solucions un cop l'app ha creat el primer joc, recorda fer clic a l'opció **"Mostra tots els fitxers"** (Show All Files).

---

## 📄 Exemples de fitxers de dades

L'aplicació genera i llegeix els següents formats. Els fitxers es creen automàticament a la carpeta `/Data` quan s'afegeix el primer joc a la web.

### 1. JSON (`Data/games.json`)
S'utilitza com a persistència principal de l'aplicació. Es llegeix a l'inici i es desa amb cada canvi.
~~~json
[
  {
    "Id": 1,
    "Title": "The Legend of Zelda: TotK",
    "Genre": "Adventure",
    "Year": 2023,
    "Score": 9.8,
    "Description": "Open-world action RPG"
  }
]
~~~

### 2. CSV (`Data/games.csv`)
S'actualitza automàticament al mateix temps que el JSON. Manté les dades separades per comes per permetre una ràpida importació a Excel o altres eines.
~~~csv
Id,Title,Genre,Year,Score
1,The Legend of Zelda: TotK,Adventure,2023,9.8
2,Elden Ring,RPG,2022,9.5
~~~

### 3. XML (`Data/ranking.xml`)
S'exporta només sota demanda des de la vista `/Files`. Ordena els jocs per puntuació descendent (usant LINQ) i inclou només les dades sol·licitades.
~~~xml
<?xml version="1.0" encoding="utf-8" standalone="yes"?>
<AppConfig>
  <AppTitle>VideoGame Ranking</AppTitle>
  <Games>
    <Game>
      <id>1</id>
      <score>9.8</score>
      <title>The Legend of Zelda: TotK</title>
      <genre>Adventure</genre>
      <year>2023</year>
    </Game>
    <Game>
      <id>2</id>
      <score>9.5</score>
      <title>Elden Ring</title>
      <genre>RPG</genre>
      <year>2022</year>
    </Game>
  </Games>
</AppConfig>
~~~

### 4. Text Pla / Log (`Data/activity_log.txt`)
Registra cada operació en mode Append (afegeix al final sense sobreescriure). Aquest fitxer es llegeix i es mostra directament a la pàgina de fitxers.
~~~text
[03/05/2026 10:15:30] [CREATE] The Legend of Zelda: TotK
[03/05/2026 10:18:45] [CREATE] Elden Ring
[03/05/2026 10:20:12] [UPDATE] Elden Ring
[03/05/2026 10:25:00] [DELETE] FIFA 23
~~~
