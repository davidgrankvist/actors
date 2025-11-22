namespace Chat.Lib;

public record CreateRoom(string Room);
public record JoinRoom(string Room);
public record LeaveRoom(string Room);
public record ChatMessage(string Message, string Room);
