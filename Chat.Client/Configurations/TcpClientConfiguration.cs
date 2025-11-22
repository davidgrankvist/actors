using System.Net;
using Akka.Actor;
using Akka.Configuration;

namespace Chat.Client.Configurations;

public class TcpClientConfiguration(IPAddress address, int port) : IClientConfiguration
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
      port = 0   # 0 = random free port
    }}
  }}
  loglevel = INFO
}}
");
        return config;
    }

    public Task<IActorRef> CreateServerRefAsync(ActorSystem actorSystem)
    {
        var serverPath = $"akka.tcp://ChatServerSystem@{address}:{port}/user/echoServer";
        var selection = actorSystem.ActorSelection(serverPath);
        return selection.ResolveOne(TimeSpan.FromSeconds(3));
    }
}