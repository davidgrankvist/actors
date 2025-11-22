using Akka.Actor;
using Akka.Event;
using Chat.Lib;

namespace Chat.Server;

public record UserJoin(IActorRef User);
public record UserLeave(IActorRef User);
public record RoomBroadcast(string Message, IActorRef User);

public class ChatRoom : ReceiveActor
{
    private readonly ILoggingAdapter log = Context.GetLogger();
    private readonly HashSet<IActorRef> users = [];

    public string Name { get; }

    public ChatRoom(string name)
    {
        Name = name;

        Receive<UserJoin>(msg =>
        {
            log.Info($"User joined room {Name}: {msg.User.Path}");
            users.Add(msg.User);
        });

        Receive<UserLeave>(msg =>
        {
            log.Info($"User left room {Name}: {msg.User.Path}");
            users.Remove(msg.User);
        });

        Receive<RoomBroadcast>(msg =>
        {
            if (!users.Contains(msg.User))
            {
                log.Info($"Sender not in room. Ignoring broadcast.");
                return;
            }
            log.Info($"Broad cast in room {Name}: {msg.Message}");

            // Could filter out sender, but echo for now.
            foreach (var user in users)
            {
                user.Tell(new ChatMessage(msg.Message, Name));
            }
        });
    }
}