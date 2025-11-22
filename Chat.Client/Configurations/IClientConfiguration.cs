using Akka.Actor;
using Akka.Configuration;

namespace Chat.Client.Configurations;

public interface IClientConfiguration
{
    Config GetConfig();

    Task<IActorRef> CreateServerRefAsync(ActorSystem actorSystem);
}