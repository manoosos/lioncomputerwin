using System.Windows.Input;
using System.Collections.Generic;

namespace LionComputerEmulator
{
    public static class Keyboard
    {
        public static void ScanKeysForJoystick()
        {
            int joyValue = 0;

            if (System.Windows.Input.Keyboard.IsKeyDown(Key.Up))
                joyValue |= Device.JoyValueUP;

            if (System.Windows.Input.Keyboard.IsKeyDown(Key.Down))
                joyValue |= Device.JoyValueDOWN;

            if (System.Windows.Input.Keyboard.IsKeyDown(Key.Left))
                joyValue |= Device.JoyValueLEFT;

            if (System.Windows.Input.Keyboard.IsKeyDown(Key.Right))
                joyValue |= Device.JoyValueRIGHT;

            if (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftCtrl))
                joyValue |= Device.JoyValueBUTTON;

            Device.Port[Device.JOYSTICK] = (ushort)(joyValue ^ 0x0ffff);
        }

        private static Dictionary<string, byte> KeysToAsciiDict = new Dictionary<string, byte>()
        {
            {"Back" ,8},                     //
            {"Tab" ,9},                      //
            {"Return" ,13},                  //
            {"\r" ,13},                      //
            {"Escape" ,27},                  //
            {"Space" ,32},                   //
            {" " ,32},                       //
            {"D1, Shift" ,33},               // !
            {"!" ,33},                       // !
            {"Oem7, Shift" ,34},             // "
            {"\"" ,34},                      // "
            {"D3, Shift" ,35},               // #
            {"#" ,35},                       // #
            {"D4, Shift" ,36},               // $
            {"$" ,36},                       // $
            {"D5, Shift" ,37},               // %
            {"%" ,37},                       // %
            {"D7, Shift" ,38},               // &
            {"&" ,38},                       // &
            {"Oem7" ,39},                    // '
            {"'" ,39},                       // '
            {"D9, Shift" ,40},               // (
            {"(" ,40},                       // (
            {"D0, Shift" ,41},               // )
            {")" ,41},                       // )
            {"D8, Shift" ,42},               // *
            {"Multiply" ,42},                // *
            {"*" ,42},                       // *
            {"Oemplus, Shift" ,43},          // +
            {"Add" ,43},                     // +
            {"+" ,43},                       // +
            {"Oemcomma" ,44},                // ,
            {"," ,44},                       // ,
            {"OemMinus" ,45},                // -
            {"Subtract" ,45},                // -
            {"-" ,45},                       // -
            {"OemPeriod" ,46},               // .
            {"Decimal" ,46},                 // .
            {"." ,46},                       // .
            {"OemQuestion" ,47},             // /
            {"Divide" ,47},                  // /
            {"/" ,47},                       // /
            {"0" ,48},                      // 0
            {"1" ,49},                      // 1
            {"2" ,50},                      // 2
            {"3" ,51},                      // 3
            {"4" ,52},                      // 4
            {"5" ,53},                      // 5
            {"6" ,54},                      // 6
            {"7" ,55},                      // 7
            {"8" ,56},                      // 8
            {"9" ,57},                      // 9
            {"D0" ,48},                      // 0
            {"D1" ,49},                      // 1
            {"D2" ,50},                      // 2
            {"D3" ,51},                      // 3
            {"D4" ,52},                      // 4
            {"D5" ,53},                      // 5
            {"D6" ,54},                      // 6
            {"D7" ,55},                      // 7
            {"D8" ,56},                      // 8
            {"D9" ,57},                      // 9
            {"NumPad0" ,48},                 // 0
            {"NumPad1" ,49},                 // 1
            {"NumPad2" ,50},                 // 2
            {"NumPad3" ,51},                 // 3
            {"NumPad4" ,52},                 // 4
            {"NumPad5" ,53},                 // 5
            {"NumPad6" ,54},                 // 6
            {"NumPad7" ,55},                 // 7
            {"NumPad8" ,56},                 // 8
            {"NumPad9" ,57},                 // 9
            {"Oem1, Shift" ,58},             // :
            {":" ,58},                       // :
            {"Oem1" ,59},                    // ;
            {";" ,59},                       // ;
            {"Oemcomma, Shift" ,60},         // <
            {"<" ,60},                       // <
            {"Oemplus" ,61},                 // =
            {"=" ,61},                       // =
            {"OemPeriod, Shift" ,62},        // >
            {">" ,62},                       // >
            {"OemQuestion, Shift" ,63},      // ?
            {"?" ,63},                       // ?
            {"D2, Shift" ,64},               // @
            {"@" ,64},                       // @
            {"A, Shift" ,65},                // A
            {"B, Shift" ,66},                // B
            {"C, Shift" ,67},                // C
            {"D, Shift" ,68},                // D
            {"E, Shift" ,69},                // E
            {"F, Shift" ,70},                // F
            {"G, Shift" ,71},                // G
            {"H, Shift" ,72},                // H
            {"I, Shift" ,73},                // I
            {"J, Shift" ,74},                // J
            {"K, Shift" ,75},                // K
            {"L, Shift" ,76},                // L
            {"M, Shift" ,77},                // M
            {"N, Shift" ,78},                // N
            {"O, Shift" ,79},                // O
            {"P, Shift" ,80},                // P
            {"Q, Shift" ,81},                // Q
            {"R, Shift" ,82},                // R
            {"S, Shift" ,83},                // S
            {"T, Shift" ,84},                // T
            {"U, Shift" ,85},                // U
            {"V, Shift" ,86},                // V
            {"W, Shift" ,87},                // W
            {"X, Shift" ,88},                // X
            {"Y, Shift" ,89},                // Y
            {"Z, Shift" ,90},                // Z
            {"OemOpenBrackets" ,91},         // [
            {"[" ,91},                       // [
            {"Oem5" ,92},                    // \
            {"\\" ,92},                      // \
            {"Oem6" ,93},                    // ]
            {"]" ,93},                       // ]
            {"D6, Shift" ,94},               // ^
            {"^" ,94},                       // ^
            {"OemMinus, Shift" ,95},         // _
            {"_" ,95},                       // _
            {"Oemtilde" ,96},                // `
            {"`" ,96},                       // `
            {"A" ,97},                       // a
            {"B" ,98},                       // b
            {"C" ,99},                       // c
            {"D" ,100},                      // d
            {"E" ,101},                      // e
            {"F" ,102},                      // f
            {"G" ,103},                      // g
            {"H" ,104},                      // h
            {"I" ,105},                      // i
            {"J" ,106},                      // j
            {"K" ,107},                      // k
            {"L" ,108},                      // l
            {"M" ,109},                      // m
            {"N" ,110},                      // n
            {"O" ,111},                      // o
            {"P" ,112},                      // p
            {"Q" ,113},                      // q
            {"R" ,114},                      // r
            {"S" ,115},                      // s
            {"T" ,116},                      // t
            {"U" ,117},                      // u
            {"V" ,118},                      // v
            {"W" ,119},                      // w
            {"X" ,120},                      // x
            {"Y" ,121},                      // y
            {"Z" ,122},                      // z
            {"OemOpenBrackets, Shift" ,123}, // {
            {"{" ,123},                      // {
            {"Oem5, Shift" ,124},            // |
            {"|" ,124},                      // |
            {"Oem6, Shift" ,125},            // }
            {"}" ,125},                      // }
            {"Oemtilde, Shift" ,126},        // ~
            {"~" ,126},                      // ~
            {"Delete" ,127},                 //
        };

        public static void SendKeysToSerial(string keyData)
        {
            if (KeysToAsciiDict.ContainsKey(keyData))
            {
                Device.Port[Device.SERIAL_BYTE_RECEIVED] = KeysToAsciiDict[keyData];
                Device.Port[Device.SERIAL_SKBD_STATUS] |= 2;
            }
        }
    }
}
