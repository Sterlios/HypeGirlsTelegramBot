
public class HTTPController
{
    private IMessanger _messanger;

    public HTTPController(IMessanger messanger)
    {
        _messanger = messanger;
        _messanger.Send += OnSend;
    }

    ~HTTPController()
    {
        _messanger.Send -= OnSend;
    }

    private void OnSend(string message)
    {

    }

    public void Recieve(string message)
    {
        _messanger.Recieve(message);
    }
}

