namespace zzz;

public interface FileReceivedEvent
{
    public int Id { get; }
}

public interface FileReceived : FileReceivedEvent
{

}

public interface CustomerDataReceived : FileReceivedEvent
{

}
