using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace SokobanContentPipeline
{
    [ContentImporter(".tmx", DefaultProcessor = "TiledMapProcessor", DisplayName = "TiledMap Importer")]
    public class TiledMapImporter : ContentImporter<TiledMap>
    {
        public override TiledMap Import(string filename, ContentImporterContext context)
        {
            context.Logger.LogMessage("Importing XML file: {0}", filename);

            using (var streamReader = new StreamReader(filename))
            {
                var deserializer = new XmlSerializer(typeof(TiledMap));
                return (TiledMap)deserializer.Deserialize(streamReader);
            }
        }
    }
}
