# Compteur carnet (Windows)

Application de bureau Windows (WPF / .NET 8) portée depuis le projet Android
d'origine ("NewProject34" / Sketchware). Elle permet de :

- importer des carnets depuis des fichiers JSON ;
- afficher la liste des carnets disponibles et les supprimer ;
- répartir les carnets entre plusieurs "onglets" (agents) et suivre
  l'avancement feuillet par feuillet ("Vita") ;
- consulter l'historique des carnets terminés.

Toute la navigation se fait à l'intérieur d'une seule fenêtre (comme sur
smartphone) : chaque écran (Liste, Tâches, Historique) possède un petit
bouton **‹ Retour** en bas pour revenir au menu principal.

Le compteur de feuillets fonctionne en compte à rebours : il affiche le
nombre de feuillets restants (ex. `22/22`) et descend jusqu'à `00/22`
lorsque le carnet est terminé.

Les données sont sauvegardées automatiquement dans
`%AppData%\CompteurCarnet\data.json`.

## Compiler depuis GitHub

### Option A — GitHub Actions (recommandé, aucune installation requise)

1. Créez un dépôt GitHub et poussez ce dossier dedans :
   ```
   git init
   git add .
   git commit -m "Compteur carnet - version Windows"
   git branch -M main
   git remote add origin <URL_DE_VOTRE_DEPOT>
   git push -u origin main
   ```
2. Le workflow `.github/workflows/build.yml` se déclenche automatiquement
   (`windows-latest`) et compile l'application.
3. Allez dans l'onglet **Actions** du dépôt, ouvrez la dernière exécution ;
   deux artefacts sont proposés au téléchargement :
   - **CompteurCarnet-Setup** → `CompteurCarnet-Setup.exe`, un véritable
     installeur (raccourcis, désinstalleur dans "Applications") ;
   - **CompteurCarnet-windows-exe** → `CompteurCarnet.exe` seul, à copier où
     vous voulez, sans installation.
4. Pour obtenir un lien de téléchargement permanent, créez un tag de version
   (`git tag v1.0.0 && git push origin v1.0.0`) : une **Release** GitHub sera
   créée automatiquement avec les deux fichiers ci-dessus.

### Option B — Compiler localement sous Windows

Prérequis : [.NET 8 SDK](https://dotnet.microsoft.com/download) avec la
charge de travail de développement de bureau (`desktop` / WPF).

```powershell
git clone <URL_DE_VOTRE_DEPOT>
cd CompteurCarnet
dotnet restore
dotnet build -c Release
```

Pour obtenir un `.exe` autonome à distribuer :

```powershell
dotnet publish CompteurCarnet\CompteurCarnet.csproj -c Release -r win-x64 `
  --self-contained true -p:PublishSingleFile=true `
  -p:IncludeNativeLibrariesForSelfExtract=true -o publish
```

L'exécutable se trouve alors dans `publish\CompteurCarnet.exe`.

### Créer l'installeur (.exe) localement

Prérequis : [Inno Setup](https://jrsoftware.org/isdl.php) (gratuit).

Après avoir publié l'exécutable (étape précédente), compilez le script :

```powershell
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" Installer\CompteurCarnet.iss
```

L'installeur `CompteurCarnet-Setup.exe` est généré dans le dossier
`installer-output\`. Il installe l'application dans "Program Files", crée un
raccourci dans le menu Démarrer (et sur le Bureau si l'utilisateur coche la
case), et ajoute une entrée de désinstallation dans
"Applications et fonctionnalités" de Windows.

## Installer sur Windows

**Avec l'installeur (`CompteurCarnet-Setup.exe`) — recommandé :**

1. Téléchargez `CompteurCarnet-Setup.exe` (artefact GitHub Actions ou
   Release).
2. Double-cliquez dessus et suivez l'assistant (choix du dossier, icône sur
   le Bureau en option).
3. Windows peut afficher un avertissement SmartScreen car l'installeur n'est
   pas signé numériquement : cliquez sur **Informations complémentaires**
   puis **Exécuter quand même**.
4. L'application apparaît ensuite dans le menu Démarrer, et peut être
   désinstallée normalement depuis "Applications et fonctionnalités".

**Sans installeur (`CompteurCarnet.exe` seul) :**

Copiez simplement l'exécutable où vous voulez sur l'ordinateur et
double-cliquez dessus pour le lancer — aucune installation requise.

## Structure du projet

```
CompteurCarnet.sln
CompteurCarnet/
  CompteurCarnet.csproj
  MainWindow.xaml(.cs)        - fenêtre unique, contient la zone de contenu qui change d'écran
  MenuView.xaml(.cs)          - menu principal
  ListeView.xaml(.cs)         - importation et liste des carnets
  TachesView.xaml(.cs)        - onglets / suivi (compte à rebours) de la progression
  HistoriqueView.xaml(.cs)    - historique des carnets terminés
  SelectCarnetWindow.xaml(.cs)- petite boîte de dialogue de sélection d'un carnet
  Models/                     - Carnet, HistoriqueEntry, AppData, import JSON
  Services/
    DataStore.cs              - sauvegarde/chargement (remplace SharedPreferences)
    CommunesData.cs           - données communes/fokontany (région Itasy)
.github/workflows/build.yml   - compilation automatique via GitHub Actions
```

## Format du fichier JSON à importer

```json
{
  "communes": [
    {
      "code_commune": "130101",
      "fokontany": [
        {
          "code_fokontany": "13010101",
          "carnets": [
            {
              "num_carnet": "001",
              "feuillets": [
                { "num_feuillet": "0001", "statut": "" },
                { "num_feuillet": "0002", "statut": "annulé" }
              ]
            }
          ]
        }
      ]
    }
  ]
}
```

Un feuillet est considéré comme "valide" quand son champ `statut` est vide
ou absent (comme dans l'application Android d'origine).
