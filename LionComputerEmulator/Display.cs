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
 * multi color sprites:
 * 11 sprites
 * 
 * sprite data: 16384 4000
 * 8bytes x 16 lines
 * 
 * 
 * 
 * 
 * 
 * 
 */

namespace LionComputerEmulator
{
    public static class Display
    {
        private static Bitmap screenBitMap = new Bitmap(Width << 1, Height0 << 1, PixelFormat.Format8bppIndexed);
        private static ColorPalette screenPalette = screenBitMap.Palette;
        private static byte[] screenBytes = new byte[(Width << 1) * (Height0 << 1)]; // 8 bpp space x2 size
        private static byte[] spriteColorBytes = new byte[32 << 5]; // 8 bpp space x2 size
        private static byte[] spriteMaskBytes = new byte[32 << 5];  // 8 bpp space x2 size
        private static byte[] borderZeroBmp = new byte[borderZeroBmpBytesNum];
        public static BackgroundWorker SpritePortWorker = new BackgroundWorker();
        public static BackgroundWorker VideoModePortWorker = new BackgroundWorker();
        private static byte[] scrCols0 = new byte[129];
        private static byte[] scrCols1 = new byte[4];
        // sprite colors and mask values
        private static byte[] sprMaskVals = new byte[] { 0x0ff, 0 };
        private static byte[] sprMaskValsMode1 = new byte[] { 0, 0x0ff };
        private static byte[] sprColVals = new byte[2];
        // sprite to screen line blocks
        private static byte[] sprPixelBytes = new byte[16];
        private static byte[] sprMaskBytes = new byte[32];
        private static byte[] sprColorBytes = new byte[32];
        private static ulong[] screenBitBlock = new ulong[4];
        private static ulong[] spriteBitBlock = new ulong[4];
        private static ulong[] maskBitBlock = new ulong[4];
        private static ulong[] rasterOpBitBlock = new ulong[4];

        private static int spriteBank = 0; // updated from sprite port worker
        private static int videoMode = 0;  // updated from videomode port worker

        private static uint vblCounter = 0;
        private static int __vblCounterHigh = 0;
        private static int __vblCounterLow = 0;

        private const int borderZeroBmpBytesNum = (Width << 1) * (Height0 - Height1);

        /// <summary>
        /// Mode 0 Sprite Parameters Bank 1
        /// </summary>
        public const int SPRITE_PARAMS_0_1 = 0x0f6b0;

        /// <summary>
        /// Mode 0 Sprite Parameters Bank 2
        /// </summary>
        public const int SPRITE_PARAMS_0_2 = SPRITE_PARAMS_0_1 + 128;

        /// <summary>
        /// Mode 0 Sprite Data Bank 1 at address 63552
        /// </summary>
        public const int SPRITE_DATA_0_1 = 0x0f840;

        /// <summary>
        /// Mode 0 Sprite Data Bank 2 at address 64064
        /// </summary>
        public const int SPRITE_DATA_0_2 = SPRITE_DATA_0_1 + 512;

        /// <summary>
        /// Mode 1 Sprite Parameters Bank 1
        /// </summary>
        public const int SPRITE_PARAMS_1_1 = 16384;

        /// <summary>
        /// Mode 1 Sprite Parameters Bank 2
        /// </summary>
        public const int SPRITE_PARAMS_1_2 = SPRITE_PARAMS_1_1 + 256;

        /// <summary>
        /// Mode 1 Sprite Data Bank 1
        /// </summary>
        public const int SPRITE_DATA_1_1 = SPRITE_PARAMS_1_2 + 256;

        /// <summary>
        /// Mode 1 Sprite Data Bank 2
        /// </summary>
        public const int SPRITE_DATA_1_2 = SPRITE_PARAMS_1_1 + 4352;

        // offsets in sprite parameters
        public const int SPRITE_X = 0;       // word
        public const int SPRITE_Y = 2;       // word
        public const int SPRITE_DX = 4;      // byte
        public const int SPRITE_DY = 5;      // byte
        public const int SPRITE_COLOR = 6;   // byte
        public const int SPRITE_ENABLE = 7;  // byte

        private const int SPRITES_NUM = 11;

        /// <summary>
        /// Start of VIDEO RAM Address Mode 0
        /// </summary>
        public const int VIDEO_RAM_START_MODE0 = 0x0c000;

        /// <summary>
        /// End of VIDEO RAM Address Mode 0
        /// </summary>
        public const int VIDEO_RAM_END_MODE0 = VIDEO_RAM_START_MODE0 + (Width * Height0) / 8;

        /// <summary>
        /// Start of VIDEO RAM Address Mode 1
        /// </summary>
        public const int VIDEO_RAM_START_MODE1 = 0x08000;

        /// <summary>
        /// End of VIDEO RAM Address Mode 1
        /// </summary>
        public const int VIDEO_RAM_END_MODE1 = VIDEO_RAM_START_MODE1 + 32000;

        /// <summary>
        /// Start of Video Color Lookup Table constant
        /// </summary>
        public const int VIDEO_CLUT_START = VIDEO_RAM_START_MODE0 + 0x02ee0;

        /// <summary>
        /// End of Video Color Lookup Table constant
        /// </summary>
        public const int VIDEO_CLUT_END = VIDEO_CLUT_START + 0x07c0;

        public const int Width = 320;

        public const int Height0 = 240; // mode0 pixels height

        public const int Height1 = 200; // mode1 pixels height

        public const int CharWidth = 6;

        public const int CharHeight = 8;

        public const int CharXDimension = Width / CharWidth;

        public const int CharYDimension = Height0 / CharHeight;

        /// <summary>
        /// Memory Data Byte Array
        /// </summary>
        public static byte[] Ram = new byte[0x010000];

        // argument passed from the device port access
        private static void SpritePortWork(object sender, DoWorkEventArgs e)
        {
            lock (copylock)
                spriteBank = Convert.ToInt32(e.Argument);
        }

        // argument passed from the videomode port access
        private static void VideoModePortWork(object sender, DoWorkEventArgs e)
        {
            lock (copylock)
            {
                videoMode = Convert.ToInt32(e.Argument);
                switch (videoMode)
                {
                    case 0:
                        // poke the palette for mode0
                        screenPalette.Entries[0] = Color.FromArgb(0, 0, 0);
                        screenPalette.Entries[1] = Color.FromArgb(0, 0, 255);
                        screenPalette.Entries[2] = Color.FromArgb(0, 255, 0);
                        screenPalette.Entries[3] = Color.FromArgb(0, 255, 255);
                        screenPalette.Entries[4] = Color.FromArgb(255, 0, 0);
                        screenPalette.Entries[5] = Color.FromArgb(255, 0, 255);
                        screenPalette.Entries[6] = Color.FromArgb(255, 255, 0);
                        screenPalette.Entries[7] = Color.FromArgb(255, 255, 255);
                        break;

                    case 1:
                        // poke the low palette (half intensity) for mode1
                        screenPalette.Entries[0] = Color.FromArgb(0, 0, 0);
                        screenPalette.Entries[1] = Color.FromArgb(0, 0, 200);
                        screenPalette.Entries[2] = Color.FromArgb(0, 200, 0);
                        screenPalette.Entries[3] = Color.FromArgb(0, 200, 200);
                        screenPalette.Entries[4] = Color.FromArgb(200, 0, 0);
                        screenPalette.Entries[5] = Color.FromArgb(200, 0, 200);
                        screenPalette.Entries[6] = Color.FromArgb(160, 160, 0);
                        screenPalette.Entries[7] = Color.FromArgb(200, 200, 200);
                        // clear rest mode0 lines of bitmap
                        Buffer.BlockCopy(borderZeroBmp, 0, screenBytes, 0, borderZeroBmpBytesNum);
                        Buffer.BlockCopy(borderZeroBmp, 0, screenBytes, (Width << 1) * ((Height1 << 1) + (Height0 - Height1)), borderZeroBmpBytesNum);
                        break;
                }
            }
        }

        public static void InitScreen()
        {
            // poke the palette for mode0 to start showing
            screenPalette.Entries[0] = Color.FromArgb(0, 0, 0);
            screenPalette.Entries[1] = Color.FromArgb(0, 0, 255);
            screenPalette.Entries[2] = Color.FromArgb(0, 255, 0);
            screenPalette.Entries[3] = Color.FromArgb(0, 255, 255);
            screenPalette.Entries[4] = Color.FromArgb(255, 0, 0);
            screenPalette.Entries[5] = Color.FromArgb(255, 0, 255);
            screenPalette.Entries[6] = Color.FromArgb(255, 255, 0);
            screenPalette.Entries[7] = Color.FromArgb(255, 255, 255);

            // poke the high palette for mode1
            screenPalette.Entries[8] = Color.FromArgb(0, 0, 0);
            screenPalette.Entries[9] = Color.FromArgb(0, 0, 255);
            screenPalette.Entries[10] = Color.FromArgb(0, 255, 0);
            screenPalette.Entries[11] = Color.FromArgb(0, 255, 255);
            screenPalette.Entries[12] = Color.FromArgb(255, 0, 0);
            screenPalette.Entries[13] = Color.FromArgb(255, 0, 255);
            screenPalette.Entries[14] = Color.FromArgb(255, 255, 0);
            screenPalette.Entries[15] = Color.FromArgb(255, 255, 255);

            SpritePortWorker.DoWork += SpritePortWork;
            VideoModePortWorker.DoWork += VideoModePortWork;
            __vblCounterHigh = Cpu.COUNTER;
            __vblCounterLow = Cpu.COUNTER + 1;
        }

        public static Bitmap Screen()
        {
            screenBitMap.Palette = screenPalette;
            switch (videoMode)
            {
                case 1:
                    int scrBytesNdx = borderZeroBmpBytesNum;
                    for (int _vrambytendx = VIDEO_RAM_START_MODE1; _vrambytendx < VIDEO_RAM_END_MODE1; _vrambytendx++)
                    {
                        byte dualPixelValue = Ram[_vrambytendx];
                        scrCols0[0] = (byte)(dualPixelValue >> 4);
                        scrCols0[2] = (byte)(dualPixelValue & 0x0f);
                        scrCols0[1] = scrCols0[0];
                        scrCols0[3] = scrCols0[2];
                        Buffer.BlockCopy(scrCols0, 0, screenBytes, scrBytesNdx, 4);
                        Buffer.BlockCopy(scrCols0, 0, screenBytes, scrBytesNdx + 640, 4);
                        scrBytesNdx += 4;
                        if (scrBytesNdx % 640 == 0)
                            scrBytesNdx += 640;
                    }
                    break;

                default:
                    int tileOffsetVram = VIDEO_RAM_START_MODE0;
                    int columnOffsetBmp = 0;
                    int tileScreenIndex = 0;
                    int tileColorIndex = 0;

                    byte vRamTileValue1 = 0;
                    byte vRamTileValue2 = 0;
                    byte vRamTileValue3 = 0;
                    byte vRamTileValue4 = 0;
                    byte vRamTileValue5 = 0;
                    byte vRamTileValue6 = 0;

                    byte colfg = 0;

                    while (tileScreenIndex < CharXDimension * CharYDimension)
                    {
                        colfg = (byte)(Ram[VIDEO_CLUT_START + tileColorIndex] >> 3);
                        scrCols0[0] = (byte)(Ram[VIDEO_CLUT_START + tileColorIndex] & 0x07);
                        scrCols0[1] = colfg;
                        scrCols0[2] = colfg;
                        scrCols0[4] = colfg;
                        scrCols0[8] = colfg;
                        scrCols0[0x010] = colfg;
                        scrCols0[0x020] = colfg;
                        scrCols0[0x040] = colfg;
                        scrCols0[0x080] = colfg;

                        vRamTileValue1 = Ram[tileOffsetVram];
                        vRamTileValue2 = Ram[tileOffsetVram + 1];
                        vRamTileValue3 = Ram[tileOffsetVram + 2];
                        vRamTileValue4 = Ram[tileOffsetVram + 3];
                        vRamTileValue5 = Ram[tileOffsetVram + 4];
                        vRamTileValue6 = Ram[tileOffsetVram + 5];

                        screenBytes[columnOffsetBmp] = scrCols0[vRamTileValue1 & 0x080];
                        screenBytes[columnOffsetBmp + 1] = scrCols0[vRamTileValue1 & 0x080];
                        screenBytes[columnOffsetBmp + 2] = scrCols0[vRamTileValue2 & 0x080];
                        screenBytes[columnOffsetBmp + 3] = scrCols0[vRamTileValue2 & 0x080];
                        screenBytes[columnOffsetBmp + 4] = scrCols0[vRamTileValue3 & 0x080];
                        screenBytes[columnOffsetBmp + 5] = scrCols0[vRamTileValue3 & 0x080];
                        screenBytes[columnOffsetBmp + 6] = scrCols0[vRamTileValue4 & 0x080];
                        screenBytes[columnOffsetBmp + 7] = scrCols0[vRamTileValue4 & 0x080];
                        screenBytes[columnOffsetBmp + 8] = scrCols0[vRamTileValue5 & 0x080];
                        screenBytes[columnOffsetBmp + 9] = scrCols0[vRamTileValue5 & 0x080];
                        screenBytes[columnOffsetBmp + 10] = scrCols0[vRamTileValue6 & 0x080];
                        screenBytes[columnOffsetBmp + 11] = scrCols0[vRamTileValue6 & 0x080];

                        screenBytes[columnOffsetBmp + 640] = scrCols0[vRamTileValue1 & 0x080];
                        screenBytes[columnOffsetBmp + 641] = scrCols0[vRamTileValue1 & 0x080];
                        screenBytes[columnOffsetBmp + 642] = scrCols0[vRamTileValue2 & 0x080];
                        screenBytes[columnOffsetBmp + 643] = scrCols0[vRamTileValue2 & 0x080];
                        screenBytes[columnOffsetBmp + 644] = scrCols0[vRamTileValue3 & 0x080];
                        screenBytes[columnOffsetBmp + 645] = scrCols0[vRamTileValue3 & 0x080];
                        screenBytes[columnOffsetBmp + 646] = scrCols0[vRamTileValue4 & 0x080];
                        screenBytes[columnOffsetBmp + 647] = scrCols0[vRamTileValue4 & 0x080];
                        screenBytes[columnOffsetBmp + 648] = scrCols0[vRamTileValue5 & 0x080];
                        screenBytes[columnOffsetBmp + 649] = scrCols0[vRamTileValue5 & 0x080];
                        screenBytes[columnOffsetBmp + 650] = scrCols0[vRamTileValue6 & 0x080];
                        screenBytes[columnOffsetBmp + 651] = scrCols0[vRamTileValue6 & 0x080];

                        screenBytes[columnOffsetBmp + 1280] = scrCols0[vRamTileValue1 & 0x040];
                        screenBytes[columnOffsetBmp + 1281] = scrCols0[vRamTileValue1 & 0x040];
                        screenBytes[columnOffsetBmp + 1282] = scrCols0[vRamTileValue2 & 0x040];
                        screenBytes[columnOffsetBmp + 1283] = scrCols0[vRamTileValue2 & 0x040];
                        screenBytes[columnOffsetBmp + 1284] = scrCols0[vRamTileValue3 & 0x040];
                        screenBytes[columnOffsetBmp + 1285] = scrCols0[vRamTileValue3 & 0x040];
                        screenBytes[columnOffsetBmp + 1286] = scrCols0[vRamTileValue4 & 0x040];
                        screenBytes[columnOffsetBmp + 1287] = scrCols0[vRamTileValue4 & 0x040];
                        screenBytes[columnOffsetBmp + 1288] = scrCols0[vRamTileValue5 & 0x040];
                        screenBytes[columnOffsetBmp + 1289] = scrCols0[vRamTileValue5 & 0x040];
                        screenBytes[columnOffsetBmp + 1290] = scrCols0[vRamTileValue6 & 0x040];
                        screenBytes[columnOffsetBmp + 1291] = scrCols0[vRamTileValue6 & 0x040];

                        screenBytes[columnOffsetBmp + 1920] = scrCols0[vRamTileValue1 & 0x040];
                        screenBytes[columnOffsetBmp + 1921] = scrCols0[vRamTileValue1 & 0x040];
                        screenBytes[columnOffsetBmp + 1922] = scrCols0[vRamTileValue2 & 0x040];
                        screenBytes[columnOffsetBmp + 1923] = scrCols0[vRamTileValue2 & 0x040];
                        screenBytes[columnOffsetBmp + 1924] = scrCols0[vRamTileValue3 & 0x040];
                        screenBytes[columnOffsetBmp + 1925] = scrCols0[vRamTileValue3 & 0x040];
                        screenBytes[columnOffsetBmp + 1926] = scrCols0[vRamTileValue4 & 0x040];
                        screenBytes[columnOffsetBmp + 1927] = scrCols0[vRamTileValue4 & 0x040];
                        screenBytes[columnOffsetBmp + 1928] = scrCols0[vRamTileValue5 & 0x040];
                        screenBytes[columnOffsetBmp + 1929] = scrCols0[vRamTileValue5 & 0x040];
                        screenBytes[columnOffsetBmp + 1930] = scrCols0[vRamTileValue6 & 0x040];
                        screenBytes[columnOffsetBmp + 1931] = scrCols0[vRamTileValue6 & 0x040];

                        screenBytes[columnOffsetBmp + 2560] = scrCols0[vRamTileValue1 & 0x020];
                        screenBytes[columnOffsetBmp + 2561] = scrCols0[vRamTileValue1 & 0x020];
                        screenBytes[columnOffsetBmp + 2562] = scrCols0[vRamTileValue2 & 0x020];
                        screenBytes[columnOffsetBmp + 2563] = scrCols0[vRamTileValue2 & 0x020];
                        screenBytes[columnOffsetBmp + 2564] = scrCols0[vRamTileValue3 & 0x020];
                        screenBytes[columnOffsetBmp + 2565] = scrCols0[vRamTileValue3 & 0x020];
                        screenBytes[columnOffsetBmp + 2566] = scrCols0[vRamTileValue4 & 0x020];
                        screenBytes[columnOffsetBmp + 2567] = scrCols0[vRamTileValue4 & 0x020];
                        screenBytes[columnOffsetBmp + 2568] = scrCols0[vRamTileValue5 & 0x020];
                        screenBytes[columnOffsetBmp + 2569] = scrCols0[vRamTileValue5 & 0x020];
                        screenBytes[columnOffsetBmp + 2570] = scrCols0[vRamTileValue6 & 0x020];
                        screenBytes[columnOffsetBmp + 2571] = scrCols0[vRamTileValue6 & 0x020];

                        screenBytes[columnOffsetBmp + 3200] = scrCols0[vRamTileValue1 & 0x020];
                        screenBytes[columnOffsetBmp + 3201] = scrCols0[vRamTileValue1 & 0x020];
                        screenBytes[columnOffsetBmp + 3202] = scrCols0[vRamTileValue2 & 0x020];
                        screenBytes[columnOffsetBmp + 3203] = scrCols0[vRamTileValue2 & 0x020];
                        screenBytes[columnOffsetBmp + 3204] = scrCols0[vRamTileValue3 & 0x020];
                        screenBytes[columnOffsetBmp + 3205] = scrCols0[vRamTileValue3 & 0x020];
                        screenBytes[columnOffsetBmp + 3206] = scrCols0[vRamTileValue4 & 0x020];
                        screenBytes[columnOffsetBmp + 3207] = scrCols0[vRamTileValue4 & 0x020];
                        screenBytes[columnOffsetBmp + 3208] = scrCols0[vRamTileValue5 & 0x020];
                        screenBytes[columnOffsetBmp + 3209] = scrCols0[vRamTileValue5 & 0x020];
                        screenBytes[columnOffsetBmp + 3210] = scrCols0[vRamTileValue6 & 0x020];
                        screenBytes[columnOffsetBmp + 3211] = scrCols0[vRamTileValue6 & 0x020];

                        screenBytes[columnOffsetBmp + 3840] = scrCols0[vRamTileValue1 & 0x010];
                        screenBytes[columnOffsetBmp + 3841] = scrCols0[vRamTileValue1 & 0x010];
                        screenBytes[columnOffsetBmp + 3842] = scrCols0[vRamTileValue2 & 0x010];
                        screenBytes[columnOffsetBmp + 3843] = scrCols0[vRamTileValue2 & 0x010];
                        screenBytes[columnOffsetBmp + 3844] = scrCols0[vRamTileValue3 & 0x010];
                        screenBytes[columnOffsetBmp + 3845] = scrCols0[vRamTileValue3 & 0x010];
                        screenBytes[columnOffsetBmp + 3846] = scrCols0[vRamTileValue4 & 0x010];
                        screenBytes[columnOffsetBmp + 3847] = scrCols0[vRamTileValue4 & 0x010];
                        screenBytes[columnOffsetBmp + 3848] = scrCols0[vRamTileValue5 & 0x010];
                        screenBytes[columnOffsetBmp + 3849] = scrCols0[vRamTileValue5 & 0x010];
                        screenBytes[columnOffsetBmp + 3850] = scrCols0[vRamTileValue6 & 0x010];
                        screenBytes[columnOffsetBmp + 3851] = scrCols0[vRamTileValue6 & 0x010];

                        screenBytes[columnOffsetBmp + 4480] = scrCols0[vRamTileValue1 & 0x010];
                        screenBytes[columnOffsetBmp + 4481] = scrCols0[vRamTileValue1 & 0x010];
                        screenBytes[columnOffsetBmp + 4482] = scrCols0[vRamTileValue2 & 0x010];
                        screenBytes[columnOffsetBmp + 4483] = scrCols0[vRamTileValue2 & 0x010];
                        screenBytes[columnOffsetBmp + 4484] = scrCols0[vRamTileValue3 & 0x010];
                        screenBytes[columnOffsetBmp + 4485] = scrCols0[vRamTileValue3 & 0x010];
                        screenBytes[columnOffsetBmp + 4486] = scrCols0[vRamTileValue4 & 0x010];
                        screenBytes[columnOffsetBmp + 4487] = scrCols0[vRamTileValue4 & 0x010];
                        screenBytes[columnOffsetBmp + 4488] = scrCols0[vRamTileValue5 & 0x010];
                        screenBytes[columnOffsetBmp + 4489] = scrCols0[vRamTileValue5 & 0x010];
                        screenBytes[columnOffsetBmp + 4490] = scrCols0[vRamTileValue6 & 0x010];
                        screenBytes[columnOffsetBmp + 4491] = scrCols0[vRamTileValue6 & 0x010];

                        screenBytes[columnOffsetBmp + 5120] = scrCols0[vRamTileValue1 & 8];
                        screenBytes[columnOffsetBmp + 5121] = scrCols0[vRamTileValue1 & 8];
                        screenBytes[columnOffsetBmp + 5122] = scrCols0[vRamTileValue2 & 8];
                        screenBytes[columnOffsetBmp + 5123] = scrCols0[vRamTileValue2 & 8];
                        screenBytes[columnOffsetBmp + 5124] = scrCols0[vRamTileValue3 & 8];
                        screenBytes[columnOffsetBmp + 5125] = scrCols0[vRamTileValue3 & 8];
                        screenBytes[columnOffsetBmp + 5126] = scrCols0[vRamTileValue4 & 8];
                        screenBytes[columnOffsetBmp + 5127] = scrCols0[vRamTileValue4 & 8];
                        screenBytes[columnOffsetBmp + 5128] = scrCols0[vRamTileValue5 & 8];
                        screenBytes[columnOffsetBmp + 5129] = scrCols0[vRamTileValue5 & 8];
                        screenBytes[columnOffsetBmp + 5130] = scrCols0[vRamTileValue6 & 8];
                        screenBytes[columnOffsetBmp + 5131] = scrCols0[vRamTileValue6 & 8];

                        screenBytes[columnOffsetBmp + 5760] = scrCols0[vRamTileValue1 & 8];
                        screenBytes[columnOffsetBmp + 5761] = scrCols0[vRamTileValue1 & 8];
                        screenBytes[columnOffsetBmp + 5762] = scrCols0[vRamTileValue2 & 8];
                        screenBytes[columnOffsetBmp + 5763] = scrCols0[vRamTileValue2 & 8];
                        screenBytes[columnOffsetBmp + 5764] = scrCols0[vRamTileValue3 & 8];
                        screenBytes[columnOffsetBmp + 5765] = scrCols0[vRamTileValue3 & 8];
                        screenBytes[columnOffsetBmp + 5766] = scrCols0[vRamTileValue4 & 8];
                        screenBytes[columnOffsetBmp + 5767] = scrCols0[vRamTileValue4 & 8];
                        screenBytes[columnOffsetBmp + 5768] = scrCols0[vRamTileValue5 & 8];
                        screenBytes[columnOffsetBmp + 5769] = scrCols0[vRamTileValue5 & 8];
                        screenBytes[columnOffsetBmp + 5770] = scrCols0[vRamTileValue6 & 8];
                        screenBytes[columnOffsetBmp + 5771] = scrCols0[vRamTileValue6 & 8];

                        screenBytes[columnOffsetBmp + 6400] = scrCols0[vRamTileValue1 & 4];
                        screenBytes[columnOffsetBmp + 6401] = scrCols0[vRamTileValue1 & 4];
                        screenBytes[columnOffsetBmp + 6402] = scrCols0[vRamTileValue2 & 4];
                        screenBytes[columnOffsetBmp + 6403] = scrCols0[vRamTileValue2 & 4];
                        screenBytes[columnOffsetBmp + 6404] = scrCols0[vRamTileValue3 & 4];
                        screenBytes[columnOffsetBmp + 6405] = scrCols0[vRamTileValue3 & 4];
                        screenBytes[columnOffsetBmp + 6406] = scrCols0[vRamTileValue4 & 4];
                        screenBytes[columnOffsetBmp + 6407] = scrCols0[vRamTileValue4 & 4];
                        screenBytes[columnOffsetBmp + 6408] = scrCols0[vRamTileValue5 & 4];
                        screenBytes[columnOffsetBmp + 6409] = scrCols0[vRamTileValue5 & 4];
                        screenBytes[columnOffsetBmp + 6410] = scrCols0[vRamTileValue6 & 4];
                        screenBytes[columnOffsetBmp + 6411] = scrCols0[vRamTileValue6 & 4];

                        screenBytes[columnOffsetBmp + 7040] = scrCols0[vRamTileValue1 & 4];
                        screenBytes[columnOffsetBmp + 7041] = scrCols0[vRamTileValue1 & 4];
                        screenBytes[columnOffsetBmp + 7042] = scrCols0[vRamTileValue2 & 4];
                        screenBytes[columnOffsetBmp + 7043] = scrCols0[vRamTileValue2 & 4];
                        screenBytes[columnOffsetBmp + 7044] = scrCols0[vRamTileValue3 & 4];
                        screenBytes[columnOffsetBmp + 7045] = scrCols0[vRamTileValue3 & 4];
                        screenBytes[columnOffsetBmp + 7046] = scrCols0[vRamTileValue4 & 4];
                        screenBytes[columnOffsetBmp + 7047] = scrCols0[vRamTileValue4 & 4];
                        screenBytes[columnOffsetBmp + 7048] = scrCols0[vRamTileValue5 & 4];
                        screenBytes[columnOffsetBmp + 7049] = scrCols0[vRamTileValue5 & 4];
                        screenBytes[columnOffsetBmp + 7050] = scrCols0[vRamTileValue6 & 4];
                        screenBytes[columnOffsetBmp + 7051] = scrCols0[vRamTileValue6 & 4];

                        screenBytes[columnOffsetBmp + 7680] = scrCols0[vRamTileValue1 & 2];
                        screenBytes[columnOffsetBmp + 7681] = scrCols0[vRamTileValue1 & 2];
                        screenBytes[columnOffsetBmp + 7682] = scrCols0[vRamTileValue2 & 2];
                        screenBytes[columnOffsetBmp + 7683] = scrCols0[vRamTileValue2 & 2];
                        screenBytes[columnOffsetBmp + 7684] = scrCols0[vRamTileValue3 & 2];
                        screenBytes[columnOffsetBmp + 7685] = scrCols0[vRamTileValue3 & 2];
                        screenBytes[columnOffsetBmp + 7686] = scrCols0[vRamTileValue4 & 2];
                        screenBytes[columnOffsetBmp + 7687] = scrCols0[vRamTileValue4 & 2];
                        screenBytes[columnOffsetBmp + 7688] = scrCols0[vRamTileValue5 & 2];
                        screenBytes[columnOffsetBmp + 7689] = scrCols0[vRamTileValue5 & 2];
                        screenBytes[columnOffsetBmp + 7690] = scrCols0[vRamTileValue6 & 2];
                        screenBytes[columnOffsetBmp + 7691] = scrCols0[vRamTileValue6 & 2];

                        screenBytes[columnOffsetBmp + 8320] = scrCols0[vRamTileValue1 & 2];
                        screenBytes[columnOffsetBmp + 8321] = scrCols0[vRamTileValue1 & 2];
                        screenBytes[columnOffsetBmp + 8322] = scrCols0[vRamTileValue2 & 2];
                        screenBytes[columnOffsetBmp + 8323] = scrCols0[vRamTileValue2 & 2];
                        screenBytes[columnOffsetBmp + 8324] = scrCols0[vRamTileValue3 & 2];
                        screenBytes[columnOffsetBmp + 8325] = scrCols0[vRamTileValue3 & 2];
                        screenBytes[columnOffsetBmp + 8326] = scrCols0[vRamTileValue4 & 2];
                        screenBytes[columnOffsetBmp + 8327] = scrCols0[vRamTileValue4 & 2];
                        screenBytes[columnOffsetBmp + 8328] = scrCols0[vRamTileValue5 & 2];
                        screenBytes[columnOffsetBmp + 8329] = scrCols0[vRamTileValue5 & 2];
                        screenBytes[columnOffsetBmp + 8330] = scrCols0[vRamTileValue6 & 2];
                        screenBytes[columnOffsetBmp + 8331] = scrCols0[vRamTileValue6 & 2];

                        screenBytes[columnOffsetBmp + 8960] = scrCols0[vRamTileValue1 & 1];
                        screenBytes[columnOffsetBmp + 8961] = scrCols0[vRamTileValue1 & 1];
                        screenBytes[columnOffsetBmp + 8962] = scrCols0[vRamTileValue2 & 1];
                        screenBytes[columnOffsetBmp + 8963] = scrCols0[vRamTileValue2 & 1];
                        screenBytes[columnOffsetBmp + 8964] = scrCols0[vRamTileValue3 & 1];
                        screenBytes[columnOffsetBmp + 8965] = scrCols0[vRamTileValue3 & 1];
                        screenBytes[columnOffsetBmp + 8966] = scrCols0[vRamTileValue4 & 1];
                        screenBytes[columnOffsetBmp + 8967] = scrCols0[vRamTileValue4 & 1];
                        screenBytes[columnOffsetBmp + 8968] = scrCols0[vRamTileValue5 & 1];
                        screenBytes[columnOffsetBmp + 8969] = scrCols0[vRamTileValue5 & 1];
                        screenBytes[columnOffsetBmp + 8970] = scrCols0[vRamTileValue6 & 1];
                        screenBytes[columnOffsetBmp + 8971] = scrCols0[vRamTileValue6 & 1];

                        screenBytes[columnOffsetBmp + 9600] = scrCols0[vRamTileValue1 & 1];
                        screenBytes[columnOffsetBmp + 9601] = scrCols0[vRamTileValue1 & 1];
                        screenBytes[columnOffsetBmp + 9602] = scrCols0[vRamTileValue2 & 1];
                        screenBytes[columnOffsetBmp + 9603] = scrCols0[vRamTileValue2 & 1];
                        screenBytes[columnOffsetBmp + 9604] = scrCols0[vRamTileValue3 & 1];
                        screenBytes[columnOffsetBmp + 9605] = scrCols0[vRamTileValue3 & 1];
                        screenBytes[columnOffsetBmp + 9606] = scrCols0[vRamTileValue4 & 1];
                        screenBytes[columnOffsetBmp + 9607] = scrCols0[vRamTileValue4 & 1];
                        screenBytes[columnOffsetBmp + 9608] = scrCols0[vRamTileValue5 & 1];
                        screenBytes[columnOffsetBmp + 9609] = scrCols0[vRamTileValue5 & 1];
                        screenBytes[columnOffsetBmp + 9610] = scrCols0[vRamTileValue6 & 1];
                        screenBytes[columnOffsetBmp + 9611] = scrCols0[vRamTileValue6 & 1];

                        tileScreenIndex++;
                        tileColorIndex++;
                        tileOffsetVram += CharWidth;
                        columnOffsetBmp += 12;
                        if ((tileScreenIndex % 53) == 0)
                        {
                            colfg = (byte)(Ram[VIDEO_CLUT_START + tileColorIndex] >> 3);
                            scrCols0[0] = (byte)(Ram[VIDEO_CLUT_START + tileColorIndex] & 0x07);
                            scrCols0[1] = colfg;
                            scrCols0[2] = colfg;
                            scrCols0[4] = colfg;
                            scrCols0[8] = colfg;
                            scrCols0[0x010] = colfg;
                            scrCols0[0x020] = colfg;
                            scrCols0[0x040] = colfg;
                            scrCols0[0x080] = colfg;

                            vRamTileValue1 = Ram[tileOffsetVram];
                            vRamTileValue2 = Ram[tileOffsetVram + 1];

                            screenBytes[columnOffsetBmp] = scrCols0[vRamTileValue1 & 0x080];
                            screenBytes[columnOffsetBmp + 1] = scrCols0[vRamTileValue1 & 0x080];
                            screenBytes[columnOffsetBmp + 2] = scrCols0[vRamTileValue2 & 0x080];
                            screenBytes[columnOffsetBmp + 3] = scrCols0[vRamTileValue2 & 0x080];

                            screenBytes[columnOffsetBmp + 640] = scrCols0[vRamTileValue1 & 0x080];
                            screenBytes[columnOffsetBmp + 641] = scrCols0[vRamTileValue1 & 0x080];
                            screenBytes[columnOffsetBmp + 642] = scrCols0[vRamTileValue2 & 0x080];
                            screenBytes[columnOffsetBmp + 643] = scrCols0[vRamTileValue2 & 0x080];

                            screenBytes[columnOffsetBmp + 1280] = scrCols0[vRamTileValue1 & 0x040];
                            screenBytes[columnOffsetBmp + 1281] = scrCols0[vRamTileValue1 & 0x040];
                            screenBytes[columnOffsetBmp + 1282] = scrCols0[vRamTileValue2 & 0x040];
                            screenBytes[columnOffsetBmp + 1283] = scrCols0[vRamTileValue2 & 0x040];

                            screenBytes[columnOffsetBmp + 1920] = scrCols0[vRamTileValue1 & 0x040];
                            screenBytes[columnOffsetBmp + 1921] = scrCols0[vRamTileValue1 & 0x040];
                            screenBytes[columnOffsetBmp + 1922] = scrCols0[vRamTileValue2 & 0x040];
                            screenBytes[columnOffsetBmp + 1923] = scrCols0[vRamTileValue2 & 0x040];

                            screenBytes[columnOffsetBmp + 2560] = scrCols0[vRamTileValue1 & 0x020];
                            screenBytes[columnOffsetBmp + 2561] = scrCols0[vRamTileValue1 & 0x020];
                            screenBytes[columnOffsetBmp + 2562] = scrCols0[vRamTileValue2 & 0x020];
                            screenBytes[columnOffsetBmp + 2563] = scrCols0[vRamTileValue2 & 0x020];

                            screenBytes[columnOffsetBmp + 3200] = scrCols0[vRamTileValue1 & 0x020];
                            screenBytes[columnOffsetBmp + 3201] = scrCols0[vRamTileValue1 & 0x020];
                            screenBytes[columnOffsetBmp + 3202] = scrCols0[vRamTileValue2 & 0x020];
                            screenBytes[columnOffsetBmp + 3203] = scrCols0[vRamTileValue2 & 0x020];

                            screenBytes[columnOffsetBmp + 3840] = scrCols0[vRamTileValue1 & 0x010];
                            screenBytes[columnOffsetBmp + 3841] = scrCols0[vRamTileValue1 & 0x010];
                            screenBytes[columnOffsetBmp + 3842] = scrCols0[vRamTileValue2 & 0x010];
                            screenBytes[columnOffsetBmp + 3843] = scrCols0[vRamTileValue2 & 0x010];

                            screenBytes[columnOffsetBmp + 4480] = scrCols0[vRamTileValue1 & 0x010];
                            screenBytes[columnOffsetBmp + 4481] = scrCols0[vRamTileValue1 & 0x010];
                            screenBytes[columnOffsetBmp + 4482] = scrCols0[vRamTileValue2 & 0x010];
                            screenBytes[columnOffsetBmp + 4483] = scrCols0[vRamTileValue2 & 0x010];

                            screenBytes[columnOffsetBmp + 5120] = scrCols0[vRamTileValue1 & 8];
                            screenBytes[columnOffsetBmp + 5121] = scrCols0[vRamTileValue1 & 8];
                            screenBytes[columnOffsetBmp + 5122] = scrCols0[vRamTileValue2 & 8];
                            screenBytes[columnOffsetBmp + 5123] = scrCols0[vRamTileValue2 & 8];

                            screenBytes[columnOffsetBmp + 5760] = scrCols0[vRamTileValue1 & 8];
                            screenBytes[columnOffsetBmp + 5761] = scrCols0[vRamTileValue1 & 8];
                            screenBytes[columnOffsetBmp + 5762] = scrCols0[vRamTileValue2 & 8];
                            screenBytes[columnOffsetBmp + 5763] = scrCols0[vRamTileValue2 & 8];

                            screenBytes[columnOffsetBmp + 6400] = scrCols0[vRamTileValue1 & 4];
                            screenBytes[columnOffsetBmp + 6401] = scrCols0[vRamTileValue1 & 4];
                            screenBytes[columnOffsetBmp + 6402] = scrCols0[vRamTileValue2 & 4];
                            screenBytes[columnOffsetBmp + 6403] = scrCols0[vRamTileValue2 & 4];

                            screenBytes[columnOffsetBmp + 7040] = scrCols0[vRamTileValue1 & 4];
                            screenBytes[columnOffsetBmp + 7041] = scrCols0[vRamTileValue1 & 4];
                            screenBytes[columnOffsetBmp + 7042] = scrCols0[vRamTileValue2 & 4];
                            screenBytes[columnOffsetBmp + 7043] = scrCols0[vRamTileValue2 & 4];

                            screenBytes[columnOffsetBmp + 7680] = scrCols0[vRamTileValue1 & 2];
                            screenBytes[columnOffsetBmp + 7681] = scrCols0[vRamTileValue1 & 2];
                            screenBytes[columnOffsetBmp + 7682] = scrCols0[vRamTileValue2 & 2];
                            screenBytes[columnOffsetBmp + 7683] = scrCols0[vRamTileValue2 & 2];

                            screenBytes[columnOffsetBmp + 8320] = scrCols0[vRamTileValue1 & 2];
                            screenBytes[columnOffsetBmp + 8321] = scrCols0[vRamTileValue1 & 2];
                            screenBytes[columnOffsetBmp + 8322] = scrCols0[vRamTileValue2 & 2];
                            screenBytes[columnOffsetBmp + 8323] = scrCols0[vRamTileValue2 & 2];

                            screenBytes[columnOffsetBmp + 8960] = scrCols0[vRamTileValue1 & 1];
                            screenBytes[columnOffsetBmp + 8961] = scrCols0[vRamTileValue1 & 1];
                            screenBytes[columnOffsetBmp + 8962] = scrCols0[vRamTileValue2 & 1];
                            screenBytes[columnOffsetBmp + 8963] = scrCols0[vRamTileValue2 & 1];

                            screenBytes[columnOffsetBmp + 9600] = scrCols0[vRamTileValue1 & 1];
                            screenBytes[columnOffsetBmp + 9601] = scrCols0[vRamTileValue1 & 1];
                            screenBytes[columnOffsetBmp + 9602] = scrCols0[vRamTileValue2 & 1];
                            screenBytes[columnOffsetBmp + 9603] = scrCols0[vRamTileValue2 & 1];

                            tileOffsetVram += 2;
                            columnOffsetBmp = (tileScreenIndex / 53) * 10240;
                            tileColorIndex++;
                        }
                    }
                    break;
            }

            lock (copylock)
            {
                // sprites
                switch (videoMode)
                {
                    case 1:
                        for (int sprnum = 0; sprnum < SPRITES_NUM; sprnum++)
                        {
                            int sparams = ((spriteBank & 1) == 1 ? SPRITE_PARAMS_1_2 : SPRITE_PARAMS_1_1) + (sprnum << 3);
                            int sdata = ((spriteBank & 2) == 2 ? SPRITE_DATA_1_2 : SPRITE_DATA_1_1) + (sprnum << 7);
                            if (Ram[sparams + SPRITE_ENABLE] == 1)
                                BlitSpriteMode1(ref sparams, ref sdata);
                        }
                        break;

                    default:
                        for (int sprnum = 0; sprnum < SPRITES_NUM; sprnum++)
                        {
                            int sparams = ((spriteBank & 1) == 1 ? SPRITE_PARAMS_0_2 : SPRITE_PARAMS_0_1) + (sprnum << 3);
                            int sdata = ((spriteBank & 2) == 2 ? SPRITE_DATA_0_2 : SPRITE_DATA_0_1) + (sprnum << 5);
                            if (Ram[sparams + SPRITE_ENABLE] == 1)
                                BlitSpriteMode0(ref sparams, ref sdata);
                        }
                        break;
                }
                BitmapData bmpdata = screenBitMap.LockBits(new Rectangle(0, 0, Width << 1, Height0 << 1), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                Marshal.Copy(screenBytes, 0, bmpdata.Scan0, screenBytes.Length);
                screenBitMap.UnlockBits(bmpdata);
                vblCounter++;
                Memory.Data[Cpu.COUNTER] = (byte)(vblCounter >> 8);
                Memory.Data[Cpu.COUNTER + 1] = (byte)(vblCounter);
                return screenBitMap;
            }
        }
        
        private static void BlitSpriteMode1(ref int spriteParams, ref int spriteData)
        {
            int __maxclip = screenBytes.Length - 64 - borderZeroBmpBytesNum;
            int x = (Ram[spriteParams++] << 8 | Ram[spriteParams++]) << 1;
            int y = (Ram[spriteParams++] << 8 | Ram[spriteParams]) << 1;
            int srcScreenIndex = borderZeroBmpBytesNum + y * 640 + x;
            for (int sprndx = 0; sprndx < 16; sprndx++)
            {
                if (srcScreenIndex >= __maxclip)
                    break;
                sprMaskBytes[00] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprMaskBytes[01] = sprMaskBytes[00];
                sprColorBytes[00] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprColorBytes[01] = sprColorBytes[00];
                sprMaskBytes[02] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprMaskBytes[03] = sprMaskBytes[02];
                sprColorBytes[02] = (byte)(Ram[spriteData++] & 0x07);
                sprColorBytes[03] = sprColorBytes[02];
                sprMaskBytes[04] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprMaskBytes[05] = sprMaskBytes[04];
                sprColorBytes[04] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprColorBytes[05] = sprColorBytes[04];
                sprMaskBytes[06] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprMaskBytes[07] = sprMaskBytes[06];
                sprColorBytes[06] = (byte)(Ram[spriteData++] & 0x07);
                sprColorBytes[07] = sprColorBytes[06];
                sprMaskBytes[08] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprMaskBytes[09] = sprMaskBytes[08];
                sprColorBytes[08] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprColorBytes[09] = sprColorBytes[08];
                sprMaskBytes[10] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprMaskBytes[11] = sprMaskBytes[10];
                sprColorBytes[10] = (byte)(Ram[spriteData++] & 0x07);
                sprColorBytes[11] = sprColorBytes[10];
                sprMaskBytes[12] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprMaskBytes[13] = sprMaskBytes[12];
                sprColorBytes[12] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprColorBytes[13] = sprColorBytes[12];
                sprMaskBytes[14] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprMaskBytes[15] = sprMaskBytes[14];
                sprColorBytes[14] = (byte)(Ram[spriteData++] & 0x07);
                sprColorBytes[15] = sprColorBytes[14];
                sprMaskBytes[16] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprMaskBytes[17] = sprMaskBytes[16];
                sprColorBytes[16] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprColorBytes[17] = sprColorBytes[16];
                sprMaskBytes[18] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprMaskBytes[19] = sprMaskBytes[18];
                sprColorBytes[18] = (byte)(Ram[spriteData++] & 0x07);
                sprColorBytes[19] = sprColorBytes[18];
                sprMaskBytes[20] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprMaskBytes[21] = sprMaskBytes[20];
                sprColorBytes[20] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprColorBytes[21] = sprColorBytes[20];
                sprMaskBytes[22] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprMaskBytes[23] = sprMaskBytes[22];
                sprColorBytes[22] = (byte)(Ram[spriteData++] & 0x07);
                sprColorBytes[23] = sprColorBytes[22];
                sprMaskBytes[24] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprMaskBytes[25] = sprMaskBytes[24];
                sprColorBytes[24] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprColorBytes[25] = sprColorBytes[24];
                sprMaskBytes[26] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprMaskBytes[27] = sprMaskBytes[26];
                sprColorBytes[26] = (byte)(Ram[spriteData++] & 0x07);
                sprColorBytes[27] = sprColorBytes[26];
                sprMaskBytes[28] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprMaskBytes[29] = sprMaskBytes[28];
                sprColorBytes[28] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprColorBytes[29] = sprColorBytes[28];
                sprMaskBytes[30] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprMaskBytes[31] = sprMaskBytes[30];
                sprColorBytes[30] = (byte)(Ram[spriteData++] & 0x07);
                sprColorBytes[31] = sprColorBytes[30];
                Buffer.BlockCopy(sprMaskBytes, 0, maskBitBlock, 0, 32);
                Buffer.BlockCopy(sprColorBytes, 0, spriteBitBlock, 0, 32);
                Buffer.BlockCopy(screenBytes, srcScreenIndex, screenBitBlock, 0, 32);
                rasterOpBitBlock[0] = screenBitBlock[0] & maskBitBlock[0] | spriteBitBlock[0];
                rasterOpBitBlock[1] = screenBitBlock[1] & maskBitBlock[1] | spriteBitBlock[1];
                rasterOpBitBlock[2] = screenBitBlock[2] & maskBitBlock[2] | spriteBitBlock[2];
                rasterOpBitBlock[3] = screenBitBlock[3] & maskBitBlock[3] | spriteBitBlock[3];
                Buffer.BlockCopy(rasterOpBitBlock, 0, screenBytes, srcScreenIndex, 32);
                srcScreenIndex += 640;
                Buffer.BlockCopy(rasterOpBitBlock, 0, screenBytes, srcScreenIndex, 32);
                srcScreenIndex += 640;
            }
        }

        private static void BlitSpriteMode0(ref int spriteParams, ref int spriteData)
        {
            sprColVals[1] = Ram[spriteParams + SPRITE_COLOR]; // [0] always black
            int x = (Ram[spriteParams++] << 8 | Ram[spriteParams++]) << 1;
            int y = (Ram[spriteParams++] << 8 | Ram[spriteParams]) << 1;
            int srcScreenIndex = y * 640 + x;
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
                // mask
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
                srcScreenIndex += 640;
                if (srcScreenIndex >= screenBytes.Length - 32)
                    break;
                Buffer.BlockCopy(rasterOpBitBlock, 0, screenBytes, srcScreenIndex, 32);
                srcScreenIndex += 640;
            }
        }
        public static object copylock = new object();
    }
}
