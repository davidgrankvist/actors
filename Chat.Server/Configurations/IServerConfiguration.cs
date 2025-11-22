using Akka.Configuration;

namespace Chat.Server.Configurations;

public interface IServerConfiguration
{
    Config GetConfig();

    string GetLocationDescriptor();
}