# StageLink — Rapport d'Activité 8

**Application mobile Android de recherche et de suivi de stages** — .NET 8 MAUI, MVVM, Shell
Auteur : Idriss Kamfo — Génie Logiciel — Dépôt GitHub : `<lien du dépôt>`

---

## 1. Objectifs

**Objectif fonctionnel.** StageLink met en relation des étudiants en recherche de stage (PFE de 6 mois) et des entreprises. L'application permet de :

- créer un compte étudiant et se connecter (profils Étudiant / Recruteur) ;
- consulter, filtrer et sauvegarder des offres de stage (poste, entreprise, durée, gratification, compétences) ;
- rechercher par mot-clé et par filtres cumulables (domaine, durée, compétence) ;
- postuler en un geste et suivre l'avancement des candidatures (Envoyée, En attente, Entretien, Acceptée) ;
- gérer son profil (compétences, CV, préférences) ;
- paramétrer des alertes programmées, consulter les notifications et activer un mode nuit silencieux (sobriété numérique).

**Objectifs techniques.**

- Cible unique `net8.0-android`, déploiement par APK sur appareil physique.
- Architecture **MVVM stricte** avec `CommunityToolkit.Mvvm` (`[ObservableProperty]`, `[RelayCommand]`, classes partielles), séparation `Views/`, `ViewModels/`, `Models/`, `Services/`.
- Navigation exclusivement par **MAUI Shell** (barre d'onglets + routes poussées appelées par chaînes brutes).
- Conversion fidèle de 8 maquettes HTML/CSS (thème sombre vert StageLink) en XAML natif.
- Livrable de démonstration : visite guidée automatique de 30 s filmable avec `adb screenrecord`.

## 2. Taux de réalisation : 100 %

Les 8 écrans du périmètre défini ont été réalisés, reliés par la navigation et testables de bout en bout.

| Maquette | Écran / ViewModel | État |
|---|---|---|
| connexion.html | `LoginPage` / `LoginViewModel` (rôles, erreur, mot de passe, mémorisation) | Réalisé |
| inscription.html | `RegisterPage` / `RegisterViewModel` (validation, jauge de robustesse) | Réalisé |
| offre.html | `OffersPage` / `OffersViewModel` (recherche, filtres, favoris) | Réalisé |
| recherche.html | `SearchPage` / `SearchViewModel` (filtres cumulables, état vide) | Réalisé |
| detail.html | `OfferDetailPage` / `OfferDetailViewModel` (postuler, favori) | Réalisé |
| post.html | `ApplicationsPage` / `ApplicationsViewModel` (statuts, compteurs) | Réalisé |
| profil.html | `ProfilePage` / `ProfileViewModel` | Réalisé |
| alerte.html | `AlertsPage` / `AlertsViewModel` (alertes, notifications, mode nuit) | Réalisé |

Autres éléments livrés : `StageLink.csproj` configuré, `AndroidManifest.xml` (minSdk 21, targetSdk 34), `MainActivity` / `MainApplication`, SVG de contournement du Resizetizer (`splash.svg`, `appicon.svg`, `appiconfg.svg`), scripts `build-apk.sh` et `record-demo.sh`.

**Périmètre.** Le projet fonctionne sans serveur : les offres et notifications sont un jeu d'essai en mémoire, et les comptes sont enregistrés localement (mots de passe hachés SHA-256). Le raccordement à une API REST fait partie des perspectives.

## 3. Difficultés rencontrées et solutions

**Configuration du SDK Android sous Linux (Crostini).** Aucun Android Studio n'est disponible dans le conteneur. Il a fallu installer manuellement le JDK 17, les `cmdline-tools`, accepter les licences avec `sdkmanager`, installer `platform-tools`, `platforms;android-34` et `build-tools`, puis installer le workload `maui-android`. Le SDK n'étant pas détecté automatiquement, chaque commande reçoit `-p:AndroidSdkDirectory="$HOME/android-sdk"`. Le téléphone est relié en USB (partage d'appareil Crostini) pour `adb`.

**Mémoire vive limitée.** Les builds Release standards faisaient tuer le processus par le noyau (**Exit Code 137**, manque de RAM) lors de la compilation et de l'édition de liens. Solutions : build unique ciblant `net8.0-android`, un seul framework, pas de compilation parallèle superflue, emballage des assemblies dans l'APK, et surtout désactivation des optimisations coûteuses (ci-dessous).

**Optimisations AOT et Trimming.** `PublishTrimmed` et `RunAOTCompilation` consomment plusieurs Go de RAM. Ils sont désactivés dans le `.csproj` et dans la commande :
`dotnet build -f net8.0-android -c Release -t:SignAndroidPackage -p:AndroidSdkDirectory="$HOME/android-sdk" -p:AndroidPackageFormat=apk -p:PublishTrimmed=false -p:RunAOTCompilation=false`.
Contrepartie : APK plus volumineux et démarrage un peu plus lent, acceptables pour un prototype.

**Tâche Resizetizer.** L'absence d'icône ou d'écran de démarrage vectoriels faisait échouer le build ; des SVG de contournement (`appicon.svg`, `appiconfg.svg`, `splash.svg`) ont été ajoutés, ainsi qu'un nom d'activité fixe pour que l'icône de lancement s'affiche.

**Navigation Shell et erreurs CS0103.** `nameof(Page)` provoquait des erreurs d'espace de noms ; les routes sont désormais déclarées et appelées par chaînes brutes (`GoToAsync("RegisterPage")`).

**Traduction HTML/Tailwind vers XAML.** Pas d'équivalent des classes utilitaires, des polices d'icônes Material ni des états `hover`. Réponses : ressources centralisées (couleurs, styles), composants réutilisables (`AppHeader`, `OfferCard`, `FilterChipView`), `DataTrigger` pour les états sélectionnés, pictogrammes SVG/Unicode.

## 4. Conclusion

StageLink atteint 100 % du périmètre fixé : 8 écrans MVVM, navigation Shell complète, APK installable sur téléphone physique malgré les contraintes matérielles. Perspectives : API REST et base SQLite, espace recruteur complet, notifications push, tests unitaires des ViewModels.
