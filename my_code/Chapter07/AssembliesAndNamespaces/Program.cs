using System.Xml.Linq;

XDocument doc = new();

string s1 = "Hello";
String s2 = "World";
WriteLine($"{s1} {s2}");


WriteLine($"Environment.Is64BitProcess = {Environment.Is64BitProcess}");
WriteLine($"int.MaxValue = {int.MaxValue:N0}");
WriteLine($"nint.MaxValue = {nint.MaxValue:N0}");