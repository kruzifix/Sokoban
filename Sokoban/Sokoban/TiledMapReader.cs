using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SokobanGame;
using SokobanGame.Logic;
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

            // --- Property ---
            int propertyCount = input.ReadInt32();
            Dictionary<string, string> properties = new Dictionary<string, string>();
            for (int i = 0; i < propertyCount; i++)
            {
                properties.Add(input.ReadString(), input.ReadString());
            }

            // --- Tileset ---
            string tsName = input.ReadString();
            int tsTileWidth = input.ReadInt32();
            int tsTileHeight = input.ReadInt32();
            int tsTileCount = input.ReadInt32();
            int tsColumns = input.ReadInt32();
            string tsPath = input.ReadString();

            Texture2D tileSetTexture = input.ContentManager.Load<Texture2D>(tsPath);
            var tileset = new TiledTileset(tsName, tsTileWidth, tsTileHeight, tsTileCount, tsColumns, tileSetTexture);

            // --- Initial Room State ---
            IntVec playerPos = new IntVec(input.ReadInt32(), input.ReadInt32());

            // --- Entities ---
            List<Entity> ents = new List<Entity>();
            int entCount = input.ReadInt32();
            for (int i = 0; i < entCount; i++)
            {
                string type = input.ReadString();

                switch (type)
                {
                    case "box":
                        {
                            IntVec pos = new IntVec(input.ReadInt32(), input.ReadInt32());
                            ents.Add(new Box() { Pos = pos });
                            break;
                        }
                    case "sbox":
                        {
                            IntVec pos = new IntVec(input.ReadInt32(), input.ReadInt32());
                            ents.Add(new StickyBox() { Pos = pos });
                            break;
                        }
                }
            }

            // --- Room ---
            IntVec[] switches = new IntVec[input.ReadInt32()];
            for (int i = 0; i < switches.Length; i++)
                switches[i] = new IntVec(input.ReadInt32(), input.ReadInt32());

            int roomWidth = input.ReadInt32();
            int roomHeight = input.ReadInt32();
            
            Room room = new Room(roomWidth, roomHeight, switches, new RoomState(playerPos, ents));
            
            for (int j = 0; j < roomHeight; j++)
            {
                for (int i = 0; i < roomWidth; i++)
                {
                    room.SetWall(i, j, input.ReadInt32());
                }
            }
            
            var map = new TiledMap(width, height, tileWidth, tileHeight, tileset, room);

            foreach (KeyValuePair<string, string> kvp in properties)
                map.Properties.Add(kvp.Key, kvp.Value);

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
