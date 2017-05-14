// ----------------------------------------------------------------------------------------//
// Das Spiel "Sokoban" wurde im Rahmen des Bachelorstudiengangs "MultiMediaTechnology" der //
// Fachhochschule Salzburg von David Cukrowicz als MultiMediaProjekt 1 entwickelt.         //
//                                                                                         //
// Author: David Cukrowicz                                                                 //
//                                                                                         //
// Copyright (c) 2017 All Rights Reserved                                                  //
// ----------------------------------------------------------------------------------------//

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
