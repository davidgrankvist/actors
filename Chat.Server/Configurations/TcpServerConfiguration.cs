using System.Net;
using Akka.Configuration;

namespace Chat.Server.Configurations;

public class TcpServerConfiguration(IPAddress address, int port) : IServerConfiguration
{
    private readonly IPAddress address = address;
    private readonly int port = port;

    public Config GetConfig()
    {
        var config = ConfigurationFactory.ParseString(@$"
akka {{
  actor {{
    provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
  }}
  remote {{
    dot-netty.tcp {{
      hostname = ""{address}""
      port = {port}
    }}
  }}
  loglevel = INFO
}}
");
        return config;
    }

    public string GetLocationDescriptor()
    {
        return $"{address}:{port}";
    }
}