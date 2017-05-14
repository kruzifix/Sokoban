// ----------------------------------------------------------------------------------------//
// Das Spiel "Sokoban" wurde im Rahmen des Bachelorstudiengangs "MultiMediaTechnology" der //
// Fachhochschule Salzburg von David Cukrowicz als MultiMediaProjekt 1 entwickelt.         //
//                                                                                         //
// Author: David Cukrowicz                                                                 //
//                                                                                         //
// Copyright (c) 2017 All Rights Reserved                                                  //
// ----------------------------------------------------------------------------------------//

namespace SokobanGame.Logic
{
    public class Teleporter
    {
        public IntVec Pos { get; private set; }
        public IntVec Target { get; private set; }

        public Teleporter(IntVec pos, IntVec target)
        {
            Pos = pos;
            Target = target;
        }
    }
}