using Akka.Actor;
using Akka.Event;
using Chat.Lib;

namespace Chat.Server;

public class EchoServer : ReceiveActor
{
    private readonly ILoggingAdapter log = Context.GetLogger();

    public EchoServer()
    {
        Receive<ChatMessage>(msg =>
        {
           log.Info($"Received {msg.Text}");
           Sender.Tell(new ChatMessage($"Echo: {msg.Text}"));
        });
    }
}