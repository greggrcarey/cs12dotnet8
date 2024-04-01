namespace Packt.Shared;

public class PersonComparer : IComparer<Person?>
{
    public int Compare(Person? x, Person? y)
    {
        int position;

        if ((x is not null) && (y is not null))
        {
            if ((x.Name is not null) && (y.Name is not null))
            {
                int result = x.Name.Length.CompareTo(y.Name.Length);

                if (result == 0)
                {
                    // ...then compare by the Names...
                    return x.Name.CompareTo(y.Name);
                }
                else
                {
                    // ...otherwise compare by the lengths.
                    position = result;
                }
            }
            else if ((x.Name is not null) && (y.Name is null))
            {
                position = -1; // x Person precedes y Person.
            }
            else if ((x.Name is null) && (y.Name is not null))
            {
                position = 1; // x Person follows y Person.
            }
            else // x.Name and y.Name are both null.
            {
                position = 0; // x and y are at same position.
            }
        }
        else if ((x is not null) && (y is null))
        {
            position = -1; // x Person precedes y Person.
        }
        else if ((x is null) && (y is not null))
        {
            position = 1; // x Person follows y Person.
        }
        else // x and y are both null.
        {
            position = 0; // x and y are at same position.
        }
        return position;
    }
}
