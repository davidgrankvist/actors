namespace Chat.Client;

public enum ChatCommandType
{
    CreateRoom,
    JoinRoom,
    LeaveRoom,
    SendMessage,
}

public record ChatInput(ChatCommandType Command, string Room, string? Message = null);
