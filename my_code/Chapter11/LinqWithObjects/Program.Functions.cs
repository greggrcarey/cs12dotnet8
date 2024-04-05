partial class Program
{
    //private static void DeferredExecution(string[] names)
    //{
    //    SectionTitle("Deferred execution");

    //    //Which names ends with "m"?
    //    //Extension method
    //    var query1 = names.Where(name => name.EndsWith("m"));

    //    //LINQ query syntax
    //    var query2 = from name in names where name.EndsWith("m") select name;

    //    //Array of strings with Pam and Jim
    //    string[] result1 = query1.ToArray();

    //    //List of strings with Pam and Jim
    //    List<string> result2 = query2.ToList();

    //    foreach (string name in query1) 
    //    {
    //        WriteLine(name);//Outputs Pam
    //        names[2] = "Jimmy";
    //        //On the Pam iteration
    //        //the array updates Jim to Jimmy 
    //        //so the final letter is no longer m
    //        //and Jimmy does not get output.
    //    }
    //}

    private static void FilteringUsingWhereAndFunc(string[] names)
    {
        SectionTitle("Filtering entities using Where");
        //Explicity creating the delegate is not required since the compiler can do it for us.
        //var query = names.Where(new Func<string, bool>(NameLongerThanFour));

        //Here is using a named function
        //var query = names.Where(NameLongerThanFour);

        //Final step using anonymous function
        IOrderedEnumerable<string> query = names
            .Where(name => name.Length > 4)
            .OrderBy(name => name.Length)//Order by length
            .ThenBy(name => name);//then by alphabet 

        static bool NameLongerThanFour(string name)
        {
            return name.Length > 4;
        }

        foreach (string name in query)
        {
            WriteLine(name);
        }
    }

    private static void FilteringByType()
    {
        SectionTitle("Filtering By Type");

        List<Exception> exceptions = new()
        {
            new ArgumentException(), 
            new SystemException(),
            new IndexOutOfRangeException(), 
            new InvalidOperationException(),
            new NullReferenceException(), 
            new InvalidCastException(),
            new OverflowException(), 
            new DivideByZeroException(),
            new ApplicationException()
        };

        IEnumerable<ArithmeticException> arithmeticExceptionsQuery = exceptions.OfType<ArithmeticException>();

        foreach (ArithmeticException ex in arithmeticExceptionsQuery)
        {
            WriteLine(ex);
        }


    }








}