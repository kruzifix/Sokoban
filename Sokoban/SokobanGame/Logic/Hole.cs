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
    public class Hole : Entity
    {
        public bool Filled { get; set; } = false;

        public override Entity Copy()
        {
            return new Hole() { Pos = Pos, Filled = Filled };
        }
    }
}
