using Akka.Actor;
using Akka.Event;
using Chat.Lib;

namespace Chat.Client;

public class ChatClient : ReceiveActor
{
    private readonly ILoggingAdapter log = Context.GetLogger();

    public ChatClient(IActorRef server)
    {
        Receive<ChatInput>(input =>
        {
            log.Info($"Received chat input: {Enum.GetName(input.Command)} - {input.Room}");

            object? msg = ToChatCommand(input);
            if (msg == null)
            {
                log.Info("Unsupported chat command. Skipping.");
                return;
            }

            server.Tell(msg);
        });

        Receive<ChatMessage>(msg =>
        {
            log.Info($"Received message in room {msg.Room}: {msg.Message}");
        });
    }

    private static object? ToChatCommand(ChatInput input)
    {
        switch (input.Command)
        {
            case ChatCommandType.CreateRoom:
                return new CreateRoom(input.Room);
            case ChatCommandType.JoinRoom:
                return new JoinRoom(input.Room);
            case ChatCommandType.LeaveRoom:
                return new LeaveRoom(input.Room);
            case ChatCommandType.SendMessage:
                if (!string.IsNullOrEmpty(input.Message))
                {
                    return new ChatMessage(input.Message, input.Room);
                }
                break;
        }
        return null;
    }
}