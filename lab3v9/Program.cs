using System;

public class WebSocketClient : IDisposable
{
    private bool _disposed = false;
    private string _serverUrl;
    private bool _isConnected;

    public string ServerUrl
    {
        get { return _serverUrl; }
    }

    public bool IsConnected
    {
        get { return _isConnected; }
    }

    public WebSocketClient(string serverUrl)
    {
        _serverUrl = serverUrl;
        _isConnected = true;
        Console.WriteLine($"WebSocket підключено до {_serverUrl}");
    }

    public void SendMessage(string message)
    {
        if (_isConnected)
            Console.WriteLine($"Надсилання повідомлення: \"{message}\" на {_serverUrl}");
        else
            Console.WriteLine("Неможливо надіслати повідомлення: WebSocket закрито.");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Звільнення керованих ресурсів
                Console.WriteLine("Звільнення керованих ресурсів WebSocketClient");
            }

            // Звільнення некерованого ресурсу
            if (_isConnected)
            {
                Console.WriteLine($"Закриття WebSocket-з'єднання з {_serverUrl}");
                _isConnected = false;
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~WebSocketClient()
    {
        Dispose(false);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("--- Сценарій 1: using ---");
        using (var client1 = new WebSocketClient("wss://example.com/socket1"))
        {
            client1.SendMessage("Привіт із using!");
        } // Dispose() викликається автоматично тут

        Console.WriteLine("\n--- Сценарій 2: явний Dispose() ---");
        var client2 = new WebSocketClient("wss://example.com/socket2");
        client2.SendMessage("Привіт із явним Dispose!");
        client2.Dispose();

        Console.WriteLine("\n--- Сценарій 3: без Dispose(), через GC ---");
        CreateWithoutDispose();
        Console.WriteLine("Об'єкт вийшов з області видимості, викликаємо GC...");
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine("\nКінець програми.");
    }

    static void CreateWithoutDispose()
    {
        var client3 = new WebSocketClient("wss://example.com/socket3");
        client3.SendMessage("Привіт без Dispose!");
        // Dispose() не викликається навмисно — об'єкт буде знищено через фіналізатор
    }
}