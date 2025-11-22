using Akka.Actor;
using Akka.Event;
using Chat.Lib;

namespace Chat.Client;

public class ChatClient : ReceiveActor
{
    private readonly ILoggingAdapter log = Context.GetLogger();

    public ChatClient(IActorRef server)
    {
        Receive<ChatInput>(msg =>
        {
            log.Info($"Received input: {msg.Text}");
            server.Tell(new ChatMessage(msg.Text));
        });

        Receive<ChatMessage>(msg =>
        {
            log.Info($"Received message: {msg.Text}");
        });
    }
}