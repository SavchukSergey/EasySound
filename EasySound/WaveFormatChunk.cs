namespace EasySound {
    /// <summary>
    /// Represents the "fmt " subchunk which describes the sound data's format.
    /// </summary>
    public struct WaveFormatChunk {

        /// <summary>
        /// PCM = 1 (i.e. Linear quantization) Values other than 1 indicate some form of compression.
        /// </summary>
        public ushort AudioFormat;

        /// <summary>
        /// Count of channels. (1 for mono, 2 for stereo)
        /// </summary>
        public ushort NumChannels;

        /// <summary>
        /// Gets sample rate. (44100 Hz, 22050 Hz, etc)
        /// </summary>
        public uint SampleRate;

        /// <summary>
        /// SampleRate * NumChannels * BitsPerSample/8
        /// </summary>
        public uint ByteRate;

        /// <summary>
        /// NumChannels * BitsPerSample/8 The number of bytes for one sample including all channels. I wonder what happens when this number isn't an integer?
        /// </summary>
        public ushort BlockAlign;


        /// <summary>
        /// Gets count of bits per sample. (16 or 8)
        /// </summary>
        public ushort BitsPerSample;

    }
}