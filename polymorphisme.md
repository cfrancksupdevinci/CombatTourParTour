# Le Polymorphisme en Programmation Orientée Objet

## Introduction

Le polymorphisme est l'un des quatre piliers fondamentaux de la programmation orientée objet (POO), aux côtés de l'**encapsulation**, l'**abstraction** et l'**héritage**.

> **Étymologie** : Du grec _poly_ (plusieurs) et _morphê_ (forme) — littéralement "plusieurs formes".

Le polymorphisme permet à une entité (méthode, opérateur, objet) de se comporter différemment selon le contexte.

---

## 1. Les Différents Types de Polymorphisme

### 1.1 Polymorphisme de Sous-typage (ou d'Héritage)

C'est la forme la plus courante en POO. Un objet d'une classe dérivée peut être traité comme un objet de sa classe parente.

```csharp
public abstract class Animal
{
    public abstract string Parler();
}

public class Chien : Animal
{
    public override string Parler() => "Wouf!";
}

public class Chat : Animal
{
    public override string Parler() => "Miaou!";
}

public class Vache : Animal
{
    public override string Parler() => "Meuh!";
}

// Polymorphisme en action
public static class Programme
{
    public static string FaireParler(Animal animal) => animal.Parler();

    public static void Main()
    {
        Animal[] animaux = { new Chien(), new Chat(), new Vache() };

        foreach (var animal in animaux)
        {
            Console.WriteLine(FaireParler(animal));
        }
        // Output: Wouf! Miaou! Meuh!
    }
}
```

**Point clé** : La méthode `FaireParler` ne connaît pas le type concret de l'animal. Elle appelle `Parler()` et c'est l'objet lui-même qui détermine le comportement.

---

### 1.2 Polymorphisme Ad-hoc (Surcharge de Méthodes)

Permet d'avoir plusieurs méthodes avec le même nom mais des signatures différentes.

```csharp
public class Calculatrice
{
    public int Additionner(int a, int b) => a + b;

    public double Additionner(double a, double b) => a + b;

    public string Additionner(string a, string b) => a + b; // Concaténation

    public int Additionner(int a, int b, int c) => a + b + c;
}

// Utilisation
var calc = new Calculatrice();
Console.WriteLine(calc.Additionner(5, 3));           // 8
Console.WriteLine(calc.Additionner(5.5, 3.2));       // 8.7
Console.WriteLine(calc.Additionner("Hello", " World")); // Hello World
Console.WriteLine(calc.Additionner(1, 2, 3));        // 6
```

---

### 1.3 Polymorphisme Paramétrique (Génériques)

Permet d'écrire du code qui fonctionne avec n'importe quel type.

```csharp
public static class CollectionUtils
{
    public static T? Premier<T>(T[] tableau)
    {
        return tableau.Length > 0 ? tableau[0] : default;
    }

    public static T? Dernier<T>(IList<T> liste)
    {
        return liste.Count > 0 ? liste[^1] : default;
    }
}

// Fonctionne avec n'importe quel type
int? premierInt = CollectionUtils.Premier<int>(new[] { 1, 2, 3 });
string? premierString = CollectionUtils.Premier(new[] { "a", "b", "c" });
bool? premierBool = CollectionUtils.Premier(new[] { true, false });
```

**Contraintes génériques** :

```csharp
// Interface déjà fournie par C# dans System
public interface IComparable<T>
{
    int CompareTo(T autre);
}

public static T Max<T>(T a, T b) where T : IComparable<T>
{
    return a.CompareTo(b) > 0 ? a : b;
}
```

---

### 1.4 Polymorphisme de Coercition

Conversion implicite de types par le langage.

```csharp
// C# convertit automatiquement int en double
double resultat = 5 + 3.14; // int + double = double (8.14)

// Conversions implicites personnalisées
public readonly struct Celsius
{
    public double Valeur { get; }

    public Celsius(double valeur) => Valeur = valeur;

    // implicit : conversion automatique sans cast
    public static implicit operator Fahrenheit(Celsius c)
        => new Fahrenheit(c.Valeur * 9 / 5 + 32);
}

public readonly struct Fahrenheit
{
    public double Valeur { get; }

    public Fahrenheit(double valeur) => Valeur = valeur;

    // explicit : nécessite un cast explicite
    public static explicit operator Celsius(Fahrenheit f)
        => new Celsius((f.Valeur - 32) * 5 / 9);
}

// Utilisation
Celsius celsius = new(100);
Fahrenheit fahrenheit = celsius;         // Conversion implicite (pas de cast)
Celsius retour = (Celsius)fahrenheit;    // Conversion explicite (cast requis)


```

---

## 2. Le Polymorphisme en Pratique

### 2.1 Le Principe de Substitution de Liskov (LSP)

> "Les objets d'une classe dérivée doivent pouvoir remplacer les objets de la classe de base sans altérer le comportement correct du programme."

**Exemple de violation du LSP :**

```csharp
public class Rectangle
{
    public virtual double Largeur { get; set; }
    public virtual double Hauteur { get; set; }

    public Rectangle(double largeur, double hauteur)
    {
        Largeur = largeur;
        Hauteur = hauteur;
    }

    public double Aire() => Largeur * Hauteur;
}

public class Carre : Rectangle
{
    public override double Largeur
    {
        get => base.Largeur;
        set
        {
            base.Largeur = value;
            base.Hauteur = value; // Violation: comportement inattendu
        }
    }

    public override double Hauteur
    {
        get => base.Hauteur;
        set
        {
            base.Largeur = value;
            base.Hauteur = value;
        }
    }

    public Carre(double cote) : base(cote, cote) { }
}

// Ce code ne fonctionne pas comme attendu
public static double DoublerLargeur(Rectangle rect)
{
    rect.Largeur *= 2;
    return rect.Aire();
}

var r = new Rectangle(4, 5);
Console.WriteLine(DoublerLargeur(r)); // 40 ✓

var c = new Carre(4);
Console.WriteLine(DoublerLargeur(c)); // Attendu: 40, Obtenu: 64 ✗
```

**Solution** : Repenser la hiérarchie avec une interface commune.

```csharp
// Interface commune définissant le contrat
public interface IForme
{
    double Aire();
    double Perimetre();
}

// Rectangle et Carré sont des classes indépendantes
public class Rectangle : IForme
{
    public double Largeur { get; }
    public double Hauteur { get; }

    public Rectangle(double largeur, double hauteur)
    {
        Largeur = largeur;
        Hauteur = hauteur;
    }

    public double Aire() => Largeur * Hauteur;
    public double Perimetre() => 2 * (Largeur + Hauteur);
}

public class Carre : IForme
{
    public double Cote { get; }

    public Carre(double cote) => Cote = cote;

    public double Aire() => Cote * Cote;
    public double Perimetre() => 4 * Cote;
}

// Utilisation polymorphique respectant LSP
public static void AfficherInfos(IForme forme)
{
    Console.WriteLine($"Aire: {forme.Aire()}, Périmètre: {forme.Perimetre()}");
}

IForme[] formes = { new Rectangle(4, 5), new Carre(4) };
foreach (var forme in formes)
{
    AfficherInfos(forme); // Fonctionne correctement pour tous les types ✓
}
```

**Pourquoi ça respecte LSP ?**

- `Carre` n'hérite plus de `Rectangle` → pas de comportement hérité à "casser"
- Chaque classe implémente son propre comportement cohérent
- Les propriétés sont **immutables** (`get` uniquement) → pas d'effets de bord

---

### 2.2 Interfaces

Les interfaces définissent un contrat que les classes doivent respecter.

```csharp
public interface IDessinable
{
    string Dessiner();
    double Aire();
}

public class Cercle : IDessinable
{
    public double Rayon { get; }

    public Cercle(double rayon) => Rayon = rayon;

    public string Dessiner() => $"○ (rayon={Rayon})";

    public double Aire() => Math.PI * Rayon * Rayon;
}

public class RectangleForme : IDessinable
{
    public double Largeur { get; }
    public double Hauteur { get; }

    public RectangleForme(double largeur, double hauteur)
    {
        Largeur = largeur;
        Hauteur = hauteur;
    }

    public string Dessiner() => $"▭ ({Largeur}x{Hauteur})";

    public double Aire() => Largeur * Hauteur;
}

// Utilisation polymorphique
public static void AfficherFormes(IEnumerable<IDessinable> formes)
{
    foreach (var forme in formes)
    {
        Console.WriteLine($"{forme.Dessiner()} - Aire: {forme.Aire():F2}");
    }
}

IDessinable[] formes = { new Cercle(5), new RectangleForme(4, 6), new Cercle(3) };
AfficherFormes(formes);
```

---

### 2.3 Classes Abstraites vs Interfaces

```csharp
// Classe abstraite : partage de code + contrat
public abstract class Vehicule
{
    public string Marque { get; }

    protected Vehicule(string marque) => Marque = marque;

    // Méthode concrète partagée
    public void Demarrer() => Console.WriteLine($"{Marque} démarre...");

    // Méthode abstraite à implémenter
    public abstract void Rouler();
}

// Interface : contrat uniquement (multi-implémentation possible)
public interface IVolant
{
    void Voler();
}

public interface INageant
{
    void Nager();
}

// Une classe peut implémenter plusieurs interfaces
public class VoitureAmphibie : Vehicule, INageant
{
    public VoitureAmphibie(string marque) : base(marque) { }

    public override void Rouler() => Console.WriteLine("Roule sur la route");

    public void Nager() => Console.WriteLine("Navigue sur l'eau");
}

public class Avion : Vehicule, IVolant
{
    public Avion(string marque) : base(marque) { }

    public override void Rouler() => Console.WriteLine("Roule sur la piste");

    public void Voler() => Console.WriteLine("Vole dans les airs");
}
```

---

## 3. Patterns Utilisant le Polymorphisme

### 3.1 Pattern Strategy

Permet de changer dynamiquement l'algorithme utilisé.

```csharp
public interface IStrategieTriage<T>
{
    IEnumerable<T> Trier(IEnumerable<T> donnees);
}

public class TriCroissant<T> : IStrategieTriage<T> where T : IComparable<T>
{
    public IEnumerable<T> Trier(IEnumerable<T> donnees)
        => donnees.OrderBy(x => x);
}

public class TriDecroissant<T> : IStrategieTriage<T> where T : IComparable<T>
{
    public IEnumerable<T> Trier(IEnumerable<T> donnees)
        => donnees.OrderByDescending(x => x);
}

public class TriParLongueur : IStrategieTriage<string>
{
    public IEnumerable<string> Trier(IEnumerable<string> donnees)
        => donnees.OrderBy(x => x.Length);
}

public class Trieur<T>
{
    private IStrategieTriage<T> _strategie;

    public Trieur(IStrategieTriage<T> strategie) => _strategie = strategie;

    public void DefinirStrategie(IStrategieTriage<T> strategie)
        => _strategie = strategie;

    public IEnumerable<T> Executer(IEnumerable<T> donnees)
        => _strategie.Trier(donnees);

}

// Utilisation
var trieur = new Trieur<int>(new TriCroissant<int>());
var resultat1 = trieur.Executer(new[] { 3, 1, 4, 1, 5 });
Console.WriteLine(string.Join(", ", resultat1)); // 1, 1, 3, 4, 5

trieur.DefinirStrategie(new TriDecroissant<int>());
var resultat2 = trieur.Executer(new[] { 3, 1, 4, 1, 5 });
Console.WriteLine(string.Join(", ", resultat2)); // 5, 4, 3, 1, 1
```

---

### 3.2 Pattern Factory

Crée des objets sans exposer la logique d'instanciation.

```csharp
public interface IDocument
{
    string Ouvrir();
    string Sauvegarder();
}

public class DocumentPDF : IDocument
{
    public string Ouvrir() => "Ouverture du PDF avec lecteur PDF";

    public string Sauvegarder() => "Sauvegarde au format PDF";
}

public class DocumentWord : IDocument
{
    public string Ouvrir() => "Ouverture avec Microsoft Word";

    public string Sauvegarder() => "Sauvegarde au format DOCX";
}

public class DocumentTexte : IDocument
{
    public string Ouvrir() => "Ouverture avec éditeur de texte";

    public string Sauvegarder() => "Sauvegarde au format TXT";
}

public static class DocumentFactory
{
    private static readonly Dictionary<string, Func<IDocument>> _types = new()
    {
        ["pdf"] = () => new DocumentPDF(),
        ["docx"] = () => new DocumentWord(),
        ["txt"] = () => new DocumentTexte()
    };

    public static IDocument Creer(string extension)
    {
        if (!_types.TryGetValue(extension.ToLower(), out var factory))
            throw new ArgumentException($"Type de document non supporté: {extension}");

        return factory();
    }
}

// Utilisation
// listdir ./
// recuperer les fichiers
var listeFichiers = Directory.GetFiles("./");
foreach (var fichier in listeFichiers)
{
    var extension = Path.GetExtension(fichier);
    var doc = DocumentFactory.Creer(extension);
    Console.WriteLine(doc.Ouvrir());
}
var doc = DocumentFactory.Creer("pdf");
Console.WriteLine(doc.Ouvrir()); // Ouverture du PDF avec lecteur PDF
```

---

### 3.3 Pattern Visitor

Permet d'ajouter des opérations à une hiérarchie de classes sans les modifier.

```csharp
public interface IFormeVisitor
{
    void Visiter(Cercle cercle);
    void Visiter(CarreForme carre);
    void Visiter(TriangleForme triangle);
}

public interface IFormeVisitable
{
    void Accepter(IFormeVisitor visitor);
}

public class Cercle : IFormeVisitable
{
    public double Rayon { get; }

    public Cercle(double rayon) => Rayon = rayon;

    public void Accepter(IFormeVisitor visitor) => visitor.Visiter(this);
}

public class CarreForme : IFormeVisitable
{
    public double Cote { get; }

    public CarreForme(double cote) => Cote = cote;

    public void Accepter(IFormeVisitor visitor) => visitor.Visiter(this);
}

public class TriangleForme : IFormeVisitable
{
    public double Base { get; }
    public double Hauteur { get; }

    public TriangleForme(double @base, double hauteur)
    {
        Base = @base;
        Hauteur = hauteur;
    }

    public void Accepter(IFormeVisitor visitor) => visitor.Visiter(this);
}

// Visiteur calculant l'aire
public class CalculateurAire : IFormeVisitor
{
    public double ResultatTotal { get; private set; }

    public void Visiter(Cercle cercle)
        => ResultatTotal += Math.PI * cercle.Rayon * cercle.Rayon;

    public void Visiter(CarreForme carre)
        => ResultatTotal += carre.Cote * carre.Cote;

    public void Visiter(TriangleForme triangle)
        => ResultatTotal += triangle.Base * triangle.Hauteur / 2;
}

// Utilisation
IFormeVisitable[] formes = { new Cercle(5), new CarreForme(4), new TriangleForme(3, 6) };
var calculateur = new CalculateurAire();

foreach (var forme in formes)
{
    forme.Accepter(calculateur);
}

Console.WriteLine($"Aire totale: {calculateur.ResultatTotal:F2}");
```

---

## 4. Polymorphisme avec les Mots-Clés C#

### 4.1 virtual, override et new

```csharp
public class Base
{
    public virtual void MethodeVirtuelle()
        => Console.WriteLine("Base.MethodeVirtuelle");

    public void MethodeNonVirtuelle()
        => Console.WriteLine("Base.MethodeNonVirtuelle");
}

public class Derivee : Base
{
    // Override: polymorphisme vrai
    public override void MethodeVirtuelle()
        => Console.WriteLine("Derivee.MethodeVirtuelle");

    // new: masquage (hiding) - PAS du polymorphisme
    public new void MethodeNonVirtuelle()
        => Console.WriteLine("Derivee.MethodeNonVirtuelle");
}

Base obj = new Derivee();
obj.MethodeVirtuelle();      // "Derivee.MethodeVirtuelle" (polymorphisme)
obj.MethodeNonVirtuelle();   // "Base.MethodeNonVirtuelle" (pas de polymorphisme)
```

### 4.2 sealed

```csharp
public class Animal
{
    public virtual void Parler() => Console.WriteLine("...");
}

public class Chien : Animal
{
    // sealed empêche les sous-classes de redéfinir cette méthode
    public sealed override void Parler() => Console.WriteLine("Wouf!");
}

public class ChienDressé : Chien
{
    // Erreur de compilation: impossible de redéfinir Parler()
    // public override void Parler() => Console.WriteLine("Wouf wouf!");
}
```

---

## 5. Avantages du Polymorphisme

| Avantage            | Description                                                   |
| ------------------- | ------------------------------------------------------------- |
| **Extensibilité**   | Ajouter de nouveaux types sans modifier le code existant      |
| **Maintenabilité**  | Code plus propre et plus facile à comprendre                  |
| **Réutilisabilité** | Écrire du code générique utilisable dans différents contextes |
| **Testabilité**     | Facilite l'injection de dépendances et les mocks              |
| **Découplage**      | Réduit les dépendances entre les composants                   |

---

## 6. Exercices Pratiques

### Exercice 1 : Système de Paiement

Implémentez un système de paiement polymorphique supportant :

- Carte bancaire
- PayPal
- Virement bancaire

```csharp
// Squelette à compléter
public interface IMethodePaiement
{
    bool Payer(decimal montant);
    bool Rembourser(decimal montant);
    string NomMethode { get; }
}
```

### Exercice 2 : Système de Notifications

Créez un système de notifications polymorphique :

- Email
- SMS
- Push notification

### Exercice 3 : Calculatrice d'Aires

Implémentez différentes formes géométriques avec calcul d'aire et de périmètre.

---

## 7. Bonnes Pratiques

1. **Privilégier la composition à l'héritage** quand c'est possible
2. **Respecter le principe de Liskov** (LSP)
3. **Programmer vers des interfaces**, pas des implémentations
4. **Utiliser les génériques** pour le code réutilisable
5. **Utiliser `sealed`** quand l'héritage n'est pas prévu
6. **Éviter `new`** pour le masquage de méthodes

---

## Conclusion

Le polymorphisme est un outil puissant qui, bien utilisé, permet d'écrire du code :

- **Flexible** : s'adapte aux changements
- **Extensible** : facile à étendre
- **Maintenable** : facile à comprendre et modifier

La maîtrise du polymorphisme est essentielle pour écrire du code orienté objet de qualité professionnelle.

---

## Ressources Complémentaires

- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [Design Patterns - Gang of Four](https://refactoring.guru/design-patterns)
- [Microsoft C# Documentation - Polymorphism](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/polymorphism)
- [Microsoft C# Documentation - Interfaces](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface)
