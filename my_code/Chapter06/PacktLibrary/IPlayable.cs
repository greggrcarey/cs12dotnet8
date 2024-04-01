namespace Packt.Shared;

public interface IPlayable
{
    void Play();
    void Pause();

    void Stop() // default implementations available after C# 8
    {
        WriteLine("Default implemetation of Stop");
    }
}