// ----------------------------------------------------------------------------------------//
// Das Spiel "Sokoban" wurde im Rahmen des Bachelorstudiengangs "MultiMediaTechnology" der //
// Fachhochschule Salzburg von David Cukrowicz als MultiMediaProjekt 1 entwickelt.         //
//                                                                                         //
// Author: David Cukrowicz                                                                 //
//                                                                                         //
// Copyright (c) 2017 All Rights Reserved                                                  //
// ----------------------------------------------------------------------------------------//

using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using SokobanGame;

namespace SokobanContentPipeline
{
    static class Extensions
    {
        public static void Write(this ContentWriter output, IntVec v)
        {
            output.Write(v.X);
            output.Write(v.Y);
        }
    }
}
