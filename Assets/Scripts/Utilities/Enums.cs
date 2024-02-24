using System;
using System.Linq;


public enum ShapeSize : byte
{
    Domino = 2,
    Tromino = 3,
    Tetromino = 4
}

public enum ShapeType : sbyte
{
    None = -1,
    I = 0,
    L = 1,
    J = 2,
    O = 3,
    T = 4,
    S = 5,
    Z = 6
}

public enum MixerGroup : byte
{
    Music,
    SFX,
}

public enum Music : byte
{
    Menu,
    Game,
}

public enum Device : byte
{
    MouseKeyboard,
    Gamepad
}

public enum AvatarSize : byte
{
    Small,
    Medium,
    Large
}

public enum MessageType : byte
{
    Normal,
    Warning,
    Error,
    FatalError
}

public enum GameModeStart : byte
{
    GameNew,
    GameContinue
}

public static class ExtensionsEnum
{
    public static int ToInt<T>(this T self) where T : Enum => Convert.ToInt32(self);
    public static T ToEnum<T>(this int self) where T : Enum => (T)Enum.ToObject(typeof(T), self);

    //public static T ToEnum<T>(this string self) where T : Enum => (T)Enum.Parse(typeof(T), self, true);
    //public static int ToEnumInt<T>(this string self) where T : Enum => self.ToEnum<T>().ToInt<T>();
}

public class Enum<T> where T : Enum
{
    //public static int Count => Enum.GetNames(typeof(T)).Length;
    public static T[] GetValues() => Enum.GetValues(typeof(T)).Cast<T>().ToArray<T>();
}
