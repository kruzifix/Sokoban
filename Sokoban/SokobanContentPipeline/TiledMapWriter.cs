// ----------------------------------------------------------------------------------------//
// Das Spiel "Sokoban" wurde im Rahmen des Bachelorstudiengangs "MultiMediaTechnology" der //
// Fachhochschule Salzburg von David Cukrowicz als MultiMediaProjekt 1 entwickelt.         //
//                                                                                         //
// Author: David Cukrowicz                                                                 //
//                                                                                         //
// Copyright (c) 2017 All Rights Reserved                                                  //
// ----------------------------------------------------------------------------------------//

using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using SokobanContentPipeline.Output;
using SokobanGame.Tiled;

/*
 * Reference: http://dylanwilson.net/creating-custom-content-importers-for-the-monogame-pipeline
 */

namespace SokobanContentPipeline
{
    [ContentTypeWriter]
    public class TiledMapWriter : ContentTypeWriter<TiledMapProcessorOutput>
    {
        protected override void Write(ContentWriter output, TiledMapProcessorOutput value)
        {
            value.WriteToOutput(output);
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(TiledMap).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Sokoban.TiledMapReader, Sokoban, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null";
        }
    }
}
