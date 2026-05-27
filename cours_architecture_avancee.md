# Architecture Logicielle et Concepts Avancés

## Introduction

Ce cours aborde des concepts fondamentaux pour tout développeur souhaitant maîtriser l'architecture logicielle et écrire du code professionnel. Nous explorerons le passage par valeur et par référence, les différentes architectures applicatives, les design patterns les plus utilisés, et les bonnes pratiques d'architecture projet.

---

## 1. Passage par Valeur vs Passage par Référence

### 1.1 Comprendre la Différence Fondamentale

Lorsqu'on passe un argument à une fonction, deux mécanismes sont possibles :

| Aspect           | Passage par Valeur                       | Passage par Référence                    |
| ---------------- | ---------------------------------------- | ---------------------------------------- |
| **Copie**        | Une copie de la valeur est créée         | Aucune copie, on passe l'adresse mémoire |
| **Modification** | L'original n'est PAS modifié             | L'original PEUT être modifié             |
| **Performance**  | Coût de la copie (lent pour gros objets) | Rapide (juste une adresse)               |
| **Sécurité**     | Isolation des données                    | Risque d'effets de bord                  |

### 1.2 Types Valeur vs Types Référence

```csharp
// ═══════════════════════════════════════════════════════════════
// TYPES VALEUR (Value Types) - Stockés sur la STACK
// ═══════════════════════════════════════════════════════════════

// Les types primitifs sont des types valeur
int nombre = 42;
double prix = 19.99;
bool estActif = true;
char lettre = 'A';

// Les structs sont des types valeur
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

Point p1 = new Point { X = 10, Y = 20 };
Point p2 = p1;  // COPIE complète de p1
p2.X = 100;     // p1.X reste à 10 !

Console.WriteLine(p1.X);  // 10 (non modifié)
Console.WriteLine(p2.X);  // 100
```

```csharp
// ═══════════════════════════════════════════════════════════════
// TYPES RÉFÉRENCE (Reference Types) - Stockés sur la HEAP
// ═══════════════════════════════════════════════════════════════

// Les classes sont des types référence
public class Personne
{
    public string Nom { get; set; }
    public int Age { get; set; }
}

Personne p1 = new Personne { Nom = "Alice", Age = 30 };
Personne p2 = p1;  // COPIE de la RÉFÉRENCE (même objet!)
p2.Nom = "Bob";    // p1.Nom devient aussi "Bob"!

Console.WriteLine(p1.Nom);  // "Bob" (modifié!)
Console.WriteLine(p2.Nom);  // "Bob"
```

### 1.3 Visualisation Mémoire

```
TYPES VALEUR (Stack)                    TYPES RÉFÉRENCE (Heap)
┌─────────────────────┐                 ┌─────────────────────┐
│ Variable: nombre    │                 │ Variable: p1        │──┐
│ Valeur: 42          │                 │ Valeur: 0x7FFF001   │  │
├─────────────────────┤                 ├─────────────────────┤  │
│ Variable: p1 (Point)│                 │ Variable: p2        │──┤
│ X: 10, Y: 20        │                 │ Valeur: 0x7FFF001   │  │
├─────────────────────┤                 └─────────────────────┘  │
│ Variable: p2 (Point)│                   (même adresse!)        │
│ X: 100, Y: 20       │                        ▼                 │
└─────────────────────┘                 ┌─────────────────────┐  │
   (copie indépendante)                 │ Objet Personne      │◄─┘
                                        │ Nom: "Bob"          │
                                        │ Age: 30             │
                                        └─────────────────────┘
```

### 1.4 Passage de Paramètres en C#

```csharp
public class ExemplesPassage
{
    // ═══════════════════════════════════════════════════════════════
    // 1. Passage par VALEUR (défaut pour types valeur)
    // ═══════════════════════════════════════════════════════════════
    public void ModifierValeur(int x)
    {
        x = 999;  // Modifie uniquement la copie locale
    }

    // ═══════════════════════════════════════════════════════════════
    // 2. Passage par RÉFÉRENCE avec 'ref'
    // ═══════════════════════════════════════════════════════════════
    public void ModifierAvecRef(ref int x)
    {
        x = 999;  // Modifie la variable originale !
    }

    // ═══════════════════════════════════════════════════════════════
    // 3. Passage 'out' - doit être initialisé dans la méthode
    // ═══════════════════════════════════════════════════════════════
    public bool TryParse(string input, out int resultat)
    {
        if (int.TryParse(input, out resultat))
        {
            return true;
        }
        resultat = 0;
        return false;
    }

    public bool TryParse(string input, out int resultat)
    {

        int i = 0
        // Loop on every char
        while ( i  )
            // multiply result by 10
            // take the char add it to result

        resultat = 0;
        return false;
    }

    // ═══════════════════════════════════════════════════════════════
    // 4. Passage 'in' - référence en lecture seule (performance)
    // ═══════════════════════════════════════════════════════════════
    public double CalculerDistance(in Point p1, in Point p2)
    {
        // p1 et p2 ne peuvent pas être modifiés
        // Mais évite la copie pour les grosses structures
        return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
    }
}

// Utilisation
var demo = new ExemplesPassage();
int valeur = 42;

demo.ModifierValeur(valeur);
Console.WriteLine(valeur);  // 42 (inchangé)

demo.ModifierAvecRef(ref valeur);
Console.WriteLine(valeur);  // 999 (modifié!)
```

### 1.5 Cas Particulier : Les Tableaux et Collections

```csharp
// Les tableaux sont des types RÉFÉRENCE
public void ModifierTableau(int[] tableau)
{
    tableau[0] = 999;  // Modifie le tableau original !
}

int[] nombres = { 1, 2, 3 };
ModifierTableau(nombres);
Console.WriteLine(nombres[0]);  // 999 (modifié!)

// ✅ Pour éviter la modification : créer une copie
public void TraiterSansModifier(int[] tableau)
{
    int[] copie = (int[])tableau.Clone();
    copie[0] = 999;  // Ne modifie que la copie
}
```

### 1.6 Bonnes Pratiques

```csharp
// ✅ Utiliser des types immuables quand possible
public readonly struct PointImmuable
{
    public int X { get; }
    public int Y { get; }

    public PointImmuable(int x, int y) => (X, Y) = (x, y);

    // Retourne une nouvelle instance au lieu de modifier
    public PointImmuable Deplacer(int dx, int dy)
        => new PointImmuable(X + dx, Y + dy);
}

// ✅ Utiliser 'in' pour les grosses structures en lecture seule
public double CalculerAire(in Rectangle rect)
    => rect.Largeur * rect.Hauteur;

// ✅ Préférer les records pour les objets de données
public record PersonneRecord(string Nom, int Age);

var p1 = new PersonneRecord("Alice", 30);
var p2 = p1 with { Age = 31 };  // Crée une copie modifiée
```

---

## 2. Architectures Applicatives : Monolithe, Microservices et Monolithe Modulaire

### 2.1 Vue d'Ensemble

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         SPECTRE DES ARCHITECTURES                           │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│   MONOLITHE          MONOLITHE MODULAIRE         MICROSERVICES              │
│   ┌─────────┐        ┌──────────────────┐         ┌───┐ ┌───┐ ┌───┐         │
│   │█████████│        │ ┌───┐ ┌───┐ ┌───┐│         │ A │ │ B │ │ C │         │
│   │█████████│        │ │ A │ │ B │ │ C ││         └─┬─┘ └─┬─┘ └─┬─┘         │
│   │█████████│   ──►  │ └───┘ └───┘ └───┘│   ──►     │     │     │           │
│   │█████████│        │ (frontières)     │         ┌─┴─────┴─────┴──┐        │
│   └─────────┘        └──────────────────┘         │   Message Bus  │        │
│                                                   └────────────────┘        │
│   Simple             Équilibre                   Complexe                   │
│   Couplé             Découplé (interne)          Découplé (réseau)          │
│   1 déploiement      1 déploiement               N déploiements             │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### 2.2 Architecture Monolithique

Une application monolithique est une application **unique et unifiée** où tous les composants sont interconnectés et interdépendants.

```csharp
// ═══════════════════════════════════════════════════════════════
// ARCHITECTURE MONOLITHIQUE - Tout dans un seul projet
// ═══════════════════════════════════════════════════════════════

namespace MonApplication
{
    // Couche Présentation
    public class UtilisateurController
    {
        private readonly UtilisateurService _service;

        public UtilisateurController()
        {
            // Couplage fort : instanciation directe
            _service = new UtilisateurService();
        }

        public void CreerUtilisateur(string nom, string email)
        {
            _service.Creer(nom, email);
        }
    }

    // Couche Métier
    public class UtilisateurService
    {
        private readonly UtilisateurRepository _repo;

        public UtilisateurService()
        {
            _repo = new UtilisateurRepository();
        }

        public void Creer(string nom, string email)
        {
            // Logique métier
            var utilisateur = new Utilisateur(nom, email);
            _repo.Sauvegarder(utilisateur);

            // Envoi email directement ici (couplage!)
            var emailService = new EmailService();
            emailService.EnvoyerBienvenue(email);
        }
    }

    // Couche Données
    public class UtilisateurRepository
    {
        public void Sauvegarder(Utilisateur utilisateur)
        {
            // Accès direct à la base de données
        }
    }
}
```

**Avantages du Monolithe :**

-   ✅ Simple à développer initialement
-   ✅ Simple à déployer (un seul artefact)
-   ✅ Simple à tester de bout en bout
-   ✅ Performance (appels en mémoire, pas de latence réseau)
-   ✅ Transactions ACID faciles

**Inconvénients du Monolithe :**

-   ❌ Devient difficile à maintenir avec la croissance
-   ❌ Temps de build/déploiement qui augmente
-   ❌ Impossible de scaler indépendamment les composants
-   ❌ Un bug peut faire tomber toute l'application
-   ❌ Stack technologique unique imposée

---

### 2.3 Architecture Microservices

Les microservices divisent l'application en **services indépendants**, chacun responsable d'une fonctionnalité métier spécifique.

```csharp
// ═══════════════════════════════════════════════════════════════
// SERVICE UTILISATEUR - Projet indépendant
// ═══════════════════════════════════════════════════════════════
namespace UtilisateurService.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilisateursController : ControllerBase
    {
        private readonly IUtilisateurService _service;
        private readonly IMessageBus _messageBus;

        public UtilisateursController(
            IUtilisateurService service,
            IMessageBus messageBus)
        {
            _service = service;
            _messageBus = messageBus;
        }

        [HttpPost]
        public async Task<IActionResult> Creer(CreerUtilisateurDto dto)
        {
            var utilisateur = await _service.CreerAsync(dto);

            // Publier un événement au lieu d'appeler directement
            await _messageBus.PublierAsync(new UtilisateurCreeEvent
            {
                UtilisateurId = utilisateur.Id,
                Email = utilisateur.Email,
                Timestamp = DateTime.UtcNow
            });

            return CreatedAtAction(nameof(Get), new { id = utilisateur.Id }, utilisateur);
        }
    }
}

// ═══════════════════════════════════════════════════════════════
// SERVICE EMAIL - Projet indépendant (écoute les événements)
// ═══════════════════════════════════════════════════════════════
namespace EmailService.Handlers
{
    public class UtilisateurCreeHandler : IMessageHandler<UtilisateurCreeEvent>
    {
        private readonly IEmailSender _emailSender;

        public UtilisateurCreeHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task HandleAsync(UtilisateurCreeEvent @event)
        {
            await _emailSender.EnvoyerBienvenueAsync(@event.Email);
        }
    }
}

// ═══════════════════════════════════════════════════════════════
// SERVICE FACTURATION - Projet indépendant
// ═══════════════════════════════════════════════════════════════
namespace FacturationService.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public async Task<IActionResult> CreerFacture(int utilisateurId)
        {
            // Appel HTTP vers le service Utilisateur
            var client = _httpClientFactory.CreateClient("UtilisateurService");
            var utilisateur = await client.GetFromJsonAsync<UtilisateurDto>(
                $"api/utilisateurs/{utilisateurId}");

            // Créer la facture...
            return Ok();
        }
    }
}
```

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    ARCHITECTURE MICROSERVICES                               │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│   ┌─────────────┐       ┌─────────────┐       ┌─────────────┐               │
│   │   Client    │       │   Client    │       │   Client    │               │
│   │   (Web)     │       │  (Mobile)   │       │   (IoT)     │               │
│   └──────┬──────┘       └──────┬──────┘       └──────┬──────┘               │
│          │                     │                     │                      │
│          └─────────────────────┼─────────────────────┘                      │
│                                ▼                                            │
│                    ┌───────────────────────┐                                │
│                    │     API Gateway       │                                │
│                    │  (Authentification,   │                                │
│                    │   Rate Limiting,      │                                │
│                    │   Load Balancing)     │                                │
│                    └───────────┬───────────┘                                │
│                                │                                            │
│          ┌─────────────────────┼─────────────────────┐                      │
│          ▼                     ▼                     ▼                      │
│   ┌─────────────┐       ┌─────────────┐       ┌─────────────┐               │
│   │ Utilisateur │       │   Commande  │       │  Paiement   │               │
│   │   Service   │       │   Service   │       │   Service   │               │
│   ├─────────────┤       ├─────────────┤       ├─────────────┤               │
│   │  [Users DB] │       │ [Orders DB] │       │[Payments DB]│               │
│   └──────┬──────┘       └──────┬──────┘       └──────┬──────┘               │
│          │                     │                     │                      │
│          └─────────────────────┼─────────────────────┘                      │
│                                ▼                                            │
│                    ┌───────────────────────┐                                │
│                    │     Message Bus       │                                │
│                    │  (RabbitMQ, Kafka)    │                                │
│                    └───────────────────────┘                                │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

**Avantages des Microservices :**

-   ✅ Scalabilité indépendante de chaque service
-   ✅ Déploiement indépendant
-   ✅ Équipes autonomes (ownership)
-   ✅ Diversité technologique possible
-   ✅ Résilience (un service qui tombe n'affecte pas les autres)
-   ✅ Facilité de remplacement d'un service

**Inconvénients des Microservices :**

-   ❌ Complexité opérationnelle (DevOps, monitoring)
-   ❌ Latence réseau
-   ❌ Transactions distribuées difficiles
-   ❌ Debugging et tracing complexes
-   ❌ Duplication de données possible
-   ❌ Coût d'infrastructure plus élevé

---

### 2.4 Architecture Monolithe Modulaire

Le **Monolithe Modulaire** est un compromis intelligent : une application monolithique **structurée en modules découplés** avec des frontières claires.

```csharp
// ═══════════════════════════════════════════════════════════════
// MONOLITHE MODULAIRE - Structure
// ═══════════════════════════════════════════════════════════════

// Solution Structure:
// ├── src/
// │   ├── MonApp.Api/                    # Point d'entrée unique
// │   ├── MonApp.Shared/                 # Code partagé minimal
// │   │
// │   ├── Modules/
// │   │   ├── MonApp.Modules.Utilisateurs/
// │   │   │   ├── Domain/                # Entités, Value Objects
// │   │   │   ├── Application/           # Use Cases, DTOs
// │   │   │   ├── Infrastructure/        # Repositories, DB Context
// │   │   │   └── Api/                   # Controllers du module
// │   │   │
// │   │   ├── MonApp.Modules.Commandes/
// │   │   │   ├── Domain/
// │   │   │   ├── Application/
// │   │   │   ├── Infrastructure/
// │   │   │   └── Api/
// │   │   │
// │   │   └── MonApp.Modules.Paiements/
// │   │       ├── Domain/
// │   │       ├── Application/
// │   │       ├── Infrastructure/
// │   │       └── Api/
```

```csharp
// ═══════════════════════════════════════════════════════════════
// MODULE UTILISATEURS - Interface publique (contrat)
// ═══════════════════════════════════════════════════════════════
namespace MonApp.Modules.Utilisateurs.Contracts
{
    // Interface exposée aux autres modules
    public interface IUtilisateurModuleApi
    {
        Task<UtilisateurDto?> ObtenirParIdAsync(int id);
        Task<bool> ExisteAsync(int id);
    }

    // DTO public (pas d'entités exposées!)
    public record UtilisateurDto(int Id, string Nom, string Email);

    // Événements d'intégration
    public record UtilisateurCreeIntegrationEvent(int UtilisateurId, string Email);
}

// ═══════════════════════════════════════════════════════════════
// MODULE UTILISATEURS - Implémentation interne (privée)
// ═══════════════════════════════════════════════════════════════
namespace MonApp.Modules.Utilisateurs.Application
{
    internal class UtilisateurModuleApi : IUtilisateurModuleApi
    {
        private readonly IUtilisateurRepository _repository;

        public UtilisateurModuleApi(IUtilisateurRepository repository)
        {
            _repository = repository;
        }

        public async Task<UtilisateurDto?> ObtenirParIdAsync(int id)
        {
            var utilisateur = await _repository.ObtenirParIdAsync(id);
            return utilisateur?.ToDto();
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _repository.ExisteAsync(id);
        }
    }

    // Service interne (non exposé)
    internal class CreerUtilisateurHandler : IRequestHandler<CreerUtilisateurCommand, int>
    {
        private readonly IUtilisateurRepository _repository;
        private readonly IEventBus _eventBus;

        public async Task<int> Handle(CreerUtilisateurCommand request, CancellationToken ct)
        {
            var utilisateur = new Utilisateur(request.Nom, request.Email);
            await _repository.AjouterAsync(utilisateur);

            // Publier un événement d'intégration pour les autres modules
            await _eventBus.PublierAsync(new UtilisateurCreeIntegrationEvent(
                utilisateur.Id,
                utilisateur.Email
            ));

            return utilisateur.Id;
        }
    }
}

// ═══════════════════════════════════════════════════════════════
// MODULE COMMANDES - Utilise l'API du module Utilisateurs
// ═══════════════════════════════════════════════════════════════
namespace MonApp.Modules.Commandes.Application
{
    internal class CreerCommandeHandler : IRequestHandler<CreerCommandeCommand, int>
    {
        // Dépendance vers l'INTERFACE, pas l'implémentation
        private readonly IUtilisateurModuleApi _utilisateurModule;
        private readonly ICommandeRepository _commandeRepository;

        public CreerCommandeHandler(
            IUtilisateurModuleApi utilisateurModule,
            ICommandeRepository commandeRepository)
        {
            _utilisateurModule = utilisateurModule;
            _commandeRepository = commandeRepository;
        }

        public async Task<int> Handle(CreerCommandeCommand request, CancellationToken ct)
        {
            // Vérification via l'API du module (pas d'accès direct à la DB!)
            if (!await _utilisateurModule.ExisteAsync(request.UtilisateurId))
            {
                throw new UtilisateurInexistantException(request.UtilisateurId);
            }

            var commande = new Commande(request.UtilisateurId, request.Articles);
            await _commandeRepository.AjouterAsync(commande);

            return commande.Id;
        }
    }
}
```

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    MONOLITHE MODULAIRE                                      │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│   ┌───────────────────────────────────────────────────────────────────┐     │
│   │                         MonApp.Api                                │     │
│   │                    (Point d'entrée unique)                        │     │
│   └───────────────────────────────┬───────────────────────────────────┘     │
│                                   │                                         │
│   ┌───────────────────────────────┼───────────────────────────────────┐     │
│   │                               ▼                                   │     │
│   │  ┌─────────────┐   Communication   ┌─────────────┐                │     │
│   │  │ Module      │   via interfaces  │ Module      │                │     │
│   │  │ Utilisateur │ ◄───────────────► │ Commandes   │                │     │
│   │  ├─────────────┤       (in-memory) ├─────────────┤                │     │
│   │  │ • Domain    │                   │ • Domain    │                │     │
│   │  │ • App       │                   │ • App       │                │     │
│   │  │ • Infra     │   ┌─────────────┐ │ • Infra     │                │     │
│   │  │ • API       │   │ Module      │ │ • API       │                │     │
│   │  └──────┬──────┘   │ Paiements   │ └──────┬──────┘                │     │
│   │         │          ├─────────────┤        │                       │     │
│   │         │          │ • Domain    │        │                       │     │
│   │         │          │ • App       │        │                       │     │
│   │         │          │ • Infra     │        │                       │     │
│   │         │          │ • API       │        │                       │     │
│   │         │          └──────┬──────┘        │                       │     │
│   │         │                 │               │                       │     │
│   │         ▼                 ▼               ▼                       │     │
│   │   ┌───────────────────────────────────────────────────────┐       │     │
│   │   │            Base de données partagée OU                │       │     │
│   │   │            Schémas séparés par module                 │       │     │
│   │   └───────────────────────────────────────────────────────┘       │     │
│   │                                                                   │     │
│   └───────────────────────────────────────────────────────────────────┘     │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

**Avantages du Monolithe Modulaire :**

-   ✅ Simplicité de déploiement du monolithe
-   ✅ Découplage logique préparant une migration vers microservices
-   ✅ Frontières claires entre les domaines
-   ✅ Tests facilités par module
-   ✅ Performance (appels en mémoire)
-   ✅ Transactions simples

**Inconvénients :**

-   ❌ Discipline requise pour maintenir les frontières
-   ❌ Tentant de "tricher" et coupler les modules
-   ❌ Scalabilité toujours limitée au monolithe entier

---

### 2.5 Comparatif et Guide de Choix

| Critère                         | Monolithe | Monolithe Modulaire | Microservices |
| ------------------------------- | --------- | ------------------- | ------------- |
| **Taille d'équipe**             | 1-10 devs | 5-30 devs           | 20+ devs      |
| **Complexité du domaine**       | Faible    | Moyenne             | Élevée        |
| **Besoin de scalabilité**       | Uniforme  | Uniforme            | Différencié   |
| **Maturité DevOps**             | Débutant  | Intermédiaire       | Expert        |
| **Temps de mise sur le marché** | Rapide    | Moyen               | Long          |
| **Coût d'infrastructure**       | Faible    | Faible              | Élevé         |
| **Évolutivité future**          | Limitée   | Bonne               | Excellente    |

```
                    ARBRE DE DÉCISION
                          │
            ┌─────────────┴─────────────┐
            │ Équipe < 10 personnes ?   │
            └─────────────┬─────────────┘
                   │
         ┌────────┴────────┐
        OUI              NON
         │                 │
         ▼                 ▼
   ┌──────────┐    ┌─────────────────────┐
   │MONOLITHE │    │Domaines très        │
   │ SIMPLE   │    │différents à scaler? │
   └──────────┘    └──────────┬──────────┘
                        │
              ┌────────┴────────┐
             NON               OUI
              │                 │
              ▼                 ▼
       ┌───────────┐    ┌──────────────┐
       │ MONOLITHE │    │ MICROSERVICES│
       │ MODULAIRE │    │              │
       └───────────┘    └──────────────┘
```

---

## 3. Design Patterns les Plus Utilisés

### 3.1 Patterns de Création

#### 3.1.1 Factory Method

Délègue la création d'objets aux sous-classes.

```csharp
// Interface du produit
public interface INotification
{
    void Envoyer(string message);
}

// Implémentations concrètes
public class NotificationEmail : INotification
{
    public void Envoyer(string message)
        => Console.WriteLine($"📧 Email: {message}");
}

public class NotificationSMS : INotification
{
    public void Envoyer(string message)
        => Console.WriteLine($"📱 SMS: {message}");
}

public class NotificationPush : INotification
{
    public void Envoyer(string message)
        => Console.WriteLine($"🔔 Push: {message}");
}

// Factory
public static class NotificationFactory
{
    public static INotification[] Creer(string[] types){
        INotification[] notifications = new INotification[];
        foreach (var type in types){
            notifications.Add(Creer(type));
        }
        return notifications;
    }
    public static INotification Creer(string type) => type.ToLower() switch
    {
        "email" => new NotificationEmail(),
        "sms" => new NotificationSMS(),
        "push" => new NotificationPush(),
        _ => throw new ArgumentException($"Type inconnu: {type}")
    };
}

// Utilisation
var notification = NotificationFactory.Creer("email");
var notifications = NotificationFactory.Creer(new string[] { "email", "sms", "push" });
notification.Envoyer("Bienvenue!");
```

#### 3.1.2 Builder

Construit des objets complexes étape par étape.

```csharp
public class EmailBuilder
{
    private string _destinataire;
    private string _sujet;
    private string _corps;
    private List<string> _piecesJointes = new();
    private bool _estUrgent;
    private bool _accuseReception;

    public EmailBuilder Pour(string destinataire)
    {
        _destinataire = destinataire;
        return this;
    }

    public EmailBuilder AvecSujet(string sujet)
    {
        _sujet = sujet;
        return this;
    }

    public EmailBuilder AvecCorps(string corps)
    {
        _corps = corps;
        return this;
    }

    public EmailBuilder AjouterPieceJointe(string chemin)
    {
        _piecesJointes.Add(chemin);
        return this;
    }

    public EmailBuilder Urgent()
    {
        _estUrgent = true;
        return this;
    }

    public EmailBuilder AvecAccuseReception()
    {
        _accuseReception = true;
        return this;
    }

    public Email Construire()
    {
        if (string.IsNullOrEmpty(_destinataire))
            throw new InvalidOperationException("Destinataire requis");

        return new Email(
            _destinataire,
            _sujet ?? "(Sans sujet)",
            _corps ?? "",
            _piecesJointes,
            _estUrgent,
            _accuseReception
        );
    }
}

// Utilisation fluide et lisible
Email email = new EmailBuilder()
    .Pour("client@exemple.com")
    .AvecSujet("Votre commande")
    .AvecCorps("Merci pour votre commande!")
    .AjouterPieceJointe("/factures/2024-001.pdf")
    .Urgent()
    .AvecAccuseReception()
    .Construire();
```

#### 3.1.3 Singleton

Garantit une seule instance d'une classe.

```csharp
// ❌ Singleton classique (problèmes de testabilité)
public sealed class ConfigurationManager
{
    private static readonly Lazy<ConfigurationManager> _instance =
        new(() => new ConfigurationManager());

    public static ConfigurationManager Instance => _instance.Value;

    private ConfigurationManager()
    {
        // Chargement configuration
    }

    public string GetValue(string key) => /* ... */;
}

// ✅ Préférer l'injection de dépendances avec Singleton lifetime
public interface IConfiguration
{
    string GetValue(string key);
}

public class Configuration : IConfiguration
{
    public string GetValue(string key) => /* ... */;
}

// Dans Program.cs
services.AddSingleton<IConfiguration, Configuration>();
```

---

### 3.2 Patterns de Structure

#### 3.2.1 Adapter

Permet à des interfaces incompatibles de travailler ensemble.

```csharp
// Interface cible (ce que notre application attend)
public interface IPaymentProcessor
{
    Task<PaymentResult> ProcessAsync(decimal amount, string currency);
}

// Classe existante (API externe avec interface différente)
public class StripeGateway
{
    public async Task<StripeResponse> ChargeAsync(StripeCreditCard card, long amountInCents)
    {
        // Appel à l'API Stripe
        return new StripeResponse { Success = true, TransactionId = "txn_123" };
    }
}

// Adapter : fait le pont entre les deux
public class StripePaymentAdapter : IPaymentProcessor
{
    private readonly StripeGateway _stripe;
    private readonly StripeCreditCard _card;

    public StripePaymentAdapter(StripeGateway stripe, StripeCreditCard card)
    {
        _stripe = stripe;
        _card = card;
    }

    public async Task<PaymentResult> ProcessAsync(decimal amount, string currency)
    {
        // Conversion: notre format → format Stripe
        long amountInCents = (long)(amount * 100);

        var response = await _stripe.ChargeAsync(_card, amountInCents);

        // Conversion: format Stripe → notre format
        return new PaymentResult
        {
            IsSuccess = response.Success,
            TransactionId = response.TransactionId
        };
    }
}

// Utilisation
IPaymentProcessor processor = new StripePaymentAdapter(stripe, card);
var result = await processor.ProcessAsync(99.99m, "EUR");
```

#### 3.2.2 Decorator

Ajoute des comportements à un objet dynamiquement.

```csharp
public interface IDataService
{
    Task<string> GetDataAsync(string key);
}

// Implémentation de base
public class DatabaseService : IDataService
{
    public async Task<string> GetDataAsync(string key)
    {
        await Task.Delay(100); // Simulation appel DB
        return $"Data for {key} from DB";
    }
}

// Decorator : ajoute du caching
public class CachingDecorator : IDataService
{
    private readonly IDataService _inner;
    private readonly Dictionary<string, string> _cache = new();

    public CachingDecorator(IDataService inner) => _inner = inner;

    public async Task<string> GetDataAsync(string key)
    {
        if (_cache.TryGetValue(key, out var cached))
        {
            Console.WriteLine($"🎯 Cache hit for {key}");
            return cached;
        }

        var data = await _inner.GetDataAsync(key);
        _cache[key] = data;
        Console.WriteLine($"💾 Cached {key}");
        return data;
    }
}

public class LoggingDecorator : IDataService
{
    private readonly IDataService _inner;

    public LoggingDecorator(IDataService inner) => _inner = inner;

    public async Task<string> GetDataAsync(string key)
    {
        Console.WriteLine($"📝 Getting data for {key}...");
        var result = await _inner.GetDataAsync(key);
        Console.WriteLine($"📝 Got data: {result.Substring(0, 20)}...");
        return result;
    }
}

// Composition des decorators
IDataService service = new LoggingDecorator(new CachingDecorator(new DatabaseService()));

await service.GetDataAsync("user:123");
// Output:
// 📝 Getting data for user:123...
// 💾 Cached user:123
// 📝 Got data: Data for user:123...

await service.GetDataAsync("user:123");
// Output:
// 📝 Getting data for user:123...
// 🎯 Cache hit for user:123
// 📝 Got data: Data for user:123...
```

---

### 3.3 Patterns de Comportement

#### 3.3.1 Strategy

Permet de changer d'algorithme à l'exécution.

```csharp
public interface ITaxStrategy
{
    decimal CalculerTaxe(decimal montant);
    string Pays { get; }
}

public class TaxeFrance : ITaxStrategy
{
    public string Pays => "France";
    public decimal CalculerTaxe(decimal montant) => montant * 0.20m; // TVA 20%
}

public class TaxeAllemagne : ITaxStrategy
{
    public string Pays => "Allemagne";
    public decimal CalculerTaxe(decimal montant) => montant * 0.19m; // TVA 19%
}

public class TaxeUSA : ITaxStrategy
{
    public string Pays => "USA";
    public decimal CalculerTaxe(decimal montant) => montant * 0.08m; // Sales tax ~8%
}

// Contexte qui utilise la stratégie
public class CalculateurPrix
{
    private ITaxStrategy _taxStrategy;

    public CalculateurPrix(ITaxStrategy taxStrategy)
    {
        _taxStrategy = taxStrategy;
    }

    public void ChangerStrategie(ITaxStrategy taxStrategy)
    {
        _taxStrategy = taxStrategy;
    }

    public decimal CalculerPrixTTC(decimal prixHT)
    {
        var taxe = _taxStrategy.CalculerTaxe(prixHT);
        Console.WriteLine($"Taxe {_taxStrategy.Pays}: {taxe:C}");
        return prixHT + taxe;
    }
}

// Utilisation
var calculateur = new CalculateurPrix(new TaxeFrance());
Console.WriteLine(calculateur.CalculerPrixTTC(100)); // 120€

calculateur.ChangerStrategie(new TaxeUSA());
Console.WriteLine(calculateur.CalculerPrixTTC(100)); // 108$
```

#### 3.3.2 Observer

Définit une dépendance un-à-plusieurs entre objets.

```csharp
// Interface Observer
public interface IOrderObserver
{
    Task OnOrderCreatedAsync(Order order);
    Task OnOrderShippedAsync(Order order);
    Task OnOrderDeliveredAsync(Order order);
}

// Observers concrets
public class EmailNotifier : IOrderObserver
{
    public async Task OnOrderCreatedAsync(Order order)
        => Console.WriteLine($"📧 Email: Commande {order.Id} créée");

    public async Task OnOrderShippedAsync(Order order)
        => Console.WriteLine($"📧 Email: Commande {order.Id} expédiée");

    public async Task OnOrderDeliveredAsync(Order order)
        => Console.WriteLine($"📧 Email: Commande {order.Id} livrée");
}

public class InventoryUpdater : IOrderObserver
{
    public async Task OnOrderCreatedAsync(Order order)
        => Console.WriteLine($"📦 Stock: Réservation pour commande {order.Id}");

    public async Task OnOrderShippedAsync(Order order)
        => Console.WriteLine($"📦 Stock: Déduction pour commande {order.Id}");

    public async Task OnOrderDeliveredAsync(Order order) { /* Rien */ }
}

public class AnalyticsTracker : IOrderObserver
{
    public async Task OnOrderCreatedAsync(Order order)
        => Console.WriteLine($"📊 Analytics: Nouvelle commande {order.TotalAmount:C}");

    public async Task OnOrderShippedAsync(Order order)
        => Console.WriteLine($"📊 Analytics: Expédition enregistrée");

    public async Task OnOrderDeliveredAsync(Order order)
        => Console.WriteLine($"📊 Analytics: Livraison confirmée");
}

// Subject (Observable)
public class OrderService
{
    private readonly List<IOrderObserver> _observers = new();

    public void Subscribe(IOrderObserver observer) => _observers.Add(observer);
    public void Unsubscribe(IOrderObserver observer) => _observers.Remove(observer);

    public async Task<Order> CreateOrderAsync(OrderRequest request)
    {
        var order = new Order(request);

        // Notifier tous les observers
        foreach (var observer in _observers)
        {
            await observer.OnOrderCreatedAsync(order);
        }

        return order;
    }

    public async Task ShipOrderAsync(Order order)
    {
        order.Status = OrderStatus.Shipped;

        foreach (var observer in _observers)
        {
            await observer.OnOrderShippedAsync(order);
        }
    }
}

// Configuration
var orderService = new OrderService();
orderService.Subscribe(new EmailNotifier());
orderService.Subscribe(new InventoryUpdater());
orderService.Subscribe(new AnalyticsTracker());

// Utilisation
var order = await orderService.CreateOrderAsync(new OrderRequest { ... });
// Output:
// 📧 Email: Commande 1 créée
// 📦 Stock: Réservation pour commande 1
// 📊 Analytics: Nouvelle commande 150,00 €
```

#### 3.3.3 Repository

Abstrait l'accès aux données.

```csharp
// Interface Repository générique
public interface IRepository<T> where T : class, IEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}

// Interface spécifique avec méthodes métier
public interface IUtilisateurRepository : IRepository<Utilisateur>
{
    Task<Utilisateur?> GetByEmailAsync(string email);
    Task<IEnumerable<Utilisateur>> GetActifsAsync();
    Task<bool> EmailExisteAsync(string email);
}

// Implémentation Entity Framework
public class UtilisateurRepository : IUtilisateurRepository
{
    private readonly AppDbContext _context;

    public UtilisateurRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Utilisateur?> GetByIdAsync(int id)
        => await _context.Utilisateurs.FindAsync(id);

    public async Task<IEnumerable<Utilisateur>> GetAllAsync()
        => await _context.Utilisateurs.ToListAsync();

    public async Task<Utilisateur> AddAsync(Utilisateur entity)
    {
        _context.Utilisateurs.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Utilisateur?> GetByEmailAsync(string email)
        => await _context.Utilisateurs
            .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<IEnumerable<Utilisateur>> GetActifsAsync()
        => await _context.Utilisateurs
            .Where(u => u.EstActif)
            .ToListAsync();

    public async Task<bool> EmailExisteAsync(string email)
        => await _context.Utilisateurs
            .AnyAsync(u => u.Email == email);

    // ... autres méthodes
}

// Utilisation dans un service
public class UtilisateurService
{
    private readonly IUtilisateurRepository _repository;

    public UtilisateurService(IUtilisateurRepository repository)
    {
        _repository = repository;
    }

    public async Task<Utilisateur> CreerAsync(string nom, string email)
    {
        if (await _repository.EmailExisteAsync(email))
            throw new EmailDejaUtiliseException(email);

        var utilisateur = new Utilisateur(nom, email);
        return await _repository.AddAsync(utilisateur);
    }
}
```

---

### 3.4 Récapitulatif des Patterns

| Pattern        | Catégorie    | Quand l'utiliser                                         |
| -------------- | ------------ | -------------------------------------------------------- |
| **Factory**    | Création     | Créer des objets sans exposer la logique d'instanciation |
| **Builder**    | Création     | Construire des objets complexes étape par étape          |
| **Singleton**  | Création     | Une seule instance partagée (préférer DI)                |
| **Adapter**    | Structure    | Faire collaborer des interfaces incompatibles            |
| **Decorator**  | Structure    | Ajouter des comportements dynamiquement                  |
| **Strategy**   | Comportement | Changer d'algorithme à l'exécution                       |
| **Observer**   | Comportement | Notifier plusieurs objets d'un changement                |
| **Repository** | Architecture | Abstraire l'accès aux données                            |

---

## 4. Architecture Projet : Clean Architecture

### 4.1 Les Couches de la Clean Architecture

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         CLEAN ARCHITECTURE                                  │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│                    ┌───────────────────────────┐                            │
│                    │                           │                            │
│                    │      DOMAIN               │ ◄─── Entités, Value Objects│
│                    │   (Entités métier)        │      Règles métier pures   │
│                    │                           │      AUCUNE dépendance     │
│                    └───────────┬───────────────┘                            │
│                                │                                            │
│              ┌─────────────────┴─────────────────┐                          │
│              │                                   │                          │
│              │         APPLICATION               │ ◄─── Use Cases           │
│              │      (Cas d'utilisation)          │      Interfaces          │
│              │                                   │      DTOs                │
│              └─────────────────┬─────────────────┘                          │
│                                │                                            │
│     ┌──────────────────────────┴──────────────────────────┐                 │
│     │                                                     │                 │
│     │                  INFRASTRUCTURE                     │ ◄─── DB, APIs   │
│     │            (Détails techniques)                     │      Frameworks │
│     │                                                     │                 │
│     └─────────────────────────┬───────────────────────────┘                 │
│                               │                                             │
│            ┌──────────────────┴──────────────────┐                          │
│            │                                     │                          │
│            │            PRÉSENTATION             │ ◄─── Controllers         │
│            │         (Interface utilisateur)     │      ViewModels          │
│            │                                     │                          │
│            └─────────────────────────────────────┘                          │
│                                                                             │
│            ─────────────────────────────────────────────────────            │
│            La dépendance va TOUJOURS vers l'intérieur (Domain)              │
│            ─────────────────────────────────────────────────────            │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### 4.2 Structure de Projet Recommandée

```
Solution/
├── src/
│   ├── MonApp.Domain/                    # COUCHE DOMAINE
│   │   ├── Entities/
│   │   │   ├── Utilisateur.cs
│   │   │   ├── Commande.cs
│   │   │   └── Produit.cs
│   │   ├── ValueObjects/
│   │   │   ├── Email.cs
│   │   │   ├── Money.cs
│   │   │   └── Address.cs
│   │   ├── Enums/
│   │   │   └── OrderStatus.cs
│   │   ├── Exceptions/
│   │   │   └── DomainException.cs
│   │   └── Events/
│   │       └── UtilisateurCreeEvent.cs
│   │
│   ├── MonApp.Application/               # COUCHE APPLICATION
│   │   ├── Common/
│   │   │   ├── Interfaces/
│   │   │   │   ├── IRepository.cs
│   │   │   │   ├── IUnitOfWork.cs
│   │   │   │   └── IEmailService.cs
│   │   │   ├── Behaviors/
│   │   │   │   ├── ValidationBehavior.cs
│   │   │   │   └── LoggingBehavior.cs
│   │   │   └── Exceptions/
│   │   │       └── NotFoundException.cs
│   │   ├── Utilisateurs/
│   │   │   ├── Commands/
│   │   │   │   ├── CreerUtilisateur/
│   │   │   │   │   ├── CreerUtilisateurCommand.cs
│   │   │   │   │   ├── CreerUtilisateurHandler.cs
│   │   │   │   │   └── CreerUtilisateurValidator.cs
│   │   │   │   └── ModifierUtilisateur/
│   │   │   │       └── ...
│   │   │   ├── Queries/
│   │   │   │   ├── GetUtilisateur/
│   │   │   │   │   ├── GetUtilisateurQuery.cs
│   │   │   │   │   └── GetUtilisateurHandler.cs
│   │   │   │   └── ListUtilisateurs/
│   │   │   │       └── ...
│   │   │   └── DTOs/
│   │   │       └── UtilisateurDto.cs
│   │   └── Commandes/
│   │       └── ...
│   │
│   ├── MonApp.Infrastructure/            # COUCHE INFRASTRUCTURE
│   │   ├── Persistence/
│   │   │   ├── AppDbContext.cs
│   │   │   ├── Configurations/
│   │   │   │   └── UtilisateurConfiguration.cs
│   │   │   └── Repositories/
│   │   │       └── UtilisateurRepository.cs
│   │   ├── Services/
│   │   │   ├── EmailService.cs
│   │   │   └── DateTimeService.cs
│   │   └── DependencyInjection.cs
│   │
│   └── MonApp.Api/                       # COUCHE PRÉSENTATION
│       ├── Controllers/
│       │   └── UtilisateursController.cs
│       ├── Middleware/
│       │   └── ExceptionMiddleware.cs
│       ├── Filters/
│       ├── Program.cs
│       └── appsettings.json
│
└── tests/
    ├── MonApp.Domain.Tests/
    ├── MonApp.Application.Tests/
    ├── MonApp.Infrastructure.Tests/
    └── MonApp.Api.Tests/
```

### 4.3 Implémentation par Couche

#### Couche Domain

```csharp
// ═══════════════════════════════════════════════════════════════
// ENTITÉ - Règles métier encapsulées
// ═══════════════════════════════════════════════════════════════
namespace MonApp.Domain.Entities;

public class Utilisateur : BaseEntity
{
    public string Nom { get; private set; }
    public Email Email { get; private set; }
    public DateTime DateInscription { get; private set; }
    public bool EstActif { get; private set; }

    private readonly List<Commande> _commandes = new();
    public IReadOnlyCollection<Commande> Commandes => _commandes.AsReadOnly();

    // Constructeur privé pour forcer l'utilisation de la factory
    private Utilisateur() { }

    // Factory method avec validation
    public static Utilisateur Creer(string nom, string email)
    {
        if (string.IsNullOrWhiteSpace(nom))
            throw new DomainException("Le nom est requis");

        return new Utilisateur
        {
            Nom = nom,
            Email = Email.From(email),
            DateInscription = DateTime.UtcNow,
            EstActif = true
        };
    }

    // Méthodes métier
    public void Desactiver()
    {
        if (!EstActif)
            throw new DomainException("L'utilisateur est déjà désactivé");

        EstActif = false;
        AddDomainEvent(new UtilisateurDesactiveEvent(Id));
    }

    public Commande PasserCommande(List<LigneCommande> lignes)
    {
        if (!EstActif)
            throw new DomainException("Un utilisateur désactivé ne peut pas commander");

        var commande = Commande.Creer(this, lignes);
        _commandes.Add(commande);

        return commande;
    }
}

// ═══════════════════════════════════════════════════════════════
// VALUE OBJECT - Immutable, égalité par valeur
// ═══════════════════════════════════════════════════════════════
namespace MonApp.Domain.ValueObjects;

public record Email
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email From(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("L'email est requis");

        if (!email.Contains("@"))
            throw new DomainException("Format d'email invalide");

        return new Email(email.ToLowerInvariant().Trim());
    }

    public static implicit operator string(Email email) => email.Value;
}
```

#### Couche Application

```csharp
// ═══════════════════════════════════════════════════════════════
// COMMAND - Représente une intention de modification
// ═══════════════════════════════════════════════════════════════
namespace MonApp.Application.Utilisateurs.Commands.CreerUtilisateur;

public record CreerUtilisateurCommand(
    string Nom,
    string Email
) : IRequest<int>;

// ═══════════════════════════════════════════════════════════════
// HANDLER - Orchestre le cas d'utilisation
// ═══════════════════════════════════════════════════════════════
public class CreerUtilisateurHandler : IRequestHandler<CreerUtilisateurCommand, int>
{
    private readonly IUtilisateurRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public CreerUtilisateurHandler(
        IUtilisateurRepository repository,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<int> Handle(CreerUtilisateurCommand request, CancellationToken ct)
    {
        // Vérification métier
        if (await _repository.EmailExisteAsync(request.Email))
            throw new EmailDejaUtiliseException(request.Email);

        // Création via le Domain
        var utilisateur = Utilisateur.Creer(request.Nom, request.Email);

        // Persistance
        await _repository.AddAsync(utilisateur);
        await _unitOfWork.SaveChangesAsync(ct);

        // Action secondaire
        await _emailService.EnvoyerBienvenueAsync(utilisateur.Email);

        return utilisateur.Id;
    }
}

// ═══════════════════════════════════════════════════════════════
// VALIDATOR - Validation des entrées
// ═══════════════════════════════════════════════════════════════
public class CreerUtilisateurValidator : AbstractValidator<CreerUtilisateurCommand>
{
    public CreerUtilisateurValidator()
    {
        RuleFor(x => x.Nom)
            .NotEmpty().WithMessage("Le nom est requis")
            .MaximumLength(100).WithMessage("Le nom ne peut pas dépasser 100 caractères");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("L'email est requis")
            .EmailAddress().WithMessage("Format d'email invalide");
    }
}

// ═══════════════════════════════════════════════════════════════
// INTERFACE - Contrat pour l'infrastructure
// ═══════════════════════════════════════════════════════════════
namespace MonApp.Application.Common.Interfaces;

public interface IUtilisateurRepository
{
    Task<Utilisateur?> GetByIdAsync(int id);
    Task<bool> EmailExisteAsync(string email);
    Task AddAsync(Utilisateur utilisateur);
}

public interface IEmailService
{
    Task EnvoyerBienvenueAsync(string email);
}
```

#### Couche Infrastructure

```csharp
// ═══════════════════════════════════════════════════════════════
// REPOSITORY - Implémentation concrète
// ═══════════════════════════════════════════════════════════════
namespace MonApp.Infrastructure.Persistence.Repositories;

public class UtilisateurRepository : IUtilisateurRepository
{
    private readonly AppDbContext _context;

    public UtilisateurRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Utilisateur?> GetByIdAsync(int id)
        => await _context.Utilisateurs
            .Include(u => u.Commandes)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<bool> EmailExisteAsync(string email)
        => await _context.Utilisateurs
            .AnyAsync(u => u.Email.Value == email.ToLowerInvariant());

    public async Task AddAsync(Utilisateur utilisateur)
        => await _context.Utilisateurs.AddAsync(utilisateur);
}

// ═══════════════════════════════════════════════════════════════
// SERVICE EXTERNE - Implémentation
// ═══════════════════════════════════════════════════════════════
namespace MonApp.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task EnvoyerBienvenueAsync(string email)
    {
        // Implémentation avec SendGrid, SMTP, etc.
        Console.WriteLine($"📧 Email de bienvenue envoyé à {email}");
    }
}

// ═══════════════════════════════════════════════════════════════
// INJECTION DE DÉPENDANCES
// ═══════════════════════════════════════════════════════════════
namespace MonApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        // Repositories
        services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
```

#### Couche Présentation

```csharp
// ═══════════════════════════════════════════════════════════════
// CONTROLLER - Point d'entrée HTTP
// ═══════════════════════════════════════════════════════════════
namespace MonApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UtilisateursController : ControllerBase
{
    private readonly ISender _mediator;

    public UtilisateursController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreerUtilisateurRequest request)
    {
        var command = new CreerUtilisateurCommand(request.Nom, request.Email);
        var id = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetUtilisateurQuery(id);
        var utilisateur = await _mediator.Send(query);

        return utilisateur is null ? NotFound() : Ok(utilisateur);
    }
}
```

---

## 5. Bonnes Pratiques d'Architecture

### 5.1 Règles d'Or

| Règle                                   | Description                                                   |
| --------------------------------------- | ------------------------------------------------------------- |
| **Dépendance vers l'intérieur**         | Les couches externes dépendent des internes, jamais l'inverse |
| **Domain isolé**                        | Le Domain ne dépend de RIEN (pas d'ORM, pas de framework)     |
| **Interfaces dans Application**         | Les contrats sont définis là où ils sont utilisés             |
| **Implémentations dans Infrastructure** | Les détails techniques sont isolés                            |
| **Un seul chemin**                      | Présentation → Application → Domain (jamais de raccourcis)    |

### 5.2 Anti-Patterns à Éviter

```csharp
// ❌ ANEMIC DOMAIN MODEL - Entité sans comportement
public class UtilisateurAnemic
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Email { get; set; }
    // Juste des getters/setters, aucune logique métier
}

// Logique métier externalisée dans un service
public class UtilisateurService
{
    public void Desactiver(UtilisateurAnemic user)
    {
        user.EstActif = false; // La logique devrait être dans l'entité!
    }
}

// ✅ RICH DOMAIN MODEL
public class Utilisateur
{
    public void Desactiver()
    {
        if (!EstActif)
            throw new DomainException("Déjà désactivé");
        EstActif = false;
        AddDomainEvent(new UtilisateurDesactiveEvent(Id));
    }
}
```

```csharp
// ❌ COUPLAGE FORT - Controller accède directement au Repository
public class MauvaisController : ControllerBase
{
    private readonly AppDbContext _context; // Violation!

    public async Task<IActionResult> Get(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return Ok(user);
    }
}

// ✅ DÉCOUPLAGE - Passe par l'Application Layer
public class BonController : ControllerBase
{
    private readonly ISender _mediator;

    public async Task<IActionResult> Get(int id)
    {
        var user = await _mediator.Send(new GetUtilisateurQuery(id));
        return Ok(user);
    }
}
```

---

## Conclusion

Ce cours a couvert quatre concepts essentiels de l'architecture logicielle :

1. **Passage par Valeur/Référence** : Comprendre comment les données sont transmises est crucial pour éviter les bugs subtils et optimiser les performances.

2. **Architectures Applicatives** : Du monolithe simple aux microservices en passant par le monolithe modulaire, chaque approche a ses avantages selon le contexte.

3. **Design Patterns** : Ces solutions éprouvées (Factory, Builder, Strategy, Observer, Repository...) sont des outils indispensables pour écrire du code maintenable.

4. **Clean Architecture** : Organiser son code en couches avec des dépendances unidirectionnelles garantit un code testable et évolutif.

> **Règle d'or** : L'architecture n'est pas une fin en soi. Elle doit servir les besoins du projet et de l'équipe. Commencez simple et faites évoluer progressivement.

---

## Ressources Complémentaires

-   **Clean Architecture** - Robert C. Martin
-   **Domain-Driven Design** - Eric Evans
-   **Patterns of Enterprise Application Architecture** - Martin Fowler
-   **Building Microservices** - Sam Newman
-   **Implementing Domain-Driven Design** - Vaughn Vernon
-   [Microsoft .NET Architecture Guides](https://docs.microsoft.com/en-us/dotnet/architecture/)
-   [Refactoring Guru - Design Patterns](https://refactoring.guru/design-patterns)
