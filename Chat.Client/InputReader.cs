using Akka.Actor;

namespace Chat.Client;

public static class InputReader
{
    public static void RunInputLoop(IActorRef client)
    {
        while (true)
        {
            var input = Console.ReadLine();
            var result = ParseInput(input);
            if (result == null)
            {
                break;
            }

            client.Tell(result);
        }
    }

    private static ChatInput? ParseInput(string? input)
    {
        if (input == null || input == "/exit")
        {
            return null;
        }

        if (!input.StartsWith("/"))
        {
            // Assume just one room for now.
            return new(ChatCommandType.SendMessage, "test", input);
        }

        var separatorIndex = input.IndexOf(' ');
        var command = input.Substring(0, separatorIndex);
        var payload = input.Substring(separatorIndex + 1);
        var commandType = ParseCommandType(command);

        return new(commandType, payload);
    }

    private static ChatCommandType ParseCommandType(string command)
    {
        return command switch
        {
            "/create" => ChatCommandType.CreateRoom,
            "/join" => ChatCommandType.JoinRoom,
            "/leave" => ChatCommandType.LeaveRoom,
            _ => throw new ArgumentOutOfRangeException($"Unsupported command {command}"),
        };
    }
}