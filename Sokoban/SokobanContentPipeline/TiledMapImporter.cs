using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content.Pipeline;

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
                TiledMapFile map = (TiledMapFile)deserializer.Deserialize(streamReader);
                context.Logger.LogMessage("Parsed width = {0}", map.Width);
                return map;
            }
        }
    }
}
