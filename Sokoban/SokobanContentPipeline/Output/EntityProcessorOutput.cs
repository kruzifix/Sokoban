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

namespace SokobanContentPipeline.Output
{
    public class EntityProcessorOutput
    {
        public string Type { get; set; }
        public IntVec Pos { get; set; }

        public void WriteToOutput(ContentWriter output)
        {
            output.Write(Type);
            output.Write(Pos);
        }
    }
}
