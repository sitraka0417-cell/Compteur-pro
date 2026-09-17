; Script Inno Setup pour Compteur carnet.
; Compile ce fichier avec Inno Setup (ISCC.exe) pour obtenir un installeur
; classique (Setup.exe) qui copie l'application, crée les raccourcis et
; un désinstalleur.
;
; Compilation locale : installez Inno Setup (https://jrsoftware.org/isinfo.php)
; puis lancez :
;   ISCC Installer\CompteurCarnet.iss
; en supposant que l'exécutable auto-suffisant a déjà été publié dans
; publish\CompteurCarnet.exe (voir README.md, "dotnet publish").
;
; Ce script est aussi utilisé automatiquement par le workflow GitHub Actions.

#define MyAppName "Compteur carnet"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Compteur carnet"
#define MyAppExeName "CompteurCarnet.exe"

[Setup]
AppId={{97766F99-D202-4C0E-9EF2-78F295067D7D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=..\installer-output
OutputBaseFilename=CompteurCarnet-Setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern
ArchitecturesInstallIn64BitMode=x64compatible
UninstallDisplayIcon={app}\{#MyAppExeName}

[Languages]
Name: "french"; MessagesFile: "compiler:Languages\French.isl"

[Tasks]
Name: "desktopicon"; Description: "Créer une icône sur le Bureau"; GroupDescription: "Icônes supplémentaires:"

[Files]
Source: "..\publish\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\Désinstaller {#MyAppName}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Lancer {#MyAppName}"; Flags: nowait postinstall skipifsilent

