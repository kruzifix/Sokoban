// ----------------------------------------------------------------------------------------//
// Das Spiel "Sokoban" wurde im Rahmen des Bachelorstudiengangs "MultiMediaTechnology" der //
// Fachhochschule Salzburg von David Cukrowicz als MultiMediaProjekt 1 entwickelt.         //
//                                                                                         //
// Author: David Cukrowicz                                                                 //
//                                                                                         //
// Copyright (c) 2017 All Rights Reserved                                                  //
// ----------------------------------------------------------------------------------------//

using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content.Pipeline;

/*
 * Reference: http://dylanwilson.net/creating-custom-content-importers-for-the-monogame-pipeline
 */

namespace SokobanContentPipeline
{
    [ContentImporter(".tmx", DefaultProcessor = "TiledMapProcessor", DisplayName = "TiledMap Importer - Sokoban")]
    public class TiledMapImporter : ContentImporter<TiledMapFile>
    {
        public override TiledMapFile Import(string filename, ContentImporterContext context)
        {
            context.Logger.LogMessage("Importing XML file: {0}", filename);

            using (var streamReader = new StreamReader(filename))
            {
                var deserializer = new XmlSerializer(typeof(TiledMapFile));
                var file = (TiledMapFile)deserializer.Deserialize(streamReader);

                foreach (var p in file.Properties)
                {
                    if (p.Value == null)
                        p.Value = "";
                }

                return file;
            }
        }
    }
}
