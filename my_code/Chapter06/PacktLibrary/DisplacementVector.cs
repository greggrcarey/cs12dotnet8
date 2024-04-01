namespace Packt.Shared;

public record struct DisplacementVector(int initialX, int initialY)
{
    public int X { get; set; } = initialX;
    public int Y { get; set; } = initialY;

    public static DisplacementVector operator +(DisplacementVector vector1, DisplacementVector vector2)
    {
        return new DisplacementVector(vector1.X + vector2.X, vector1.Y + vector2.Y);
    }
}