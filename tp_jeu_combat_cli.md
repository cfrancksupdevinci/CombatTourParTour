# TP — Jeu de combat tour par tour (CLI)

## Contexte

Vous développez un **jeu de rôle en console** : le joueur incarne un héros qui affronte un ou plusieurs ennemis **tour par tour**. Chaque tour, le joueur choisit une action dans un menu ; l’ennemi réagit ensuite (IA simple).

Ce TP vise à appliquer les principes du **clean code** et les **design patterns** vus en cours. Le langage attendu est **C#** (.NET 8+ ou .NET 6+), en application console.

---

## Objectifs pédagogiques

À l’issue du TP, vous devez être capable de :

1. Structurer un projet en **couches** avec des responsabilités claires (SRP).
2. Modéliser le métier (personnages, dégâts, états de combat) sans tout mélanger dans `Program.cs`.
3. Implémenter **au minimum 4 design patterns** de manière **justifiée** (pas « pour cocher une case »).
4. Produire un code **lisible, testable** et documenté (README + choix d’architecture).

---

## Règles du jeu (spécification fonctionnelle)

### 1. Démarrage

1. Saisie du **nom** du héros.
2. Choix d’une **classe** parmi au moins trois :

| Classe       | Points de vie | Attaque de base | Compétence spéciale                                                                  |
| ------------ | ------------- | --------------- | ------------------------------------------------------------------------------------ |
| **Guerrier** | 120           | 18              | _Frappe lourde_ — dégâts × 1,5, cooldown 2 tours                                     |
| **Mage**     | 80            | 12              | _Éclair_ — dégâts magiques fixes + ignore 50 % de l’armure ennemie, cooldown 3 tours |
| **Voleur**   | 90            | 14              | _Coup critique_ — 30 % de chance de doubler les dégâts, cooldown 2 tours             |

3. Lancement d’un **combat** contre **3 vagues** :
   - Vague 1 : 1 ennemi faible
   - Vague 2 : 2 ennemis
   - Vague 3 : 1 boss (PV et dégâts supérieurs)

### 2. Déroulement d’un tour

À chaque tour du **joueur** (dans cet ordre) :

1. Afficher l’**état du combat** (PV actuels / max, cooldowns, ennemis vivants).
2. Proposer un **menu numéroté** :
   - `1` — Attaque de base
   - `2` — Compétence de classe (si cooldown = 0, sinon message d’erreur)
   - `3` — Se soigner (+25 PV, **2 utilisations maximum** par combat)
   - `4` — Afficher le journal du combat (derniers événements)
3. Appliquer l’action, puis exécuter le **tour de chaque ennemi vivant** (IA).

Un **tour ennemi** consiste au minimum à : choisir une cible (le héros) et infliger des dégâts selon l’IA de l’ennemi.

### 3. Conditions de fin

| État             | Condition                                                                                                    |
| ---------------- | ------------------------------------------------------------------------------------------------------------ |
| **Victoire**     | Tous les ennemis de la vague 3 sont vaincus                                                                  |
| **Défaite**      | PV du héros ≤ 0                                                                                              |
| **Entre vagues** | Si le héros survit, PV partiellement restaurés (+20 % des PV max, arrondi supérieur) avant la vague suivante |

### 4. Affichage console (exemple)

```
══════════════════════════════════════════
  VAGUE 2/3 — Tour du joueur
══════════════════════════════════════════
  Héros : Aria (Mage)     PV : 54/80
  Cooldown compétence : 1 tour(s)
  Soins restants : 1

  Ennemis :
    [1] Gobelin        PV : 12/40
    [2] Gobelin archer PV : 28/35

  Actions :
    1. Attaque de base
    2. Éclair
    3. Se soigner
    4. Journal
  Votre choix : _
```

Les messages de résultat doivent être **explicites** (qui attaque qui, combien de dégâts, esquive, soin, mort, etc.).

---

## Exigences techniques

### Stack

- Projet **Console App** C# (`dotnet new console`).
- Aucune interface graphique.
- Persistance **optionnelle** (bonus) : meilleurs scores ou historique en JSON.

### Interdictions (anti-patterns)

| À éviter                                                                  | Pourquoi                                 |
| ------------------------------------------------------------------------- | ---------------------------------------- |
| Toute la logique dans `Main` / un seul fichier de 500+ lignes             | Violation du SRP                         |
| `switch` géant sur le type d’action **sans** polymorphisme                | Pas extensible                           |
| Nombres magiques (`if (hp < 37)`)                                         | Utiliser des constantes nommées          |
| Classes « fourre-tout » (`GameManager` qui fait UI + règles + sauvegarde) | Couplage fort                            |
| Singleton global pour l’état du jeu                                       | Préférer injection / composition         |
| Copier-coller de blocs identiques pour chaque ennemi                      | Factoriser (Factory, héritage, Strategy) |

---

## Design patterns — obligatoires

Vous devez implémenter **au moins les 4 patterns** suivants, reliés au domaine du jeu.

### 1. Strategy — Actions de combat

**Objectif** : chaque action (attaque, compétence, soin) est un algorithme interchangeable.

- Interface du type `ICombatAction` avec `string Nom`, `bool PeutExecuter(...)`, `CombatResult Executer(...)`.
- Implémentations : `AttaqueBasiqueAction`, `CompetenceMageAction`, `SoinAction`, etc.
- L’**IA ennemie** peut réutiliser la même interface (`ICombatAction` ou `IAiStrategy`) avec des implémentations différentes (`AttaqueAleatoireAi`, `AttaquePuissanteAi`).

> **Clean code** : le menu console ne calcule pas les dégâts ; il délègue à l’action choisie.

### 2. State — Machine à états du combat

**Objectif** : le déroulement du combat ne dépend pas d’une cascade de `if/else` sur des chaînes ou enums éparpillés.

États minimum :

- `TourJoueurState`
- `TourEnnemiState`
- `VictoireState`
- `DefaiteState`
- `EntreVaguesState` (transition entre vagues)

Le contexte (`CombatContext` ou `CombatSession`) délègue `Entrer()`, `Executer()`, `Transition()` à l’état courant.

### 3. Factory — Création des personnages

**Objectif** : centraliser la création du héros et des ennemis.

- `IPersonnageFactory` ou factories dédiées : `HeroFactory`, `EnemyFactory`.
- Création par type (`Guerrier`, `Gobelin`, `BossOrc`) sans `new` dispersés dans l’UI.
- Peut s’appuyer sur un **registre** (dictionnaire type → fonction de création).

### 4. Command — Encapsuler les actions du joueur

**Objectif** : séparer la **saisie utilisateur** de l’**exécution métier**.

- Interface `ICommand` : `void Executer()` ou `CombatResult Executer(CombatContext ctx)`.
- Exemples : `AttaquerCommand`, `UtiliserCompetenceCommand`, `SoignerCommand`, `AfficherJournalCommand`.
- Un **invoker** (`ActionInvoker` ou menu) mappe le choix clavier → commande.

> Permet d’ajouter plus tard : file d’actions, replay, annulation (bonus).

### 5. Observer — Journal / notifications de combat

**Objectif** : lorsqu’un événement métier se produit (dégâts, soin, mort), plusieurs « observateurs » peuvent réagir sans que le domaine connaisse la console.

- Sujet : `CombatEventPublisher` ou événements sur `CombatSession`.
- Observateurs minimum :
  - `JournalCombatObserver` (historique en mémoire)
  - `ConsoleCombatObserver` (affichage immédiat, optionnel si vous séparez affichage différé)

Événements suggérés : `DegatsInfliges`, `PersonnageSoigne`, `PersonnageVaincu`, `VagueTerminee`.

---

## Design patterns — au choix (bonus)

Implémentez **au moins un** pattern supplémentaire parmi :

| Pattern             | Usage possible dans le jeu                                                                         |
| ------------------- | -------------------------------------------------------------------------------------------------- |
| **Decorator**       | Buffs / debuffs temporaires (`BuffForce`, `Poison`) autour de `IPersonnage` ou `IDamageCalculator` |
| **Template Method** | Squelette commun `ExecuterTour()` avec étapes : début de tour → actions → fin de tour              |
| **Builder**         | Construction d’un ennemi ou d’une vague paramétrable                                               |
| **Repository**      | Sauvegarde / chargement des scores (`IScoreRepository` + fichier JSON)                             |
| **Adapter**         | Lecture de config ennemis depuis JSON alors que le domaine attend des objets `Ennemi`              |

Documentez dans le README **pourquoi** vous avez choisi ce pattern bonus.

---

## Clean Code — critères obligatoires

### SOLID (minimum attendu)

| Principe                      | Application dans le TP                                                                    |
| ----------------------------- | ----------------------------------------------------------------------------------------- |
| **S** — Single Responsibility | UI console ≠ règles de combat ≠ création des entités                                      |
| **O** — Open/Closed           | Nouvelle classe de héros ou nouvelle action sans modifier tout le projet                  |
| **L** — Liskov                | Toute implémentation de `ICombatAction` / `IPersonnage` respecte le contrat               |
| **I** — Interface Segregation | Petites interfaces (`IDamageable`, `ICombatAction`) plutôt qu’un « super-objet »          |
| **D** — Dependency Inversion  | `CombatEngine` dépend de `IEnemyAi`, `ICombatEventPublisher`, pas de classes concrètes UI |

### Qualité du code

- Méthodes **courtes** (cible : ≤ 20 lignes ; exceptions justifiées).
- Noms en **français** ou **anglais**, mais **cohérents** sur tout le projet.
- Pas de logique métier dans les setters de propriétés.
- Constantes pour les seuils (soin, cooldown, multiplicateurs).
- Gestion des entrées invalides (choix menu hors plage, lettres à la place de chiffres).

### Tests (recommandé)

Au moins **3 tests unitaires** (projet `xUnit` ou `NUnit`) sur la logique **sans** console, par exemple :

- Les dégâts d’une attaque respectent l’armure.
- Un soin ne dépasse pas les PV max.
- Le cooldown d’une compétence empêche l’exécution.

---

## Architecture projet suggérée

Structure **libre** tant que les responsabilités sont séparées. Exemple :

```
CombatTourParTour/
├── src/
│   ├── CombatTourParTour.Domain/
│   │   ├── Entites/          # Personnage, Ennemi, Heros
│   │   ├── ValueObjects/     # PointsDeVie, Degats, Cooldown
│   │   ├── Enums/            # ClasseHeraut, EtatCombat
│   │   └── Services/         # CalculateurDegats (si pur domaine)
│   │
│   ├── CombatTourParTour.Application/
│   │   ├── Combat/           # CombatSession, CombatEngine
│   │   ├── Actions/          # Strategy : ICombatAction + impl.
│   │   ├── Commands/         # Command pattern
│   │   ├── Etats/            # State pattern
│   │   ├── Factories/
│   │   ├── Ai/
│   │   └── Events/           # Observer : sujet + événements
│   │
│   ├── CombatTourParTour.Infrastructure/
│   │   ├── Console/          # Menus, saisie, rendu
│   │   └── Persistence/      # (bonus) Repository JSON
│   │
│   └── CombatTourParTour.Cli/
│       └── Program.cs        # Composition root (wiring)
│
└── tests/
    └── CombatTourParTour.Tests/
```

**Règle de dépendance** : `Domain` ne référence **aucun** autre projet ; `Application` ne référence pas `Infrastructure` ; seul `Cli` assemble le tout.

---

## Livrables

| Livrable          | Détail                                                                          |
| ----------------- | ------------------------------------------------------------------------------- |
| **Code source**   | Dépôt Git (GitHub / GitLab) ou archive zip                                      |
| **README.md**     | Installation (`dotnet run`), règles, **tableau patterns → classes**             |
| **Schéma**        | Diagramme de classes ou de séquence (photo, Mermaid, draw.io)                   |
| **Démonstration** | Capture console ou courte vidéo d’une partie complète (optionnel mais apprécié) |

### Modèle de tableau à inclure dans le README

| Pattern  | Classe(s) / interface(s) | Rôle dans le jeu |
| -------- | ------------------------ | ---------------- |
| Strategy | `ICombatAction`, `...`   | …                |
| State    | `ICombatState`, `...`    | …                |
| Factory  | `EnemyFactory`, `...`    | …                |
| Command  | `ICommand`, `...`        | …                |
| Observer | `...`                    | …                |

---

## Barème indicatif (/20)

| Critère                                                   | Points |
| --------------------------------------------------------- | ------ |
| Jeu fonctionnel (vagues, menu, win/lose)                  | 5      |
| 4 patterns obligatoires correctement appliqués            | 6      |
| Clean Code (structure, nommage, SRP, pas de god class)    | 4      |
| SOLID / découplage UI ↔ métier                            | 2      |
| README + diagramme + justification des choix              | 2      |
| Bonus (tests, pattern supplémentaire, persistance scores) | +2 max |

**Pénalités** : `-1` par pattern « fantôme » (classe vide sans usage réel), `-2` si tout est dans `Program.cs`.

