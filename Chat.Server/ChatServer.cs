using Akka.Actor;
using Akka.Event;
using Chat.Lib;

namespace Chat.Server;

public class ChatServer : ReceiveActor
{
    private readonly ILoggingAdapter log = Context.GetLogger();
    private readonly Dictionary<string, IActorRef> rooms = [];

    public ChatServer()
    {
        Receive<CreateRoom>(msg =>
        {
            log.Info($"Create room {msg.Room}");
            if (rooms.ContainsKey(msg.Room))
            {
                log.Info($"Room {msg.Room} already exists");
                return;
            }
            var room = Context.ActorOf(Props.Create(() => new ChatRoom(msg.Room)));
            rooms[msg.Room] = room;
        });

        Receive<JoinRoom>(msg =>
        {
            log.Info($"Join room {msg.Room}");
            if (!rooms.TryGetValue(msg.Room, out var room))
            {
                log.Info($"Could not find room {msg.Room}");
                return;
            }
            room.Tell(new UserJoin(Sender));
        });

        Receive<LeaveRoom>(msg =>
        {
            log.Info($"Leave room {msg.Room}");
            if (!rooms.TryGetValue(msg.Room, out var room))
            {
                log.Info($"Could not find room {msg.Room}");
                return;
            }
            room.Tell(new UserLeave(Sender));
        });

        Receive<ChatMessage>(msg =>
        {
            log.Info($"Message room {msg.Room}");
            if (!rooms.TryGetValue(msg.Room, out var room))
            {
                log.Info($"Could not find room {msg.Room}");
                return;
            }
            room.Tell(new RoomBroadcast(msg.Message, Sender));
        });
    }
}
