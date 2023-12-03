var publisher = new EventPublisher();


while (true)
{
    using (var subscriber = new EventSubscriber(publisher))
    {
        // do something with the publisher and subscriber objects  
    }
}

class EventPublisher
{
    public event EventHandler MyEvent;

    public void RaiseEvent()
    {
        MyEvent?.Invoke(this, EventArgs.Empty);
    }
}

class EventSubscriber : IDisposable
{
    private readonly EventPublisher _publisher;

    public EventSubscriber(EventPublisher publisher)
    {
        _publisher = publisher;
        _publisher.MyEvent += OnMyEvent;
    }

    private void OnMyEvent(object sender, EventArgs e)
    {
        Console.WriteLine("MyEvent raised");
    }

    public void Dispose()
    {
        // Unsubscribe from the event when the subscriber is disposed
        _publisher.MyEvent -= OnMyEvent;
    }
}