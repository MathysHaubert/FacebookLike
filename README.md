# FacebookLike

## Présentation
Application web inspirée de Facebook, développée en C# avec Blazor.

## Règles de Développement

- Respecte la structure des dossiers existants (`Components`, `Controllers`, `Models`, `Service`, etc.).
- Place chaque composant, service ou modèle dans le dossier approprié.
- Nomme les fichiers et classes de façon explicite et cohérente (ex : `Login.razor`, `UserController.cs`).
- Utilise l'anglais pour le nommage des variables, fonctions, classes et commentaires.
- Commente ton code lorsque la logique n'est pas évidente.
- Respecte les conventions de codage C# et Blazor.
- Privilégie la réutilisation du code (DRY : Don't Repeat Yourself).
- Ne travaille jamais directement sur la branche `main` ou `master`.
- Crée une branche par fonctionnalité ou correction de bug (`feature/nom-fonctionnalite`, `fix/nom-bug`).
- Avant de merger, assure-toi que le code compile et que les tests passent.
- Utilise des Pull Requests pour toute intégration sur la branche principale.
- Ne partage jamais de secrets (mots de passe, clés API, etc.) dans le code source.
- Utilise le fichier `.env` ou `appsettings.Development.json` pour les configurations sensibles.
- Ajoute les fichiers sensibles dans `.gitignore` pour éviter de les versionner.
- Écris des tests unitaires pour les services et la logique métier.
- Vérifie que l'application fonctionne avant de pousser tes modifications.
- Utilise les messages de commit clairs et explicites.
- Relis le code des autres avant d'approuver une Pull Request.
- Propose des améliorations de façon constructive.
- Valide toujours les entrées utilisateur côté serveur.
- Ne stocke jamais de mots de passe en clair.
- Utilise HTTPS pour toutes les communications sensibles.
- Documente toute nouvelle fonctionnalité ou modification majeure.
- Mets à jour le README.md si besoin.

## Lancement du Projet

1. Clone le dépôt.
2. Restaure les dépendances :
   ```bash
   dotnet restore
   ```
3. Lance l'application :
   ```bash
   dotnet run
   ```
4. Accède à l'application via `http://localhost:5000` (ou le port configuré). 