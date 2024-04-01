namespace Ch04Ex02PrimeFactorsLib
{
    public static class PrimeFactorsLibrary
    {
        public static string PrimeFactors(int number)
        {
            List<string> factors = new ();

            //for(int i = number; i > 1; i--)
            //{
            //    if (number % i == 0)
            //    {
            //        var result = number / i;
            //        factors.Add($"{result} x ");
            //    }
            //}

            int divisor = 2;
            

            if(number % divisor == 0)
            {
                
            }
            

            return string.Format($"Prime factors of {number} are {factors.ToArray()}");

        }

    }
}
