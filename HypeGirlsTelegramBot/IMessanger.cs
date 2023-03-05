using System;

public interface IMessanger
{
    public event Action<string> Send;
    void Recieve(string message);
}
