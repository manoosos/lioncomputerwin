using System;

namespace LionComputerEmulator
{
    public static class Device
    {
        /// <summary>
        /// Port Numbers Array
        /// </summary>
        private static PortNumber _port = new PortNumber();
        private static DateTime timerTime = DateTime.Now;

        #region public members

        public const int MAX_DEVICE_PORTS = 0x0100;

        // serial send-receive on dte side namings
        public const ushort SERIAL_BYTE_TO_SEND = 0;
        public const ushort SERIAL_COMMAND = 2;
        public const ushort SERIAL_BYTE_RECEIVED = 4;
        public const ushort SERIAL_SKBD_STATUS = 6;
        public const ushort SOUND_CONTROL = 8;
        public const ushort SOUND_STATUS = 9;
        public const ushort SOUND_CONTROL_2 = 10;
        public const ushort SOUND_NOISE_FLAG = 11;
        public const ushort SKBD_BYTE_RECEIVED = 14;
        public const ushort SKBD_READ_COMMAND = 15;
        public const ushort SPI_DATA_READ = 16;
        public const ushort SPI_STATUS = 17;
        public const ushort SPI_DATA_TO_SEND = 18;
        public const ushort SPI_CONTROL = 19;
        public const ushort TIMER_SPRITES = 20;
        public const ushort JOYSTICK = 22; // joy1: low byte, joy2: high byte
        public const ushort VIDEO_MODE = 24;

        // useful values
        public const ushort JoyValueRIGHT = 0x010;
        public const ushort JoyValueLEFT = 8;
        public const ushort JoyValueUP = 4;
        public const ushort JoyValueDOWN = 2;
        public const ushort JoyValueBUTTON = 1;

        /// <summary>
        /// Device Ports Indexer
        /// </summary>
        public class PortNumber
        {
            private ushort[] port = new ushort[MAX_DEVICE_PORTS];

            public PortNumber()
            {
                // init joysticks - pins pulled up
                port[JOYSTICK] = 0x0ffff;
            }

            public ushort this[int number]
            {
                get
                {
                    if (number < 0 || number >= MAX_DEVICE_PORTS)
                        return 0x0ffff;

                    if (number == TIMER_SPRITES)
                        return (ushort)(DateTime.Now - timerTime).TotalMilliseconds;

                    return port[number];
                }

                set
                {
                    if (number >= 0 && number < MAX_DEVICE_PORTS)
                    {
                        port[number] = value;

                        switch (number)
                        {
                            case SERIAL_COMMAND:
                                if ((value & 0x02) != 0)
                                {
                                    port[SERIAL_BYTE_RECEIVED] = 0;
                                    port[SERIAL_SKBD_STATUS] &= 0x0fffd;
                                }
                                break;

                            case SOUND_CONTROL:
                                Device.Port[Device.SOUND_STATUS] |= 1; // channel start
                                while (Sound.WorkerChannel1.IsBusy) ;
                                Sound.WorkerChannel1.RunWorkerAsync();
                                break;

                            case SOUND_CONTROL_2:
                                Device.Port[Device.SOUND_STATUS] |= 2; // channel start
                                while (Sound.WorkerChannel2.IsBusy) ;
                                Sound.WorkerChannel2.RunWorkerAsync();
                                break;

                            case TIMER_SPRITES:
                                // pass sprite parameter-data buffer
                                while (Display.SpritePortWorker.IsBusy) ;
                                Display.SpritePortWorker.RunWorkerAsync(value);
                                break;

                            case VIDEO_MODE:
                                // pass videomode number 0 - 1
                                while (Display.VideoModePortWorker.IsBusy) ;
                                Display.VideoModePortWorker.RunWorkerAsync(value);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Port Number accessor
        /// </summary>
        public static PortNumber Port = _port;

        #endregion
    }
}
