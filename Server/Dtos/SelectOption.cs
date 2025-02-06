namespace Server.Dtos;

public record SelectOption<T>(T Key, string Text) where T : struct;
