# CombatTourParTour

CombatTourParTour est un jeu de rôle en console en C# : le joueur incarne un héros qui affronte un ou plusieurs ennemis tour par tour. Chaque tour, le joueur choisit une action dans un menu ; l’ennemi réagit ensuite (IA simple).

# Exécuter le projet

Prérequis :

- Avoir le SDK .NET installé (dotnet --version)

Depuis la racine du dépôt :

1. Se placer dans le dossier du projet

cd .\CombatTourParTour\

2. Lancer l'application

dotnet run

# Design Patterns utilisés

| Pattern             | Classe(s) / interface(s)                                               | Rôle dans le jeu                                                           | Bénéfice                                                           |
| ------------------- | ---------------------------------------------------------------------- | -------------------------------------------------------------------------- | ------------------------------------------------------------------ |
| State               | ICombatState, HeroTurnState, EnemyTurnState, VictoryState, DefeatState | Gérer les phases du combat et les transitions de tour                      | Évite les cascades de if/else et clarifie le flux                  |
| Command (simplifié) | HeroCommands, HeroCommandChoice                                        | Mapper le choix utilisateur vers une action de combat                      | Découple la saisie utilisateur de l'exécution                      |
| Strategy            | HeroBasicAttack, HeroSpecialAttack                                     | Appliquer des règles de dégâts différentes selon l'action                  | Permet de faire évoluer les attaques sans impacter le reste        |
| Factory             | HeroFactory, EnemyFactory                                              | Centraliser la création des héros et des ennemis                           | Évite les new dispersés et uniformise la création                  |
| Observer            | Waves (publisher), AttackFlow (subscriber)                             | Publier des événements de combat (CombatWon, CombatLost, etc.) et y réagir | Sépare la logique métier des effets de bord (affichage, réactions) |
