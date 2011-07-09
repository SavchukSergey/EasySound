namespace EasySound {
    /// <summary>
    /// Represents one atomic sample of sound.
    /// </summary>
    public struct Sample {

        /// <summary>
        /// Amplitude for left channel.
        /// </summary>
        public short Left;

        /// <summary>
        /// Amplitude for right channel.
        /// </summary>
        public short Right;

        /// <summary>
        /// Time of the sample in seconds.
        /// </summary>
        public double Time;

        /// <summary>
        /// Merges all channels into one 8-bit amplitude.
        /// </summary>
        /// <returns></returns>
        public byte GetMono8() {
            var val1 = Left + 32768;
            var val2 = Right + 32768;
            uint val = (uint)(val1 + val2) >> 1;
            return (byte)(val >> 8);
        }

        /// <summary>
        /// Merges all channels into 16-bit amplitude.
        /// </summary>
        /// <returns></returns>
        public short GetMono16() {
            return (short)((Left + Right) / 2);
        }

        /// <summary>
        /// Converts 16-bit value to 8-bit value.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static byte ToBits8(short val) {
            return (byte)(((uint)(val + 32768)) >> 8);
        }

        /// <summary>
        /// Converts 8-bit value to 16-bit value.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static short ToBits16(byte val) {
            return (short)(val * 257 - 32768);
        }

    }
}