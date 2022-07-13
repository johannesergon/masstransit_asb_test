namespace zzz;

public record FileReceivedEvent(int Id);
public record FileReceived(int Id) : FileReceivedEvent(Id);
public record CustomerDataReceived(int Id) : FileReceivedEvent(Id);
