namespace SokobanGame
{
    public enum Align
    {
        TopLeft,
        MidLeft,
        Center,
        MidRight,
        TopMid,
        BotMid
    }

    public enum FieldObject
    {
        Empty,
        Wall,
        IceGround
    }

    public enum InputState
    {
        Pressed,
        Released,
        Down,
        Up
    }

    public enum MovementDir
    {
        Up,
        Down,
        Left,
        Right
    }
}
