using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

namespace LionComputerEmulator
{
    public static class Display
    {
        private static Bitmap screenBitMap = new Bitmap(640, 480, PixelFormat.Format8bppIndexed);
        private static ColorPalette screenPalette = screenBitMap.Palette;
        private static byte[] screenBytes = new byte[screenBitMap.Width * screenBitMap.Height]; // 8 bpp space x2 size
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

        private static int spriteBuffer = 0; // updated from sprite port worker
        private static int videoMode = 0;  // updated from videomode port worker

        private static uint vblCounter = 0;
        private static bool triggerMode = false;

        private const int borderZeroBmpBytesNum = 640 * 40;

        /// <summary>
        /// Mode 1 Sprite Bank A Parameters Buffer 1
        /// </summary>
        public const int SPRITE_A_PARAMS_1 = 16384;

        /// <summary>
        /// Mode 1 Sprite Bank A Parameters Buffer 2
        /// </summary>
        public const int SPRITE_A_PARAMS_2 = SPRITE_A_PARAMS_1 + 256;

        /// <summary>
        /// Mode 1 Sprite Bank A Data Buffer 1
        /// </summary>
        public const int SPRITE_A_DATA_1 = SPRITE_A_PARAMS_2 + 256;

        /// <summary>
        /// Mode 1 Sprite Bank A Data Buffer 2
        /// </summary>
        public const int SPRITE_A_DATA_2 = SPRITE_A_PARAMS_1 + 2304;

        /// <summary>
        /// Sprite Bank B Parameters Buffer 1
        /// </summary>
        public const int SPRITE_B_PARAMS_1 = SPRITE_A_PARAMS_1 + 4096;

        /// <summary>
        /// Sprite Bank B Parameters Buffer 2
        /// </summary>
        public const int SPRITE_B_PARAMS_2 = SPRITE_B_PARAMS_1 + 256;

        /// <summary>
        /// Sprite Bank B Data Buffer 1
        /// </summary>
        public const int SPRITE_B_DATA_1 = SPRITE_B_PARAMS_2 + 256;

        /// <summary>
        /// Sprite Bank B Data Buffer 2
        /// </summary>
        public const int SPRITE_B_DATA_2 = SPRITE_B_PARAMS_1 + 2304;

        /// <summary>
        /// Sprite Bank C Parameters Buffer 1
        /// </summary>
        public const int SPRITE_C_PARAMS_1 = SPRITE_B_PARAMS_1 + 4096;

        /// <summary>
        /// Sprite Bank C Parameters Buffer 2
        /// </summary>
        public const int SPRITE_C_PARAMS_2 = SPRITE_C_PARAMS_1 + 256;

        /// <summary>
        /// Sprite Bank C Data Buffer 1
        /// </summary>
        public const int SPRITE_C_DATA_1 = SPRITE_C_PARAMS_2 + 256;

        /// <summary>
        /// Sprite Bank C Data Buffer 2
        /// </summary>
        public const int SPRITE_C_DATA_2 = SPRITE_C_PARAMS_1 + 2304;

        // offsets in sprite parameters
        public const int SPRITE_X = 0;       // word
        public const int SPRITE_Y = 2;       // word
        public const int SPRITE_DX = 4;      // byte
        public const int SPRITE_DY = 5;      // byte
        public const int SPRITE_COLOR = 6;   // byte
        public const int SPRITE_ENABLE = 7;  // byte

        public const int SPRITES_NUM = 14;

        /// <summary>
        /// Start of VIDEO RAM Address Mode 0
        /// </summary>
        public const int VIDEO_RAM_START_MODE0 = 0x08000;

        /// <summary>
        /// End of VIDEO RAM Address Mode 0
        /// </summary>
        public const int VIDEO_RAM_END_MODE0 = VIDEO_RAM_START_MODE0 + (640 * 240) / 8;

        /// <summary>
        /// Start of Video Color Table Mode 0
        /// </summary>
        public const int VIDEO_COLOR_START = VIDEO_RAM_START_MODE0 + SPRITE_A_PARAMS_1 + 12000;

        /// <summary>
        /// End of Video Color Table Mode 0
        /// </summary>
        public const int VIDEO_COLOR_END = VIDEO_COLOR_START + 2400;

        /// <summary>
        /// Start of VIDEO RAM Address Mode 1
        /// </summary>
        public const int VIDEO_RAM_START_MODE1 = 0x08000;

        /// <summary>
        /// End of VIDEO RAM Address Mode 1
        /// </summary>
        public const int VIDEO_RAM_END_MODE1 = VIDEO_RAM_START_MODE1 + 32000;

        public const int CharWidth = 8;

        public const int CharHeight = 8;

        public const int CharXDimension = 640 / CharWidth;

        public const int CharYDimension = 240 / CharHeight;

        /// <summary>
        /// Memory Data Byte Array
        /// </summary>
        public static byte[] Ram = new byte[0x010000];

        // argument passed from the device port access
        private static void SpritePortWork(object sender, DoWorkEventArgs e)
        {
            lock (copylock)
                spriteBuffer = Convert.ToInt32(e.Argument);
        }

        // argument passed from the videomode port access
        private static void VideoModePortWork(object sender, DoWorkEventArgs e)
        {
            lock (copylock)
            {
                videoMode = Convert.ToInt32(e.Argument);
                triggerMode = videoMode == 1;
            }
        }

        public static void InitScreen()
        {
            screenPalette.Entries[0] = Color.FromArgb(0, 0, 0);
            screenPalette.Entries[1] = Color.FromArgb(0, 0, 200);
            screenPalette.Entries[2] = Color.FromArgb(0, 200, 0);
            screenPalette.Entries[3] = Color.FromArgb(0, 200, 200);
            screenPalette.Entries[4] = Color.FromArgb(200, 0, 0);
            screenPalette.Entries[5] = Color.FromArgb(200, 0, 200);
            screenPalette.Entries[6] = Color.FromArgb(160, 160, 0);
            screenPalette.Entries[7] = Color.FromArgb(200, 200, 200);
            screenPalette.Entries[8] = Color.FromArgb(56, 56, 56);
            screenPalette.Entries[9] = Color.FromArgb(0, 0, 255);
            screenPalette.Entries[10] = Color.FromArgb(0, 255, 0);
            screenPalette.Entries[11] = Color.FromArgb(0, 255, 255);
            screenPalette.Entries[12] = Color.FromArgb(255, 0, 0);
            screenPalette.Entries[13] = Color.FromArgb(255, 0, 255);
            screenPalette.Entries[14] = Color.FromArgb(255, 255, 0);
            screenPalette.Entries[15] = Color.FromArgb(255, 255, 255);

            SpritePortWorker.DoWork += SpritePortWork;
            VideoModePortWorker.DoWork += VideoModePortWork;
            vblCounter = 0;
        }

        public static Bitmap Screen()
        {
            screenBitMap.Palette = screenPalette;
            switch (videoMode)
            {
                case 1:
                    int scrBytesNdx = borderZeroBmpBytesNum;
                    if (triggerMode)
                    {
                        // clear mode1 borders on bitmap
                        Buffer.BlockCopy(borderZeroBmp, 0, screenBytes, 0, borderZeroBmpBytesNum);
                        Buffer.BlockCopy(borderZeroBmp, 0, screenBytes, 640 * 440, borderZeroBmpBytesNum);
                        triggerMode = false;
                    }
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
                    byte vRamTileValue7 = 0;
                    byte vRamTileValue8 = 0;

                    byte colfg = 0;

                    while (tileScreenIndex < CharXDimension * CharYDimension)
                    {
                        colfg = (byte)(Ram[VIDEO_COLOR_START + tileColorIndex] >> 4);
                        scrCols0[0] = (byte)(Ram[VIDEO_COLOR_START + tileColorIndex] & 0x0f);
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
                        vRamTileValue7 = Ram[tileOffsetVram + 6];
                        vRamTileValue8 = Ram[tileOffsetVram + 7];

                        screenBytes[columnOffsetBmp] = scrCols0[vRamTileValue1 & 0x080];
                        screenBytes[columnOffsetBmp + 1] = scrCols0[vRamTileValue2 & 0x080];
                        screenBytes[columnOffsetBmp + 2] = scrCols0[vRamTileValue3 & 0x080];
                        screenBytes[columnOffsetBmp + 3] = scrCols0[vRamTileValue4 & 0x080];
                        screenBytes[columnOffsetBmp + 4] = scrCols0[vRamTileValue5 & 0x080];
                        screenBytes[columnOffsetBmp + 5] = scrCols0[vRamTileValue6 & 0x080];
                        screenBytes[columnOffsetBmp + 6] = scrCols0[vRamTileValue7 & 0x080];
                        screenBytes[columnOffsetBmp + 7] = scrCols0[vRamTileValue8 & 0x080];

                        screenBytes[columnOffsetBmp + 640] = scrCols0[vRamTileValue1 & 0x080];
                        screenBytes[columnOffsetBmp + 641] = scrCols0[vRamTileValue2 & 0x080];
                        screenBytes[columnOffsetBmp + 642] = scrCols0[vRamTileValue3 & 0x080];
                        screenBytes[columnOffsetBmp + 643] = scrCols0[vRamTileValue4 & 0x080];
                        screenBytes[columnOffsetBmp + 644] = scrCols0[vRamTileValue5 & 0x080];
                        screenBytes[columnOffsetBmp + 645] = scrCols0[vRamTileValue6 & 0x080];
                        screenBytes[columnOffsetBmp + 646] = scrCols0[vRamTileValue7 & 0x080];
                        screenBytes[columnOffsetBmp + 647] = scrCols0[vRamTileValue8 & 0x080];

                        screenBytes[columnOffsetBmp + 1280] = scrCols0[vRamTileValue1 & 0x040];
                        screenBytes[columnOffsetBmp + 1281] = scrCols0[vRamTileValue2 & 0x040];
                        screenBytes[columnOffsetBmp + 1282] = scrCols0[vRamTileValue3 & 0x040];
                        screenBytes[columnOffsetBmp + 1283] = scrCols0[vRamTileValue4 & 0x040];
                        screenBytes[columnOffsetBmp + 1284] = scrCols0[vRamTileValue5 & 0x040];
                        screenBytes[columnOffsetBmp + 1285] = scrCols0[vRamTileValue6 & 0x040];
                        screenBytes[columnOffsetBmp + 1286] = scrCols0[vRamTileValue7 & 0x040];
                        screenBytes[columnOffsetBmp + 1287] = scrCols0[vRamTileValue8 & 0x040];

                        screenBytes[columnOffsetBmp + 1920] = scrCols0[vRamTileValue1 & 0x040];
                        screenBytes[columnOffsetBmp + 1921] = scrCols0[vRamTileValue2 & 0x040];
                        screenBytes[columnOffsetBmp + 1922] = scrCols0[vRamTileValue3 & 0x040];
                        screenBytes[columnOffsetBmp + 1923] = scrCols0[vRamTileValue4 & 0x040];
                        screenBytes[columnOffsetBmp + 1924] = scrCols0[vRamTileValue5 & 0x040];
                        screenBytes[columnOffsetBmp + 1925] = scrCols0[vRamTileValue6 & 0x040];
                        screenBytes[columnOffsetBmp + 1926] = scrCols0[vRamTileValue7 & 0x040];
                        screenBytes[columnOffsetBmp + 1927] = scrCols0[vRamTileValue8 & 0x040];

                        screenBytes[columnOffsetBmp + 2560] = scrCols0[vRamTileValue1 & 0x020];
                        screenBytes[columnOffsetBmp + 2561] = scrCols0[vRamTileValue2 & 0x020];
                        screenBytes[columnOffsetBmp + 2562] = scrCols0[vRamTileValue3 & 0x020];
                        screenBytes[columnOffsetBmp + 2563] = scrCols0[vRamTileValue4 & 0x020];
                        screenBytes[columnOffsetBmp + 2564] = scrCols0[vRamTileValue5 & 0x020];
                        screenBytes[columnOffsetBmp + 2565] = scrCols0[vRamTileValue6 & 0x020];
                        screenBytes[columnOffsetBmp + 2566] = scrCols0[vRamTileValue7 & 0x020];
                        screenBytes[columnOffsetBmp + 2567] = scrCols0[vRamTileValue8 & 0x020];

                        screenBytes[columnOffsetBmp + 3200] = scrCols0[vRamTileValue1 & 0x020];
                        screenBytes[columnOffsetBmp + 3201] = scrCols0[vRamTileValue2 & 0x020];
                        screenBytes[columnOffsetBmp + 3202] = scrCols0[vRamTileValue3 & 0x020];
                        screenBytes[columnOffsetBmp + 3203] = scrCols0[vRamTileValue4 & 0x020];
                        screenBytes[columnOffsetBmp + 3204] = scrCols0[vRamTileValue5 & 0x020];
                        screenBytes[columnOffsetBmp + 3205] = scrCols0[vRamTileValue6 & 0x020];
                        screenBytes[columnOffsetBmp + 3206] = scrCols0[vRamTileValue7 & 0x020];
                        screenBytes[columnOffsetBmp + 3207] = scrCols0[vRamTileValue8 & 0x020];

                        screenBytes[columnOffsetBmp + 3840] = scrCols0[vRamTileValue1 & 0x010];
                        screenBytes[columnOffsetBmp + 3841] = scrCols0[vRamTileValue2 & 0x010];
                        screenBytes[columnOffsetBmp + 3842] = scrCols0[vRamTileValue3 & 0x010];
                        screenBytes[columnOffsetBmp + 3843] = scrCols0[vRamTileValue4 & 0x010];
                        screenBytes[columnOffsetBmp + 3844] = scrCols0[vRamTileValue5 & 0x010];
                        screenBytes[columnOffsetBmp + 3845] = scrCols0[vRamTileValue6 & 0x010];
                        screenBytes[columnOffsetBmp + 3846] = scrCols0[vRamTileValue7 & 0x010];
                        screenBytes[columnOffsetBmp + 3847] = scrCols0[vRamTileValue8 & 0x010];

                        screenBytes[columnOffsetBmp + 4480] = scrCols0[vRamTileValue1 & 0x010];
                        screenBytes[columnOffsetBmp + 4481] = scrCols0[vRamTileValue2 & 0x010];
                        screenBytes[columnOffsetBmp + 4482] = scrCols0[vRamTileValue3 & 0x010];
                        screenBytes[columnOffsetBmp + 4483] = scrCols0[vRamTileValue4 & 0x010];
                        screenBytes[columnOffsetBmp + 4484] = scrCols0[vRamTileValue5 & 0x010];
                        screenBytes[columnOffsetBmp + 4485] = scrCols0[vRamTileValue6 & 0x010];
                        screenBytes[columnOffsetBmp + 4486] = scrCols0[vRamTileValue7 & 0x010];
                        screenBytes[columnOffsetBmp + 4487] = scrCols0[vRamTileValue8 & 0x010];

                        screenBytes[columnOffsetBmp + 5120] = scrCols0[vRamTileValue1 & 8];
                        screenBytes[columnOffsetBmp + 5121] = scrCols0[vRamTileValue2 & 8];
                        screenBytes[columnOffsetBmp + 5122] = scrCols0[vRamTileValue3 & 8];
                        screenBytes[columnOffsetBmp + 5123] = scrCols0[vRamTileValue4 & 8];
                        screenBytes[columnOffsetBmp + 5124] = scrCols0[vRamTileValue5 & 8];
                        screenBytes[columnOffsetBmp + 5125] = scrCols0[vRamTileValue6 & 8];
                        screenBytes[columnOffsetBmp + 5126] = scrCols0[vRamTileValue7 & 8];
                        screenBytes[columnOffsetBmp + 5127] = scrCols0[vRamTileValue8 & 8];

                        screenBytes[columnOffsetBmp + 5760] = scrCols0[vRamTileValue1 & 8];
                        screenBytes[columnOffsetBmp + 5761] = scrCols0[vRamTileValue2 & 8];
                        screenBytes[columnOffsetBmp + 5762] = scrCols0[vRamTileValue3 & 8];
                        screenBytes[columnOffsetBmp + 5763] = scrCols0[vRamTileValue4 & 8];
                        screenBytes[columnOffsetBmp + 5764] = scrCols0[vRamTileValue5 & 8];
                        screenBytes[columnOffsetBmp + 5765] = scrCols0[vRamTileValue6 & 8];
                        screenBytes[columnOffsetBmp + 5766] = scrCols0[vRamTileValue7 & 8];
                        screenBytes[columnOffsetBmp + 5767] = scrCols0[vRamTileValue8 & 8];

                        screenBytes[columnOffsetBmp + 6400] = scrCols0[vRamTileValue1 & 4];
                        screenBytes[columnOffsetBmp + 6401] = scrCols0[vRamTileValue2 & 4];
                        screenBytes[columnOffsetBmp + 6402] = scrCols0[vRamTileValue3 & 4];
                        screenBytes[columnOffsetBmp + 6403] = scrCols0[vRamTileValue4 & 4];
                        screenBytes[columnOffsetBmp + 6404] = scrCols0[vRamTileValue5 & 4];
                        screenBytes[columnOffsetBmp + 6405] = scrCols0[vRamTileValue6 & 4];
                        screenBytes[columnOffsetBmp + 6406] = scrCols0[vRamTileValue7 & 4];
                        screenBytes[columnOffsetBmp + 6407] = scrCols0[vRamTileValue8 & 4];

                        screenBytes[columnOffsetBmp + 7040] = scrCols0[vRamTileValue1 & 4];
                        screenBytes[columnOffsetBmp + 7041] = scrCols0[vRamTileValue2 & 4];
                        screenBytes[columnOffsetBmp + 7042] = scrCols0[vRamTileValue3 & 4];
                        screenBytes[columnOffsetBmp + 7043] = scrCols0[vRamTileValue4 & 4];
                        screenBytes[columnOffsetBmp + 7044] = scrCols0[vRamTileValue5 & 4];
                        screenBytes[columnOffsetBmp + 7045] = scrCols0[vRamTileValue6 & 4];
                        screenBytes[columnOffsetBmp + 7046] = scrCols0[vRamTileValue7 & 4];
                        screenBytes[columnOffsetBmp + 7047] = scrCols0[vRamTileValue8 & 4];

                        screenBytes[columnOffsetBmp + 7680] = scrCols0[vRamTileValue1 & 2];
                        screenBytes[columnOffsetBmp + 7681] = scrCols0[vRamTileValue2 & 2];
                        screenBytes[columnOffsetBmp + 7682] = scrCols0[vRamTileValue3 & 2];
                        screenBytes[columnOffsetBmp + 7683] = scrCols0[vRamTileValue4 & 2];
                        screenBytes[columnOffsetBmp + 7684] = scrCols0[vRamTileValue5 & 2];
                        screenBytes[columnOffsetBmp + 7685] = scrCols0[vRamTileValue6 & 2];
                        screenBytes[columnOffsetBmp + 7686] = scrCols0[vRamTileValue7 & 2];
                        screenBytes[columnOffsetBmp + 7687] = scrCols0[vRamTileValue8 & 2];

                        screenBytes[columnOffsetBmp + 8320] = scrCols0[vRamTileValue1 & 2];
                        screenBytes[columnOffsetBmp + 8321] = scrCols0[vRamTileValue2 & 2];
                        screenBytes[columnOffsetBmp + 8322] = scrCols0[vRamTileValue3 & 2];
                        screenBytes[columnOffsetBmp + 8323] = scrCols0[vRamTileValue4 & 2];
                        screenBytes[columnOffsetBmp + 8324] = scrCols0[vRamTileValue5 & 2];
                        screenBytes[columnOffsetBmp + 8325] = scrCols0[vRamTileValue6 & 2];
                        screenBytes[columnOffsetBmp + 8326] = scrCols0[vRamTileValue7 & 2];
                        screenBytes[columnOffsetBmp + 8327] = scrCols0[vRamTileValue8 & 2];

                        screenBytes[columnOffsetBmp + 8960] = scrCols0[vRamTileValue1 & 1];
                        screenBytes[columnOffsetBmp + 8961] = scrCols0[vRamTileValue2 & 1];
                        screenBytes[columnOffsetBmp + 8962] = scrCols0[vRamTileValue3 & 1];
                        screenBytes[columnOffsetBmp + 8963] = scrCols0[vRamTileValue4 & 1];
                        screenBytes[columnOffsetBmp + 8964] = scrCols0[vRamTileValue5 & 1];
                        screenBytes[columnOffsetBmp + 8965] = scrCols0[vRamTileValue6 & 1];
                        screenBytes[columnOffsetBmp + 8966] = scrCols0[vRamTileValue7 & 1];
                        screenBytes[columnOffsetBmp + 8967] = scrCols0[vRamTileValue8 & 1];

                        screenBytes[columnOffsetBmp + 9600] = scrCols0[vRamTileValue1 & 1];
                        screenBytes[columnOffsetBmp + 9601] = scrCols0[vRamTileValue2 & 1];
                        screenBytes[columnOffsetBmp + 9602] = scrCols0[vRamTileValue3 & 1];
                        screenBytes[columnOffsetBmp + 9603] = scrCols0[vRamTileValue4 & 1];
                        screenBytes[columnOffsetBmp + 9604] = scrCols0[vRamTileValue5 & 1];
                        screenBytes[columnOffsetBmp + 9605] = scrCols0[vRamTileValue6 & 1];
                        screenBytes[columnOffsetBmp + 9606] = scrCols0[vRamTileValue7 & 1];
                        screenBytes[columnOffsetBmp + 9607] = scrCols0[vRamTileValue8 & 1];

                        tileScreenIndex++;
                        tileColorIndex++;
                        tileOffsetVram += CharWidth;
                        columnOffsetBmp += 8;
                        if ((tileScreenIndex % 80) == 0)
                            columnOffsetBmp = (tileScreenIndex / 80) * 10240;
                    }
                    break;
            }

            lock (copylock)
            {
                // sprites
                switch (videoMode)
                {
                    case 1:
                        // Bank A Sprites
                        for (int sprnum = 0; sprnum < SPRITES_NUM; sprnum++)
                        {
                            int sparams = ((spriteBuffer & 1) == 1 ? SPRITE_A_PARAMS_2 : SPRITE_A_PARAMS_1) + (sprnum << 3);
                            int sdata = ((spriteBuffer & 2) == 2 ? SPRITE_A_DATA_2 : SPRITE_A_DATA_1) + (sprnum << 7);
                            if (Ram[sparams + SPRITE_ENABLE] == 1)
                                BlitSprite(ref sparams, ref sdata, borderZeroBmpBytesNum);
                        }
                        // Bank B Sprites
                        for (int sprnum = 0; sprnum < SPRITES_NUM; sprnum++)
                        {
                            int sparams = ((spriteBuffer & 1) == 1 ? SPRITE_B_PARAMS_2 : SPRITE_B_PARAMS_1) + (sprnum << 3);
                            int sdata = ((spriteBuffer & 2) == 2 ? SPRITE_B_DATA_2 : SPRITE_B_DATA_1) + (sprnum << 7);
                            if (Ram[sparams + SPRITE_ENABLE] == 1)
                                BlitSprite(ref sparams, ref sdata, borderZeroBmpBytesNum);
                        }
                        // Bank C Sprites
                        for (int sprnum = 0; sprnum < SPRITES_NUM; sprnum++)
                        {
                            int sparams = ((spriteBuffer & 1) == 1 ? SPRITE_C_PARAMS_2 : SPRITE_C_PARAMS_1) + (sprnum << 3);
                            int sdata = ((spriteBuffer & 2) == 2 ? SPRITE_C_DATA_2 : SPRITE_C_DATA_1) + (sprnum << 7);
                            if (Ram[sparams + SPRITE_ENABLE] == 1)
                                BlitSprite(ref sparams, ref sdata, borderZeroBmpBytesNum);
                        }
                        break;

                    default:
                        // Bank A Sprites
                        for (int sprnum = 0; sprnum < SPRITES_NUM; sprnum++)
                        {
                            int sparams = ((spriteBuffer & 1) == 1 ? SPRITE_A_PARAMS_2 : SPRITE_A_PARAMS_1) + (sprnum << 3);
                            int sdata = ((spriteBuffer & 2) == 2 ? SPRITE_A_DATA_2 : SPRITE_A_DATA_1) + (sprnum << 7);
                            if (Ram[sparams + SPRITE_ENABLE] == 1)
                                BlitSprite(ref sparams, ref sdata);
                        }
                        // Bank B Sprites
                        for (int sprnum = 0; sprnum < SPRITES_NUM; sprnum++)
                        {
                            int sparams = ((spriteBuffer & 1) == 1 ? SPRITE_B_PARAMS_2 : SPRITE_B_PARAMS_1) + (sprnum << 3);
                            int sdata = ((spriteBuffer & 2) == 2 ? SPRITE_B_DATA_2 : SPRITE_B_DATA_1) + (sprnum << 7);
                            if (Ram[sparams + SPRITE_ENABLE] == 1)
                                BlitSprite(ref sparams, ref sdata);
                        }
                        // Bank C Sprites
                        for (int sprnum = 0; sprnum < SPRITES_NUM; sprnum++)
                        {
                            int sparams = ((spriteBuffer & 1) == 1 ? SPRITE_C_PARAMS_2 : SPRITE_C_PARAMS_1) + (sprnum << 3);
                            int sdata = ((spriteBuffer & 2) == 2 ? SPRITE_C_DATA_2 : SPRITE_C_DATA_1) + (sprnum << 7);
                            if (Ram[sparams + SPRITE_ENABLE] == 1)
                                BlitSprite(ref sparams, ref sdata);
                        }
                        break;
                }
                BitmapData bmpdata = screenBitMap.LockBits(new Rectangle(0, 0, screenBitMap.Width, screenBitMap.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                Marshal.Copy(screenBytes, 0, bmpdata.Scan0, screenBytes.Length);
                screenBitMap.UnlockBits(bmpdata);
                vblCounter++;
                Memory.Data[Cpu.COUNTER] = (byte)(vblCounter >> 8);
                Memory.Data[Cpu.COUNTER + 1] = (byte)(vblCounter);
                return screenBitMap;
            }
        }

        private static void BlitSprite(ref int spriteParams, ref int spriteData, int modeOffsetBytes = 0)
        {
            int __maxclip = screenBytes.Length - 64 - modeOffsetBytes;
            int x = (Ram[spriteParams++] << 8 | Ram[spriteParams++]) << 1;
            int y = (Ram[spriteParams++] << 8 | Ram[spriteParams]) << 1;
            int srcScreenIndex = modeOffsetBytes + y * 640 + x;
            for (int sprndx = 0; sprndx < 16; sprndx++)
            {
                if (srcScreenIndex >= __maxclip)
                    break;
                sprMaskBytes[00] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprColorBytes[00] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprMaskBytes[02] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprColorBytes[02] = (byte)(Ram[spriteData++] & 0x07);
                sprMaskBytes[04] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprColorBytes[04] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprMaskBytes[06] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprColorBytes[06] = (byte)(Ram[spriteData++] & 0x07);
                sprMaskBytes[08] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprColorBytes[08] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprMaskBytes[10] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprColorBytes[10] = (byte)(Ram[spriteData++] & 0x07);
                sprMaskBytes[12] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprColorBytes[12] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprMaskBytes[14] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprColorBytes[14] = (byte)(Ram[spriteData++] & 0x07);
                sprMaskBytes[16] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprColorBytes[16] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprMaskBytes[18] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprColorBytes[18] = (byte)(Ram[spriteData++] & 0x07);
                sprMaskBytes[20] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprColorBytes[20] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprMaskBytes[22] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprColorBytes[22] = (byte)(Ram[spriteData++] & 0x07);
                sprMaskBytes[24] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprColorBytes[24] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprMaskBytes[26] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprColorBytes[26] = (byte)(Ram[spriteData++] & 0x07);
                sprMaskBytes[28] = sprMaskValsMode1[Ram[spriteData] >> 7];
                sprColorBytes[28] = (byte)((Ram[spriteData] >> 4) & 0x07);
                sprMaskBytes[30] = sprMaskValsMode1[(Ram[spriteData] >> 3) & 0x01];
                sprColorBytes[30] = (byte)(Ram[spriteData++] & 0x07);
                sprMaskBytes[01] = sprMaskBytes[00];
                sprMaskBytes[03] = sprMaskBytes[02];
                sprMaskBytes[05] = sprMaskBytes[04];
                sprMaskBytes[07] = sprMaskBytes[06];
                sprMaskBytes[09] = sprMaskBytes[08];
                sprMaskBytes[11] = sprMaskBytes[10];
                sprMaskBytes[13] = sprMaskBytes[12];
                sprMaskBytes[15] = sprMaskBytes[14];
                sprMaskBytes[17] = sprMaskBytes[16];
                sprMaskBytes[19] = sprMaskBytes[18];
                sprMaskBytes[21] = sprMaskBytes[20];
                sprMaskBytes[23] = sprMaskBytes[22];
                sprMaskBytes[25] = sprMaskBytes[24];
                sprMaskBytes[27] = sprMaskBytes[26];
                sprMaskBytes[29] = sprMaskBytes[28];
                sprMaskBytes[31] = sprMaskBytes[30];
                sprColorBytes[01] = sprColorBytes[00];
                sprColorBytes[03] = sprColorBytes[02];
                sprColorBytes[05] = sprColorBytes[04];
                sprColorBytes[07] = sprColorBytes[06];
                sprColorBytes[09] = sprColorBytes[08];
                sprColorBytes[11] = sprColorBytes[10];
                sprColorBytes[13] = sprColorBytes[12];
                sprColorBytes[15] = sprColorBytes[14];
                sprColorBytes[17] = sprColorBytes[16];
                sprColorBytes[19] = sprColorBytes[18];
                sprColorBytes[21] = sprColorBytes[20];
                sprColorBytes[23] = sprColorBytes[22];
                sprColorBytes[25] = sprColorBytes[24];
                sprColorBytes[27] = sprColorBytes[26];
                sprColorBytes[29] = sprColorBytes[28];
                sprColorBytes[31] = sprColorBytes[30];
                Buffer.BlockCopy(sprMaskBytes, 0, maskBitBlock, 0, 32);
                Buffer.BlockCopy(sprColorBytes, 0, spriteBitBlock, 0, 32);
                Buffer.BlockCopy(screenBytes, srcScreenIndex, screenBitBlock, 0, 32);
                rasterOpBitBlock[0] = screenBitBlock[0] & maskBitBlock[0] | (spriteBitBlock[0] & (maskBitBlock[0] ^ 0x0ffffffffffffffff));
                rasterOpBitBlock[1] = screenBitBlock[1] & maskBitBlock[1] | (spriteBitBlock[1] & (maskBitBlock[1] ^ 0x0ffffffffffffffff));
                rasterOpBitBlock[2] = screenBitBlock[2] & maskBitBlock[2] | (spriteBitBlock[2] & (maskBitBlock[2] ^ 0x0ffffffffffffffff));
                rasterOpBitBlock[3] = screenBitBlock[3] & maskBitBlock[3] | (spriteBitBlock[3] & (maskBitBlock[3] ^ 0x0ffffffffffffffff));
                Buffer.BlockCopy(rasterOpBitBlock, 0, screenBytes, srcScreenIndex, 32);
                srcScreenIndex += 640;
                Buffer.BlockCopy(rasterOpBitBlock, 0, screenBytes, srcScreenIndex, 32);
                srcScreenIndex += 640;
            }
        }

        public static object copylock = new object();
    }
}
