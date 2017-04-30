using Microsoft.Xna.Framework;

namespace SokobanGame
{
    public class Colors
    {
        public static Color MenuScreenBackground { get { return Color.LightSlateGray; } }
        public static Color LevelSelectScreenBackground { get { return Color.LightSlateGray; } }
        public static Color GameScreenBackground { get { return Color.LightSlateGray; } }

        public static Color PadBackground { get { return new Color(51, 51, 51); } }
        public static Color PadBorder { get { return Color.LightGray; } }
        public static Color PadText { get { return Color.White; } }

        public static Color BoxBack { get { return new Color(51, 51, 51); } }
        public static Color BoxBackSel { get { return Color.DimGray; } }
        public static Color BoxBorder { get { return Color.DimGray; } }
        public static Color BoxBorderSel { get { return Color.LightGray; } }
        public static Color BoxTextBack { get { return Color.Gray; } }
        public static Color BoxTextBackSel { get { return Color.DarkOliveGreen; } }
        public static Color BoxText { get { return Color.White; } }
        public static Color BoxTextSel { get { return Color.GreenYellow; } }

        public static Color BtnBorder { get { return Color.DimGray; } }
        public static Color BtnBorderSel { get { return Color.LightGray; } }
        public static Color BtnBack { get { return Color.Gray; } }
        public static Color BtnBackSel { get { return Color.DarkOliveGreen; } }
        public static Color BtnText { get { return Color.White; } }
        public static Color BtnTextSel { get { return Color.GreenYellow; } }
    }
}
