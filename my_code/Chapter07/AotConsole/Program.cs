using System.Reflection;
using System.Reflection.Emit;
using System.Globalization; // To use CultureInfo.

WriteLine("This is an ahead-of-time (AOT) compiled console app.");
WriteLine("Current culture: {0}", CultureInfo.CurrentCulture.DisplayName);
WriteLine("OS version: {0}", Environment.OSVersion);
Write("Press any key to exit.");

ReadKey(intercept: true); // Do not output the key that was pressed.

//AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("MyAssembly"), AssemblyBuilderAccess.Run);