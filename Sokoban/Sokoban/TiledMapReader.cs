using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SokobanGame.Tiled;

namespace Sokoban
{
    public class TiledMapReader : ContentTypeReader<TiledMap>
    {
        protected override TiledMap Read(ContentReader input, TiledMap existingInstance)
        {
            int width = input.ReadInt32();
            int height = input.ReadInt32();
            int tileWidth = input.ReadInt32();
            int tileHeight = input.ReadInt32();

            // --- Tileset ---
            string tsName = input.ReadString();
            int tsTileWidth = input.ReadInt32();
            int tsTileHeight = input.ReadInt32();
            int tsTileCount = input.ReadInt32();
            int tsColumns = input.ReadInt32();
            string tsPath = input.ReadString();

            Texture2D tileSetTexture = input.ContentManager.Load<Texture2D>(tsPath);
            var tileset = new TiledTileset(tsName, tsTileWidth, tsTileHeight, tsTileCount, tsColumns, tileSetTexture);
            
            var map = new TiledMap(width, height, tileWidth, tileHeight, tileset);

            // --- Layers ---
            int layerCount = input.ReadInt32();

            for (int i = 0; i < layerCount; i++)
            {
                string name = input.ReadString();
                int layerWidth = input.ReadInt32();
                int layerHeight = input.ReadInt32();
                var layer = new TiledLayer(map, name, layerWidth, layerHeight);

                for (int j = 0; j < layerWidth * layerHeight; j++)
                {
                    layer.SetData(j, input.ReadInt32());
                }
                map.AddLayer(layer);
            }

            return map;
        }
    }
}
