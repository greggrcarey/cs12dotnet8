namespace Packt.Shared;

public class Person : IComparable<Person?>
{
    #region Properties

    public string? Name { get; set; }
    public DateTimeOffset Born { get; set; }
    public List<Person> Spouses = new();
    public List<Person> Children = new();
    public bool Married => Spouses.Count > 0;
    #endregion

    #region Methods

    public void WriteToConsole()
    {
        WriteLine($"{Name} was born on a {Born:dddd}.");
    }

    public void WriteChildernToConsole()
    {
        string term = Children.Count == 1 ? "child" : "children";
        WriteLine($"{Name} has {Children.Count} {term}");
    }
    /// <summary>
    /// Static method to "multiply" aka procreate and have a child together.
    /// </summary>
    /// <param name="p1">Parent 1</param>
    /// <param name="p2">Parent 2</param>
    /// <returns>A Person object that is the child of Parent 1 and Parent2.</returns>
    /// <exception cref="ArgumentNullException">If p1 or p2 are null.</exception>
    /// <exception cref="ArgumentException">If p1 and p2 are not married.</exception>
    public static Person Procreate(Person p1, Person p2)
    {
        ArgumentNullException.ThrowIfNull(p1);
        ArgumentNullException.ThrowIfNull(p2);
        if (!p1.Spouses.Contains(p2) && !p2.Spouses.Contains(p1))
        {
            throw new ArgumentException(string.Format("{0} must be married to {1} to procreate with them.",
                arg0: p1.Name,
                arg1: p2.Name));
        }
        Person baby = new()
        {
            Name = $"Baby of {p1.Name} and {p2.Name}",
            Born = DateTimeOffset.Now
        };
        p1.Children.Add(baby);
        p2.Children.Add(baby);
        return baby;
    }
    // Instance method to "multiply".
    public Person ProcreateWith(Person partner)
    {
        return Procreate(this, partner);
    }
    public static void Marry(Person person1, Person person2)
    {
        ArgumentNullException.ThrowIfNull(person1);
        ArgumentNullException.ThrowIfNull(person2);

        if (person1.Spouses.Contains(person2) || person2.Spouses.Contains(person1))
        {
            throw new ArgumentException($"{person1} is already married to {person2}");
        }

        person1.Spouses.Add(person2);
        person2.Spouses.Add(person1);
    }

    public void Marry(Person partner)
    {
        Marry(this, partner);
    }

    public void OutputSpouses()
    {
        if (Married)
        {
            var term = Spouses.Count == 1 ? "person" : "people";

            WriteLine($"{Name} is married to {Spouses.Count} {term}:");

            foreach (Person spouse in Spouses)
            {
                WriteLine($"     {spouse.Name}");
            }
        }
        else
        {
            WriteLine($"{Name} is single");
        }
    }

    #endregion

    #region Operators
    // Define the + operator to "marry".
    public static bool operator +(Person p1, Person p2)
    {
        Marry(p1, p2);
        // Confirm they are both now married.
        return p1.Married && p2.Married;
    }

    // Define the * operator to "multiply".
    public static Person operator *(Person p1, Person p2)
    {
        // Return a reference to the baby that results from multiplying.
        return Procreate(p1, p2);
    }
    #endregion


    #region Events
    // Delegate field to define the event.
    //public EventHandler? Shout; // null initially. original example
    public event EventHandler? Shout; // null initially. `event` enforces the subscribe syntax

    // Data field related to the event.
    public int AngerLevel;

    // Method to trigger the event in certain conditions.
    public void Poke()
    {
        AngerLevel++;
        if (AngerLevel < 3) return;
        // If something is listening to the event...
        if (Shout is not null)
        {
            // ...then call the delegate to "raise" the event.
            Shout(this, EventArgs.Empty);
        }
    }


    #endregion


    public int CompareTo(Person? other)
    {
        int position;

        if (other is not null)
        {
            if ((Name is not null) && (other.Name is not null))
            {
                // use the string implementation of CompareTo.
                position = Name.CompareTo(other.Name);
            }
            else if ((Name is not null) && (other.Name is null))
            {
                position = -1; // this Person precedes other Person.
            }
            else if ((Name is null) && (other.Name is not null))
            {
                position = 1; // this Person follows other Person.
            }
            else
            {
                position = 0; // this and other are at same position.
            }
        }
        else if (other is null)
        {
            position = -1; // this Person precedes other Person.
        }
        else
        {
            position = 0; // this and other are at same position.
        }
        return position;
    }
}
