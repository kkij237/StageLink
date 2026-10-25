# StageLink

Application mobile Android (.NET 8 MAUI, MVVM / CommunityToolkit.Mvvm, Shell) de recherche et de suivi de stages.

## Prérequis (Crostini)
- JDK 17, SDK Android dans `~/android-sdk` (platform-tools, platforms;android-34, build-tools)
- `dotnet workload install maui-android`

## Build de l'APK (sans Trimming ni AOT)
```bash
dotnet build -f net8.0-android -c Release -t:SignAndroidPackage -p:AndroidSdkDirectory="$HOME/android-sdk" -p:AndroidPackageFormat=apk -p:PublishTrimmed=false -p:RunAOTCompilation=false
```
APK : `bin/Release/net8.0-android/com.stagelink.app-Signed.apk`

## Compte de démonstration
`idriss.kamfo7@univ-tech.edu` / `StageLink2026` (ou créer un compte via « Créer un compte »)

## Vidéo de démo de 30 s
```bash
./scripts/record-demo.sh            # installe l'APK, filme 30 s, rapatrie stagelink-demo.mp4
```
Sans adb : bouton « ▶ Lancer la démo (30 s) » sur l'écran de connexion, puis enregistreur d'écran Android.

## Structure
`Views/` (XAML + code-behind, `Components/`), `ViewModels/`, `Models/`, `Services/`, `Converters/`, `Platforms/Android/`, `Resources/`, `scripts/`, `docs/RAPPORT.md`
