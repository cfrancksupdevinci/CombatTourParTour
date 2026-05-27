# CombatTourParTour

CombatTourParTour est un jeu de rôle en console en C# : le joueur incarne un héros qui affronte un ou plusieurs ennemis tour par tour. Chaque tour, le joueur choisit une action dans un menu ; l’ennemi réagit ensuite (IA simple).

# Design Patterns utilisé

- State pour stocker dans le store l'état de l'objet (ex: le suivie en direct de l'état de point de vie des joueurs, des soins restants, les victoires les tours).
- Command pour pouvoir utiliser un standalone et suivre toutes les informations d'une requête(ex: récupération des informations d'un attaques spécials par classe choisit).
- Strategy pour les actions joueurs et l'IA
- Factory pour créer les Hero et ennemies
- Observer pour le journal
- Prototype pour clonner les ennemies et mieux gérer leur pv_max et pv_actuel. ?
