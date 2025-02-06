namespace Server.Interfaces;

public interface ILogService
{
    Task Log(string message, string route, string type);
}
