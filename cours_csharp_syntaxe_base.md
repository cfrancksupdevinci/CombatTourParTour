# C# — Rappel des syntaxes de base

## 1. Structure d’un programme

### 1.1 Programme minimal (.NET 6+)

```csharp
// Fichier Program.cs — point d’entrée
Console.WriteLine("Bonjour !");
```

### 1.2 Forme classique (namespace + classe)

```csharp
using System;

namespace MonProjet;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Bonjour !");
    }
}
```

| Élément            | Rôle                                              |
| ------------------ | ------------------------------------------------- |
| `using`            | Importe des types d’autres namespaces             |
| `namespace`        | Regroupe les types (évite les collisions de noms) |
| `class`            | Type référence (POO)                              |
| `static void Main` | Point d’entrée de l’application                   |

---

## 2. Types et variables

### 2.1 Types primitifs courants

| Mot-clé C# | .NET      | Exemple         | Notes                           |
| ---------- | --------- | --------------- | ------------------------------- |
| `int`      | `Int32`   | `42`            | Entier signé 32 bits            |
| `long`     | `Int64`   | `1_000_000L`    | Suffixe `L`                     |
| `double`   | `Double`  | `3.14`          | Virgule flottante               |
| `decimal`  | `Decimal` | `19.99m`        | Suffixe `m` — argent, précision |
| `bool`     | `Boolean` | `true`, `false` |                                 |
| `char`     | `Char`    | `'A'`           | Un seul caractère               |
| `string`   | `String`  | `"texte"`       | Chaîne immuable                 |

### 2.2 Déclaration et inférence

```csharp
int age = 25;
var nom = "Alice";           // inférence → string
const double Pi = 3.14159;   // constante compile-time

// Chaînes
string msg = $"Bonjour {nom}, tu as {age} ans.";  // interpolation
string multiligne = """
    Ligne 1
    Ligne 2
    """;
```

### 2.3 Nullable

```csharp
int? peutEtreNull = null;
string? texteOptionnel = null;

if (peutEtreNull is not null)
    Console.WriteLine(peutEtreNull.Value);

// Coalescence
int valeur = peutEtreNull ?? 0;
string s = texteOptionnel ?? "défaut";
```

### 2.4 Cast et conversion

```csharp
double d = 9.8;
int i = (int)d;                    // cast explicite → 9

string entree = "42";
int n = int.Parse(entree);
bool ok = int.TryParse("abc", out int resultat);  // ok = false
```

---

## 3. Opérateurs essentiels

```csharp
// Arithmétiques : + - * / %
// Comparaison : == != < > <= >=
// Logiques : && || !
// Assignation : = += -= *= /=
// Incrément : ++ --

int a = 10, b = 3;
Console.WriteLine(a / b);      // 3 (division entière)
Console.WriteLine(a / (double)b);  // 3.333...

// Opérateur ternaire
string statut = age >= 18 ? "majeur" : "mineur";

// Switch expression (C# 8+)
string jour = DateTime.Now.DayOfWeek switch
{
    DayOfWeek.Saturday or DayOfWeek.Sunday => "week-end",
    _ => "semaine"
};
```

---

## 4. Structures de contrôle

### 4.1 Conditions

```csharp
if (age >= 18)
    Console.WriteLine("Majeur");
else if (age >= 13)
    Console.WriteLine("Ado");
else
    Console.WriteLine("Enfant");

// switch classique
switch (note)
{
    case >= 16:
        Console.WriteLine("Très bien");
        break;
    case >= 14:
        Console.WriteLine("Bien");
        break;
    default:
        Console.WriteLine("À revoir");
        break;
}
```

### 4.2 Boucles

```csharp
// for
for (int j = 0; j < 5; j++)
    Console.Write(j);

// while / do-while
int compteur = 0;
while (compteur < 3)
{
    Console.WriteLine(compteur++);
}

// foreach (collections, tableaux)
int[] nombres = { 1, 2, 3 };
foreach (var x in nombres)
    Console.WriteLine(x);
```

### 4.3 Contrôle de flux

```csharp
for (int k = 0; k < 10; k++)
{
    if (k == 3) continue;   // passe à l’itération suivante
    if (k == 7) break;      // sort de la boucle
}
```

---

## 5. Tableaux et collections

### 5.1 Tableaux

```csharp
int[] tab = new int[3];
tab[0] = 10;

int[] primes = { 2, 3, 5, 7 };
Console.WriteLine(tab.Length);
```

### 5.2 Listes et dictionnaires

```csharp
using System.Collections.Generic;

var liste = new List<string> { "a", "b" };
liste.Add("c");
liste.Remove("a");

var ages = new Dictionary<string, int>
{
    ["Alice"] = 30,
    ["Bob"] = 25
};

if (ages.TryGetValue("Alice", out int ageAlice))
    Console.WriteLine(ageAlice);
```

### 5.3 LINQ (aperçu)

```csharp
using System.Linq;

var pairs = nombres.Where(n => n % 2 == 0).OrderBy(n => n).ToList();
int somme = nombres.Sum();
```

---

## 6. Méthodes

```csharp
// Paramètres par défaut, nommés
static int Addition(int a, int b = 0) => a + b;

static void Demo()
{
    Console.WriteLine(Addition(5));           // 5
    Console.WriteLine(Addition(b: 3, a: 2)); // 5 — arguments nommés

    // out : la méthode doit assigner la variable
    if (TryDiviser(10, 0, out double quotient))
        Console.WriteLine(quotient);

    // ref : passe une référence modifiable (variable déjà initialisée)
    int x = 1;
    Incrementer(ref x);  // x vaut 2
}

static bool TryDiviser(int a, int b, out double result)
{
    if (b == 0) { result = 0; return false; }
    result = (double)a / b;
    return true;
}

static void Incrementer(ref int n) => n++;
```

| Mot-clé  | Usage                                                      |
| -------- | ---------------------------------------------------------- |
| `ref`    | Référence en lecture/écriture                              |
| `out`    | Sortie obligatoire, pas besoin d’initialiser avant l’appel |
| `in`     | Référence en lecture seule (perf. sur gros structs)        |
| `params` | Nombre variable d’arguments : `params int[] valeurs`       |

---

## 7. Classes — syntaxe de base

### 7.1 Champs, propriétés, constructeur

```csharp
public class Personne
{
  // Champ privé (optionnel si propriété auto suffit)
    private readonly string _id;

    public string Nom { get; set; }           // propriété lecture/écriture
    public int Age { get; private set; }      // set privé

    public Personne(string nom, int age)
    {
        _id = Guid.NewGuid().ToString();
        Nom = nom;
        Age = age;
    }

    public void Afficher() => Console.WriteLine($"{Nom}, {Age} ans");
}

// Utilisation
var p = new Personne("Alice", 30);
p.Afficher();
```

### 7.2 Héritage et override (rappel court)

```csharp
public abstract class Forme
{
    public abstract double Aire();
}

public class Cercle : Forme
{
    public double Rayon { get; init; }
    public override double Aire() => Math.PI * Rayon * Rayon;
}
```

| Mot-clé                | Effet                                                         |
| ---------------------- | ------------------------------------------------------------- |
| `: BaseClass`          | Héritage                                                      |
| `virtual` / `override` | Méthode redéfinissable                                        |
| `abstract`             | Classe ou méthode sans implémentation                         |
| `sealed`               | Interdit l’héritage (classe) ou la redéfinition (méthode)     |
| `interface`            | Contrat sans implémentation par défaut (sauf `default` C# 8+) |

### 7.3 Record (type immutable concis)

```csharp
public record Point(int X, int Y);

var p1 = new Point(1, 2);
var p2 = p1 with { X = 10 };  // copie modifiée
```

---

## 8. Enum, struct, interface

```csharp
public enum StatutCommande { EnAttente, Payee, Expediee }

public struct Couleur
{
    public byte R, G, B;
}

public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine(message);
}
```

---

## 9. Exceptions

```csharp
try
{
    int x = int.Parse("abc");
}
catch (FormatException ex)
{
    Console.WriteLine($"Format invalide : {ex.Message}");
}
catch (Exception ex) when (ex.Message.Contains("test"))
{
    // filtre conditionnel
}
finally
{
  // toujours exécuté (nettoyage)
}

// Lancer une exception
throw new ArgumentException("La valeur doit être positive", nameof(valeur));
```

---

## 10. Fichiers et ressources (`using`)

```csharp
// using = import de namespace
using System.IO;

// using = IDisposable (fermeture automatique)
using var flux = File.OpenRead("data.txt");
// flux.Dispose() appelé en fin de portée
```

---

## 11. Aide-mémoire rapide

| Besoin            | Syntaxe                                |
| ----------------- | -------------------------------------- |
| Commentaire ligne | `// ...`                               |
| Commentaire bloc  | `/* ... */`                            |
| Documentation XML | `/// <summary>...</summary>`           |
| Null-check        | `if (obj is null)` ou `obj?.Methode()` |
| Pattern matching  | `if (x is int n)`                      |
| Déconstruction    | `var (a, b) = (1, 2);`                 |
| Expression-bodied | `public int Double(int x) => x * 2;`   |
| Namespace fichier | `namespace MonApp;` (sans accolades)   |

### Modificateurs d’accès

| Modificateur         | Portée                     |
| -------------------- | -------------------------- |
| `public`             | Partout                    |
| `private`            | Classe uniquement          |
| `protected`          | Classe + dérivées          |
| `internal`           | Même assembly (projet)     |
| `protected internal` | Union protected + internal |

---
