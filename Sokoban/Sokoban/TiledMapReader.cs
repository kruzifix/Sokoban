using Microsoft.Xna.Framework.Content;
using SokobanGame;

namespace Sokoban
{
    public class TiledMapReader : ContentTypeReader<TiledMap>
    {
        protected override TiledMap Read(ContentReader input, TiledMap existingInstance)
        {
            int width = input.ReadInt32();

            return new TiledMap(width);
        }
    }
}
