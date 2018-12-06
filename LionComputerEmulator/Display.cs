using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

/*
 * sprites:
 * 11 sprites
 * 63152 F6B0: parameters
 * SX,  SY,  SDX, SDY, SCOLOR,   SENABLE
 * word word byte byte byte3lsb  byte
 * +128 bank 2 63280 F730
 * 
 * sprite data: 63552 F840 
 * 16words (16x16)
 * bank 2: 64064 FA40
 * 
 */

namespace LionComputerEmulator
{
    public static class Display
    {
        private static Bitmap screenBitMap = new Bitmap(Width << 1, Height << 1, PixelFormat.Format8bppIndexed);
        private static ColorPalette screenPalette = screenBitMap.Palette;
        private static byte[] screenBytes = new byte[Width * 2 * Height * 2]; // 8 bpp space x2 size
        private static byte[] spriteColorBytes = new byte[32 << 5]; // 8 bpp space x2 size
        private static byte[] spriteMaskBytes = new byte[32 << 5]; // 8 bpp space x2 size
        public static BackgroundWorker SpritePortWorker = new BackgroundWorker();
        private static byte[] scrCols = new byte[129];
        // sprite colors and mask values
        private static byte[] sprMaskVals = new byte[] { 0x0ff, 0 };
        private static byte[] sprColVals = new byte[2];
        // sprite to screen line blocks
        private static byte[] sprPixelBytes = new byte[16];
        private static byte[] sprMaskBytes = new byte[32];
        private static byte[] sprColorBytes = new byte[32];
        private static ulong[] screenBitBlock = new ulong[4];
        private static ulong[] spriteBitBlock = new ulong[4];
        private static ulong[] maskBitBlock = new ulong[4];
        private static ulong[] rasterOpBitBlock = new ulong[4];
        // updated from sprite port worker
        private static int spriteBank = 0;
        private static uint vblCounter = 0;
        private static int __vblCounterHigh = 0;
        private static int __vblCounterLow = 0;

        /// <summary>
        /// Sprite Parameters Bank 1
        /// </summary>
        public const int SPRITE_PARAMS_1 = 0x0f6b0;

        /// <summary>
        /// Sprite Parameters Bank 2
        /// </summary>
        public const int SPRITE_PARAMS_2 = SPRITE_PARAMS_1 + 128;

        /// <summary>
        /// Sprite Data Bank 1 at address 63552
        /// </summary>
        public const int SPRITE_DATA_1 = 0x0f840;

        /// <summary>
        /// Sprite Data Bank 2 at address 64064
        /// </summary>
        public const int SPRITE_DATA_2 = SPRITE_DATA_1 + 512;

        // offsets in sprite parameters
        public const int SPRITE_X = 0;       // word
        public const int SPRITE_Y = 2;       // word
        public const int SPRITE_DX = 4;      // byte
        public const int SPRITE_DY = 5;      // byte
        public const int SPRITE_COLOR = 6;   // byte
        public const int SPRITE_ENABLE = 7;  // byte

        private const int SPRITES_NUM = 11;

        /// <summary>
        /// Start of VIDEO RAM Address
        /// </summary>
        public const int VIDEO_RAM_START = 0x0c000;

        /// <summary>
        /// End of VIDEO RAM Address
        /// </summary>
        public const int VIDEO_RAM_END = VIDEO_RAM_START + (Width * Height) / 8;

        /// <summary>
        /// Start of Video Color Lookup Table constant
        /// </summary>
        public const int VIDEO_CLUT_START = VIDEO_RAM_START + 0x02ee0;

        /// <summary>
        /// End of Video Color Lookup Table constant
        /// </summary>
        public const int VIDEO_CLUT_END = VIDEO_CLUT_START + 0x07c0;

        public const int Width = 384;

        public const int Height = 248;

        public const int CharWidth = 6;

        public const int CharHeight = 8;

        public const int CharXDimension = Width / CharWidth;

        public const int CharYDimension = Height / CharHeight;

        /// <summary>
        /// Memory Data Byte Array
        /// </summary>
        public static byte[] Ram = new byte[0x010000];

        public static void InitScreen()
        {
            // poke the palette
            screenPalette.Entries[0] = Color.FromArgb(0, 0, 0);
            screenPalette.Entries[1] = Color.FromArgb(0, 0, 255);
            screenPalette.Entries[2] = Color.FromArgb(0, 255, 0);
            screenPalette.Entries[3] = Color.FromArgb(0, 255, 255);
            screenPalette.Entries[4] = Color.FromArgb(255, 0, 0);
            screenPalette.Entries[5] = Color.FromArgb(255, 0, 255);
            screenPalette.Entries[6] = Color.FromArgb(255, 255, 0);
            screenPalette.Entries[7] = Color.FromArgb(255, 255, 255);
            screenBitMap.Palette = screenPalette;
            SpritePortWorker.DoWork += SpritePortWork;
            __vblCounterHigh = Cpu.COUNTER;
            __vblCounterLow = Cpu.COUNTER + 1;
        }

        // argument passed from the device port access
        private static void SpritePortWork(object sender, DoWorkEventArgs e)
        {
            lock (copylock)
                spriteBank = Convert.ToInt32(e.Argument);
        }

        public static Bitmap Screen()
        {
            int tileOffsetVram = VIDEO_RAM_START;
            int columnOffsetBmp = 0;
            int tileColorIndex = 0;

            byte vRamTileValue1 = 0;
            byte vRamTileValue2 = 0;
            byte vRamTileValue3 = 0;
            byte vRamTileValue4 = 0;
            byte vRamTileValue5 = 0;
            byte vRamTileValue6 = 0;

            byte colfg = 0;

            vblCounter++;
            lock (copylock)
            {
                Memory.Data[Cpu.COUNTER] = (byte)(vblCounter >> 8);
                Memory.Data[Cpu.COUNTER + 1] = (byte)(vblCounter);
            }

            while (tileColorIndex < 1984)
            {
                colfg = (byte)(Ram[VIDEO_CLUT_START + tileColorIndex] >> 3);
                scrCols[0] = (byte)(Ram[VIDEO_CLUT_START + tileColorIndex] & 0x07);
                scrCols[1] = colfg;
                scrCols[2] = colfg;
                scrCols[4] = colfg;
                scrCols[8] = colfg;
                scrCols[0x010] = colfg;
                scrCols[0x020] = colfg;
                scrCols[0x040] = colfg;
                scrCols[0x080] = colfg;

                vRamTileValue1 = Ram[tileOffsetVram];
                vRamTileValue2 = Ram[tileOffsetVram + 1];
                vRamTileValue3 = Ram[tileOffsetVram + 2];
                vRamTileValue4 = Ram[tileOffsetVram + 3];
                vRamTileValue5 = Ram[tileOffsetVram + 4];
                vRamTileValue6 = Ram[tileOffsetVram + 5];

                screenBytes[columnOffsetBmp] = scrCols[vRamTileValue1 & 0x080];
                screenBytes[columnOffsetBmp + 1] = scrCols[vRamTileValue1 & 0x080];
                screenBytes[columnOffsetBmp + 2] = scrCols[vRamTileValue2 & 0x080];
                screenBytes[columnOffsetBmp + 3] = scrCols[vRamTileValue2 & 0x080];
                screenBytes[columnOffsetBmp + 4] = scrCols[vRamTileValue3 & 0x080];
                screenBytes[columnOffsetBmp + 5] = scrCols[vRamTileValue3 & 0x080];
                screenBytes[columnOffsetBmp + 6] = scrCols[vRamTileValue4 & 0x080];
                screenBytes[columnOffsetBmp + 7] = scrCols[vRamTileValue4 & 0x080];
                screenBytes[columnOffsetBmp + 8] = scrCols[vRamTileValue5 & 0x080];
                screenBytes[columnOffsetBmp + 9] = scrCols[vRamTileValue5 & 0x080];
                screenBytes[columnOffsetBmp + 10] = scrCols[vRamTileValue6 & 0x080];
                screenBytes[columnOffsetBmp + 11] = scrCols[vRamTileValue6 & 0x080];

                screenBytes[columnOffsetBmp + 768] = scrCols[vRamTileValue1 & 0x080];
                screenBytes[columnOffsetBmp + 769] = scrCols[vRamTileValue1 & 0x080];
                screenBytes[columnOffsetBmp + 770] = scrCols[vRamTileValue2 & 0x080];
                screenBytes[columnOffsetBmp + 771] = scrCols[vRamTileValue2 & 0x080];
                screenBytes[columnOffsetBmp + 772] = scrCols[vRamTileValue3 & 0x080];
                screenBytes[columnOffsetBmp + 773] = scrCols[vRamTileValue3 & 0x080];
                screenBytes[columnOffsetBmp + 774] = scrCols[vRamTileValue4 & 0x080];
                screenBytes[columnOffsetBmp + 775] = scrCols[vRamTileValue4 & 0x080];
                screenBytes[columnOffsetBmp + 776] = scrCols[vRamTileValue5 & 0x080];
                screenBytes[columnOffsetBmp + 777] = scrCols[vRamTileValue5 & 0x080];
                screenBytes[columnOffsetBmp + 778] = scrCols[vRamTileValue6 & 0x080];
                screenBytes[columnOffsetBmp + 779] = scrCols[vRamTileValue6 & 0x080];

                screenBytes[columnOffsetBmp + 1536] = scrCols[vRamTileValue1 & 0x040];
                screenBytes[columnOffsetBmp + 1537] = scrCols[vRamTileValue1 & 0x040];
                screenBytes[columnOffsetBmp + 1538] = scrCols[vRamTileValue2 & 0x040];
                screenBytes[columnOffsetBmp + 1539] = scrCols[vRamTileValue2 & 0x040];
                screenBytes[columnOffsetBmp + 1540] = scrCols[vRamTileValue3 & 0x040];
                screenBytes[columnOffsetBmp + 1541] = scrCols[vRamTileValue3 & 0x040];
                screenBytes[columnOffsetBmp + 1542] = scrCols[vRamTileValue4 & 0x040];
                screenBytes[columnOffsetBmp + 1543] = scrCols[vRamTileValue4 & 0x040];
                screenBytes[columnOffsetBmp + 1544] = scrCols[vRamTileValue5 & 0x040];
                screenBytes[columnOffsetBmp + 1545] = scrCols[vRamTileValue5 & 0x040];
                screenBytes[columnOffsetBmp + 1546] = scrCols[vRamTileValue6 & 0x040];
                screenBytes[columnOffsetBmp + 1547] = scrCols[vRamTileValue6 & 0x040];

                screenBytes[columnOffsetBmp + 2304] = scrCols[vRamTileValue1 & 0x040];
                screenBytes[columnOffsetBmp + 2305] = scrCols[vRamTileValue1 & 0x040];
                screenBytes[columnOffsetBmp + 2306] = scrCols[vRamTileValue2 & 0x040];
                screenBytes[columnOffsetBmp + 2307] = scrCols[vRamTileValue2 & 0x040];
                screenBytes[columnOffsetBmp + 2308] = scrCols[vRamTileValue3 & 0x040];
                screenBytes[columnOffsetBmp + 2309] = scrCols[vRamTileValue3 & 0x040];
                screenBytes[columnOffsetBmp + 2310] = scrCols[vRamTileValue4 & 0x040];
                screenBytes[columnOffsetBmp + 2311] = scrCols[vRamTileValue4 & 0x040];
                screenBytes[columnOffsetBmp + 2312] = scrCols[vRamTileValue5 & 0x040];
                screenBytes[columnOffsetBmp + 2313] = scrCols[vRamTileValue5 & 0x040];
                screenBytes[columnOffsetBmp + 2314] = scrCols[vRamTileValue6 & 0x040];
                screenBytes[columnOffsetBmp + 2315] = scrCols[vRamTileValue6 & 0x040];

                screenBytes[columnOffsetBmp + 3072] = scrCols[vRamTileValue1 & 0x020];
                screenBytes[columnOffsetBmp + 3073] = scrCols[vRamTileValue1 & 0x020];
                screenBytes[columnOffsetBmp + 3074] = scrCols[vRamTileValue2 & 0x020];
                screenBytes[columnOffsetBmp + 3075] = scrCols[vRamTileValue2 & 0x020];
                screenBytes[columnOffsetBmp + 3076] = scrCols[vRamTileValue3 & 0x020];
                screenBytes[columnOffsetBmp + 3077] = scrCols[vRamTileValue3 & 0x020];
                screenBytes[columnOffsetBmp + 3078] = scrCols[vRamTileValue4 & 0x020];
                screenBytes[columnOffsetBmp + 3079] = scrCols[vRamTileValue4 & 0x020];
                screenBytes[columnOffsetBmp + 3080] = scrCols[vRamTileValue5 & 0x020];
                screenBytes[columnOffsetBmp + 3081] = scrCols[vRamTileValue5 & 0x020];
                screenBytes[columnOffsetBmp + 3082] = scrCols[vRamTileValue6 & 0x020];
                screenBytes[columnOffsetBmp + 3083] = scrCols[vRamTileValue6 & 0x020];

                screenBytes[columnOffsetBmp + 3840] = scrCols[vRamTileValue1 & 0x020];
                screenBytes[columnOffsetBmp + 3841] = scrCols[vRamTileValue1 & 0x020];
                screenBytes[columnOffsetBmp + 3842] = scrCols[vRamTileValue2 & 0x020];
                screenBytes[columnOffsetBmp + 3843] = scrCols[vRamTileValue2 & 0x020];
                screenBytes[columnOffsetBmp + 3844] = scrCols[vRamTileValue3 & 0x020];
                screenBytes[columnOffsetBmp + 3845] = scrCols[vRamTileValue3 & 0x020];
                screenBytes[columnOffsetBmp + 3846] = scrCols[vRamTileValue4 & 0x020];
                screenBytes[columnOffsetBmp + 3847] = scrCols[vRamTileValue4 & 0x020];
                screenBytes[columnOffsetBmp + 3848] = scrCols[vRamTileValue5 & 0x020];
                screenBytes[columnOffsetBmp + 3849] = scrCols[vRamTileValue5 & 0x020];
                screenBytes[columnOffsetBmp + 3850] = scrCols[vRamTileValue6 & 0x020];
                screenBytes[columnOffsetBmp + 3851] = scrCols[vRamTileValue6 & 0x020];

                screenBytes[columnOffsetBmp + 4608] = scrCols[vRamTileValue1 & 0x010];
                screenBytes[columnOffsetBmp + 4609] = scrCols[vRamTileValue1 & 0x010];
                screenBytes[columnOffsetBmp + 4610] = scrCols[vRamTileValue2 & 0x010];
                screenBytes[columnOffsetBmp + 4611] = scrCols[vRamTileValue2 & 0x010];
                screenBytes[columnOffsetBmp + 4612] = scrCols[vRamTileValue3 & 0x010];
                screenBytes[columnOffsetBmp + 4613] = scrCols[vRamTileValue3 & 0x010];
                screenBytes[columnOffsetBmp + 4614] = scrCols[vRamTileValue4 & 0x010];
                screenBytes[columnOffsetBmp + 4615] = scrCols[vRamTileValue4 & 0x010];
                screenBytes[columnOffsetBmp + 4616] = scrCols[vRamTileValue5 & 0x010];
                screenBytes[columnOffsetBmp + 4617] = scrCols[vRamTileValue5 & 0x010];
                screenBytes[columnOffsetBmp + 4618] = scrCols[vRamTileValue6 & 0x010];
                screenBytes[columnOffsetBmp + 4619] = scrCols[vRamTileValue6 & 0x010];

                screenBytes[columnOffsetBmp + 5376] = scrCols[vRamTileValue1 & 0x010];
                screenBytes[columnOffsetBmp + 5377] = scrCols[vRamTileValue1 & 0x010];
                screenBytes[columnOffsetBmp + 5378] = scrCols[vRamTileValue2 & 0x010];
                screenBytes[columnOffsetBmp + 5379] = scrCols[vRamTileValue2 & 0x010];
                screenBytes[columnOffsetBmp + 5380] = scrCols[vRamTileValue3 & 0x010];
                screenBytes[columnOffsetBmp + 5381] = scrCols[vRamTileValue3 & 0x010];
                screenBytes[columnOffsetBmp + 5382] = scrCols[vRamTileValue4 & 0x010];
                screenBytes[columnOffsetBmp + 5383] = scrCols[vRamTileValue4 & 0x010];
                screenBytes[columnOffsetBmp + 5384] = scrCols[vRamTileValue5 & 0x010];
                screenBytes[columnOffsetBmp + 5385] = scrCols[vRamTileValue5 & 0x010];
                screenBytes[columnOffsetBmp + 5386] = scrCols[vRamTileValue6 & 0x010];
                screenBytes[columnOffsetBmp + 5387] = scrCols[vRamTileValue6 & 0x010];

                screenBytes[columnOffsetBmp + 6144] = scrCols[vRamTileValue1 & 0x08];
                screenBytes[columnOffsetBmp + 6145] = scrCols[vRamTileValue1 & 0x08];
                screenBytes[columnOffsetBmp + 6146] = scrCols[vRamTileValue2 & 0x08];
                screenBytes[columnOffsetBmp + 6147] = scrCols[vRamTileValue2 & 0x08];
                screenBytes[columnOffsetBmp + 6148] = scrCols[vRamTileValue3 & 0x08];
                screenBytes[columnOffsetBmp + 6149] = scrCols[vRamTileValue3 & 0x08];
                screenBytes[columnOffsetBmp + 6150] = scrCols[vRamTileValue4 & 0x08];
                screenBytes[columnOffsetBmp + 6151] = scrCols[vRamTileValue4 & 0x08];
                screenBytes[columnOffsetBmp + 6152] = scrCols[vRamTileValue5 & 0x08];
                screenBytes[columnOffsetBmp + 6153] = scrCols[vRamTileValue5 & 0x08];
                screenBytes[columnOffsetBmp + 6154] = scrCols[vRamTileValue6 & 0x08];
                screenBytes[columnOffsetBmp + 6155] = scrCols[vRamTileValue6 & 0x08];

                screenBytes[columnOffsetBmp + 6912] = scrCols[vRamTileValue1 & 0x08];
                screenBytes[columnOffsetBmp + 6913] = scrCols[vRamTileValue1 & 0x08];
                screenBytes[columnOffsetBmp + 6914] = scrCols[vRamTileValue2 & 0x08];
                screenBytes[columnOffsetBmp + 6915] = scrCols[vRamTileValue2 & 0x08];
                screenBytes[columnOffsetBmp + 6916] = scrCols[vRamTileValue3 & 0x08];
                screenBytes[columnOffsetBmp + 6917] = scrCols[vRamTileValue3 & 0x08];
                screenBytes[columnOffsetBmp + 6918] = scrCols[vRamTileValue4 & 0x08];
                screenBytes[columnOffsetBmp + 6919] = scrCols[vRamTileValue4 & 0x08];
                screenBytes[columnOffsetBmp + 6920] = scrCols[vRamTileValue5 & 0x08];
                screenBytes[columnOffsetBmp + 6921] = scrCols[vRamTileValue5 & 0x08];
                screenBytes[columnOffsetBmp + 6922] = scrCols[vRamTileValue6 & 0x08];
                screenBytes[columnOffsetBmp + 6923] = scrCols[vRamTileValue6 & 0x08];

                screenBytes[columnOffsetBmp + 7680] = scrCols[vRamTileValue1 & 0x04];
                screenBytes[columnOffsetBmp + 7681] = scrCols[vRamTileValue1 & 0x04];
                screenBytes[columnOffsetBmp + 7682] = scrCols[vRamTileValue2 & 0x04];
                screenBytes[columnOffsetBmp + 7683] = scrCols[vRamTileValue2 & 0x04];
                screenBytes[columnOffsetBmp + 7684] = scrCols[vRamTileValue3 & 0x04];
                screenBytes[columnOffsetBmp + 7685] = scrCols[vRamTileValue3 & 0x04];
                screenBytes[columnOffsetBmp + 7686] = scrCols[vRamTileValue4 & 0x04];
                screenBytes[columnOffsetBmp + 7687] = scrCols[vRamTileValue4 & 0x04];
                screenBytes[columnOffsetBmp + 7688] = scrCols[vRamTileValue5 & 0x04];
                screenBytes[columnOffsetBmp + 7689] = scrCols[vRamTileValue5 & 0x04];
                screenBytes[columnOffsetBmp + 7690] = scrCols[vRamTileValue6 & 0x04];
                screenBytes[columnOffsetBmp + 7691] = scrCols[vRamTileValue6 & 0x04];

                screenBytes[columnOffsetBmp + 8448] = scrCols[vRamTileValue1 & 0x04];
                screenBytes[columnOffsetBmp + 8449] = scrCols[vRamTileValue1 & 0x04];
                screenBytes[columnOffsetBmp + 8450] = scrCols[vRamTileValue2 & 0x04];
                screenBytes[columnOffsetBmp + 8451] = scrCols[vRamTileValue2 & 0x04];
                screenBytes[columnOffsetBmp + 8452] = scrCols[vRamTileValue3 & 0x04];
                screenBytes[columnOffsetBmp + 8453] = scrCols[vRamTileValue3 & 0x04];
                screenBytes[columnOffsetBmp + 8454] = scrCols[vRamTileValue4 & 0x04];
                screenBytes[columnOffsetBmp + 8455] = scrCols[vRamTileValue4 & 0x04];
                screenBytes[columnOffsetBmp + 8456] = scrCols[vRamTileValue5 & 0x04];
                screenBytes[columnOffsetBmp + 8457] = scrCols[vRamTileValue5 & 0x04];
                screenBytes[columnOffsetBmp + 8458] = scrCols[vRamTileValue6 & 0x04];
                screenBytes[columnOffsetBmp + 8459] = scrCols[vRamTileValue6 & 0x04];

                screenBytes[columnOffsetBmp + 9216] = scrCols[vRamTileValue1 & 0x02];
                screenBytes[columnOffsetBmp + 9217] = scrCols[vRamTileValue1 & 0x02];
                screenBytes[columnOffsetBmp + 9218] = scrCols[vRamTileValue2 & 0x02];
                screenBytes[columnOffsetBmp + 9219] = scrCols[vRamTileValue2 & 0x02];
                screenBytes[columnOffsetBmp + 9220] = scrCols[vRamTileValue3 & 0x02];
                screenBytes[columnOffsetBmp + 9221] = scrCols[vRamTileValue3 & 0x02];
                screenBytes[columnOffsetBmp + 9222] = scrCols[vRamTileValue4 & 0x02];
                screenBytes[columnOffsetBmp + 9223] = scrCols[vRamTileValue4 & 0x02];
                screenBytes[columnOffsetBmp + 9224] = scrCols[vRamTileValue5 & 0x02];
                screenBytes[columnOffsetBmp + 9225] = scrCols[vRamTileValue5 & 0x02];
                screenBytes[columnOffsetBmp + 9226] = scrCols[vRamTileValue6 & 0x02];
                screenBytes[columnOffsetBmp + 9227] = scrCols[vRamTileValue6 & 0x02];

                screenBytes[columnOffsetBmp + 9984] = scrCols[vRamTileValue1 & 0x02];
                screenBytes[columnOffsetBmp + 9985] = scrCols[vRamTileValue1 & 0x02];
                screenBytes[columnOffsetBmp + 9986] = scrCols[vRamTileValue2 & 0x02];
                screenBytes[columnOffsetBmp + 9987] = scrCols[vRamTileValue2 & 0x02];
                screenBytes[columnOffsetBmp + 9988] = scrCols[vRamTileValue3 & 0x02];
                screenBytes[columnOffsetBmp + 9989] = scrCols[vRamTileValue3 & 0x02];
                screenBytes[columnOffsetBmp + 9990] = scrCols[vRamTileValue4 & 0x02];
                screenBytes[columnOffsetBmp + 9991] = scrCols[vRamTileValue4 & 0x02];
                screenBytes[columnOffsetBmp + 9992] = scrCols[vRamTileValue5 & 0x02];
                screenBytes[columnOffsetBmp + 9993] = scrCols[vRamTileValue5 & 0x02];
                screenBytes[columnOffsetBmp + 9994] = scrCols[vRamTileValue6 & 0x02];
                screenBytes[columnOffsetBmp + 9995] = scrCols[vRamTileValue6 & 0x02];

                screenBytes[columnOffsetBmp + 10752] = scrCols[vRamTileValue1 & 0x01];
                screenBytes[columnOffsetBmp + 10753] = scrCols[vRamTileValue1 & 0x01];
                screenBytes[columnOffsetBmp + 10754] = scrCols[vRamTileValue2 & 0x01];
                screenBytes[columnOffsetBmp + 10755] = scrCols[vRamTileValue2 & 0x01];
                screenBytes[columnOffsetBmp + 10756] = scrCols[vRamTileValue3 & 0x01];
                screenBytes[columnOffsetBmp + 10757] = scrCols[vRamTileValue3 & 0x01];
                screenBytes[columnOffsetBmp + 10758] = scrCols[vRamTileValue4 & 0x01];
                screenBytes[columnOffsetBmp + 10759] = scrCols[vRamTileValue4 & 0x01];
                screenBytes[columnOffsetBmp + 10760] = scrCols[vRamTileValue5 & 0x01];
                screenBytes[columnOffsetBmp + 10761] = scrCols[vRamTileValue5 & 0x01];
                screenBytes[columnOffsetBmp + 10762] = scrCols[vRamTileValue6 & 0x01];
                screenBytes[columnOffsetBmp + 10763] = scrCols[vRamTileValue6 & 0x01];

                screenBytes[columnOffsetBmp + 11520] = scrCols[vRamTileValue1 & 0x01];
                screenBytes[columnOffsetBmp + 11521] = scrCols[vRamTileValue1 & 0x01];
                screenBytes[columnOffsetBmp + 11522] = scrCols[vRamTileValue2 & 0x01];
                screenBytes[columnOffsetBmp + 11523] = scrCols[vRamTileValue2 & 0x01];
                screenBytes[columnOffsetBmp + 11524] = scrCols[vRamTileValue3 & 0x01];
                screenBytes[columnOffsetBmp + 11525] = scrCols[vRamTileValue3 & 0x01];
                screenBytes[columnOffsetBmp + 11526] = scrCols[vRamTileValue4 & 0x01];
                screenBytes[columnOffsetBmp + 11527] = scrCols[vRamTileValue4 & 0x01];
                screenBytes[columnOffsetBmp + 11528] = scrCols[vRamTileValue5 & 0x01];
                screenBytes[columnOffsetBmp + 11529] = scrCols[vRamTileValue5 & 0x01];
                screenBytes[columnOffsetBmp + 11530] = scrCols[vRamTileValue6 & 0x01];
                screenBytes[columnOffsetBmp + 11531] = scrCols[vRamTileValue6 & 0x01];

                tileColorIndex++;
                tileOffsetVram += CharWidth;
                columnOffsetBmp += 12;
                if ((tileColorIndex & 0x03f) == 0)
                {
                    columnOffsetBmp = (tileColorIndex >> 6) * 12288;
                }
            }

            lock (copylock)
            {
                // sprites
                for (int sprnum = 0; sprnum < SPRITES_NUM; sprnum++)
                {
                    int sparams = ((spriteBank & 1) == 1 ? SPRITE_PARAMS_2 : SPRITE_PARAMS_1) + (sprnum << 3);
                    int sdata = ((spriteBank & 2) == 2 ? SPRITE_DATA_2 : SPRITE_DATA_1) + (sprnum << 5);
                    if (Ram[sparams + SPRITE_ENABLE] == 1)
                        BlitSprite(ref sparams, ref sdata);
                }
                BitmapData bmpdata = screenBitMap.LockBits(new Rectangle(0, 0, Width << 1, Height << 1), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                Marshal.Copy(screenBytes, 0, bmpdata.Scan0, screenBytes.Length);
                screenBitMap.UnlockBits(bmpdata);
                return screenBitMap;
            }
        }

        private static void BlitSprite(ref int spriteParams, ref int spriteData)
        {
            sprColVals[1] = Ram[spriteParams + SPRITE_COLOR]; // [0] always black
            int x = (Ram[spriteParams++] << 8 | Ram[spriteParams++]) << 1;
            int y = (Ram[spriteParams++] << 8 | Ram[spriteParams]) << 1;
            int srcScreenIndex = y * 768 + x;
            int sprdata = 0, sprPixlndx = 0;
            for (int sprndx = 0; sprndx < 16; sprndx++)
            {
                if (srcScreenIndex >= screenBytes.Length - 64)
                    break;
                sprdata = Ram[spriteData++] << 8 | Ram[spriteData++];
                sprPixlndx = 1;
                sprPixelBytes[0] = (byte)(sprdata & 1);
                sprPixelBytes[1] = (byte)((sprdata >> sprPixlndx++) & 1);
                sprPixelBytes[2] = (byte)((sprdata >> sprPixlndx++) & 1);
                sprPixelBytes[3] = (byte)((sprdata >> sprPixlndx++) & 1);
                sprPixelBytes[4] = (byte)((sprdata >> sprPixlndx++) & 1);
                sprPixelBytes[5] = (byte)((sprdata >> sprPixlndx++) & 1);
                sprPixelBytes[6] = (byte)((sprdata >> sprPixlndx++) & 1);
                sprPixelBytes[7] = (byte)((sprdata >> sprPixlndx++) & 1);
                sprPixelBytes[8] = (byte)((sprdata >> sprPixlndx++) & 1);
                sprPixelBytes[9] = (byte)((sprdata >> sprPixlndx++) & 1);
                sprPixelBytes[10] = (byte)((sprdata >> sprPixlndx++) & 1);
                sprPixelBytes[11] = (byte)((sprdata >> sprPixlndx++) & 1);
                sprPixelBytes[12] = (byte)((sprdata >> sprPixlndx++) & 1);
                sprPixelBytes[13] = (byte)((sprdata >> sprPixlndx++) & 1);
                sprPixelBytes[14] = (byte)((sprdata >> sprPixlndx++) & 1);
                sprPixelBytes[15] = (byte)((sprdata >> sprPixlndx) & 1);
                sprMaskBytes[00] = sprMaskVals[sprPixelBytes[0]];
                sprMaskBytes[01] = sprMaskVals[sprPixelBytes[0]];
                sprMaskBytes[02] = sprMaskVals[sprPixelBytes[1]];
                sprMaskBytes[03] = sprMaskVals[sprPixelBytes[1]];
                sprMaskBytes[04] = sprMaskVals[sprPixelBytes[2]];
                sprMaskBytes[05] = sprMaskVals[sprPixelBytes[2]];
                sprMaskBytes[06] = sprMaskVals[sprPixelBytes[3]];
                sprMaskBytes[07] = sprMaskVals[sprPixelBytes[3]];
                sprMaskBytes[08] = sprMaskVals[sprPixelBytes[4]];
                sprMaskBytes[09] = sprMaskVals[sprPixelBytes[4]];
                sprMaskBytes[10] = sprMaskVals[sprPixelBytes[5]];
                sprMaskBytes[11] = sprMaskVals[sprPixelBytes[5]];
                sprMaskBytes[12] = sprMaskVals[sprPixelBytes[6]];
                sprMaskBytes[13] = sprMaskVals[sprPixelBytes[6]];
                sprMaskBytes[14] = sprMaskVals[sprPixelBytes[7]];
                sprMaskBytes[15] = sprMaskVals[sprPixelBytes[7]];
                sprMaskBytes[16] = sprMaskVals[sprPixelBytes[8]];
                sprMaskBytes[17] = sprMaskVals[sprPixelBytes[8]];
                sprMaskBytes[18] = sprMaskVals[sprPixelBytes[9]];
                sprMaskBytes[19] = sprMaskVals[sprPixelBytes[9]];
                sprMaskBytes[20] = sprMaskVals[sprPixelBytes[10]];
                sprMaskBytes[21] = sprMaskVals[sprPixelBytes[10]];
                sprMaskBytes[22] = sprMaskVals[sprPixelBytes[11]];
                sprMaskBytes[23] = sprMaskVals[sprPixelBytes[11]];
                sprMaskBytes[24] = sprMaskVals[sprPixelBytes[12]];
                sprMaskBytes[25] = sprMaskVals[sprPixelBytes[12]];
                sprMaskBytes[26] = sprMaskVals[sprPixelBytes[13]];
                sprMaskBytes[27] = sprMaskVals[sprPixelBytes[13]];
                sprMaskBytes[28] = sprMaskVals[sprPixelBytes[14]];
                sprMaskBytes[29] = sprMaskVals[sprPixelBytes[14]];
                sprMaskBytes[30] = sprMaskVals[sprPixelBytes[15]];
                sprMaskBytes[31] = sprMaskVals[sprPixelBytes[15]];
                // color
                sprColorBytes[00] = sprColVals[sprPixelBytes[0]];
                sprColorBytes[01] = sprColVals[sprPixelBytes[0]];
                sprColorBytes[02] = sprColVals[sprPixelBytes[1]];
                sprColorBytes[03] = sprColVals[sprPixelBytes[1]];
                sprColorBytes[04] = sprColVals[sprPixelBytes[2]];
                sprColorBytes[05] = sprColVals[sprPixelBytes[2]];
                sprColorBytes[06] = sprColVals[sprPixelBytes[3]];
                sprColorBytes[07] = sprColVals[sprPixelBytes[3]];
                sprColorBytes[08] = sprColVals[sprPixelBytes[4]];
                sprColorBytes[09] = sprColVals[sprPixelBytes[4]];
                sprColorBytes[10] = sprColVals[sprPixelBytes[5]];
                sprColorBytes[11] = sprColVals[sprPixelBytes[5]];
                sprColorBytes[12] = sprColVals[sprPixelBytes[6]];
                sprColorBytes[13] = sprColVals[sprPixelBytes[6]];
                sprColorBytes[14] = sprColVals[sprPixelBytes[7]];
                sprColorBytes[15] = sprColVals[sprPixelBytes[7]];
                sprColorBytes[16] = sprColVals[sprPixelBytes[8]];
                sprColorBytes[17] = sprColVals[sprPixelBytes[8]];
                sprColorBytes[18] = sprColVals[sprPixelBytes[9]];
                sprColorBytes[19] = sprColVals[sprPixelBytes[9]];
                sprColorBytes[20] = sprColVals[sprPixelBytes[10]];
                sprColorBytes[21] = sprColVals[sprPixelBytes[10]];
                sprColorBytes[22] = sprColVals[sprPixelBytes[11]];
                sprColorBytes[23] = sprColVals[sprPixelBytes[11]];
                sprColorBytes[24] = sprColVals[sprPixelBytes[12]];
                sprColorBytes[25] = sprColVals[sprPixelBytes[12]];
                sprColorBytes[26] = sprColVals[sprPixelBytes[13]];
                sprColorBytes[27] = sprColVals[sprPixelBytes[13]];
                sprColorBytes[28] = sprColVals[sprPixelBytes[14]];
                sprColorBytes[29] = sprColVals[sprPixelBytes[14]];
                sprColorBytes[30] = sprColVals[sprPixelBytes[15]];
                sprColorBytes[31] = sprColVals[sprPixelBytes[15]];
                Buffer.BlockCopy(sprMaskBytes, 0, maskBitBlock, 0, 32);
                Buffer.BlockCopy(sprColorBytes, 0, spriteBitBlock, 0, 32);
                Buffer.BlockCopy(screenBytes, srcScreenIndex, screenBitBlock, 0, 32);
                rasterOpBitBlock[0] = screenBitBlock[0] & maskBitBlock[0] | spriteBitBlock[0];
                rasterOpBitBlock[1] = screenBitBlock[1] & maskBitBlock[1] | spriteBitBlock[1];
                rasterOpBitBlock[2] = screenBitBlock[2] & maskBitBlock[2] | spriteBitBlock[2];
                rasterOpBitBlock[3] = screenBitBlock[3] & maskBitBlock[3] | spriteBitBlock[3];
                Buffer.BlockCopy(rasterOpBitBlock, 0, screenBytes, srcScreenIndex, 32);
                srcScreenIndex += 768;
                if (srcScreenIndex >= screenBytes.Length - 32)
                    break;
                Buffer.BlockCopy(rasterOpBitBlock, 0, screenBytes, srcScreenIndex, 32);
                srcScreenIndex += 768;
            }
        }
        public static object copylock = new object();
    }
}
