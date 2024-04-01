using Packt.Shared;
using System.Threading;

Person harry = new()
{
    Name = "Harry",
    Born = new(year: 2001, month: 3, day: 25, hour: 0, minute: 0, second: 0, offset: TimeSpan.Zero)

};

harry.WriteToConsole();


Person lamech = new() { Name = "Lamech" };
Person adah = new() { Name = "Adah" };
Person zillah = new() { Name = "Zillah" };

lamech.Marry(adah);

//Person.Marry(lamech, zillah);

if (lamech + zillah)
{
    WriteLine($"{lamech.Name} and {zillah.Name} successfully got married.");
}

lamech.OutputSpouses();
adah.OutputSpouses();
zillah.OutputSpouses();


Person baby1 = lamech.ProcreateWith(adah);
baby1.Name = "Jabal";
WriteLine($"{baby1.Name} was born on {baby1.Born}");

Person baby2 = Person.Procreate(zillah, lamech);
baby2.Name = "Tubalcain";

Person baby3 = lamech * adah;
baby3.Name = "Jubal";
Person baby4 = zillah * lamech;
baby4.Name = "Naamah";


adah.WriteChildernToConsole();
zillah.WriteChildernToConsole();
lamech.WriteChildernToConsole();

for (int i = 0; i < lamech.Children.Count; i++)
{
    WriteLine(format: " {0}'s child #{1} is named \"{2}\".",
    arg0: lamech.Name, arg1: i, arg2: lamech.Children[i].Name);
}

// Assign the method to the Shout delegate.
//harry.Shout = Harry_Shout; //without `event` declaration
harry.Shout += Harry_Shout;
harry.Shout += Harry_Shout_2;
// Call the Poke method that eventually raises the Shout event.
harry.Poke();
harry.Poke();
harry.Poke();
harry.Poke();


Person?[] people =
[
null,
new() { Name = "Simon" },
new() { Name = "Jenny" },
new() { Name = "Adam" },
new() { Name = null },
new() { Name = "Richard" }
];

OutputPeopleNames(people, "Initial list of people:");

Array.Sort(people);

OutputPeopleNames(people,"After sorting using Person's IComparable implementation:");

Array.Sort(people, new PersonComparer());

OutputPeopleNames(people,"After sorting using PersonComparer's IComparer implementation:");

DisplacementVector dv1 = new(3, 5);
DisplacementVector dv2 = new(-2, 7);
DisplacementVector dv3 = dv1 + dv2;
WriteLine($"({dv1.X}, {dv1.Y}) + ({dv2.X}, {dv2.Y}) = ({dv3.X},{ dv3.Y})");

//structs are initialized to default value since they live on the stack

DisplacementVector dv4 = new();
WriteLine($"({dv4.X}, {dv4.Y})");


DisplacementVector dv5 = new(3, 5);
WriteLine($"dv1.Equals(dv5): {dv1.Equals(dv5)})");
WriteLine($"dv1 == dv5: {dv1 == dv5})");
//Cannot compare structs with == operator, but .Equals() is good
//Can compare record structs with == operator

C1 c1 = new() { Name = "Bob" };
c1.Name = "Bill";
C2 c2 = new(Name: "Bob");
//c2.Name = "Bill"; // CS8852: Init-only property.
S1 s1 = new() { Name = "Bob" };
s1.Name = "Bill";
S2 s2 = new(Name: "Bob");
s2.Name = "Bill";
S3 s3 = new(Name: "Bob");
//s3.Name = "Bill"; // CS8852: Init-only property.