namespace EasySound {
    /// <summary>
    /// Represents base class for wrapped audio streams.
    /// </summary>
    public abstract class WrappedAudioStreamBase : AudioStream {
        /// <summary>
        /// Underlying audio stream.
        /// </summary>
        protected readonly AudioStream _innerStream;

        /// <summary>
        /// Initializes an instanse of WrappedAudioStreamBase
        /// </summary>
        /// <param name="innerStream"></param>
        protected WrappedAudioStreamBase(AudioStream innerStream) {
            _innerStream = innerStream;
        }

        /// <summary>
        /// Gets count of bits per sample. (16 or 8)
        /// </summary>
        public override ushort BitsPerSample {
            get { return _innerStream.BitsPerSample; }
        }

        /// <summary>
        /// Gets count of channels. (1 for mono, 2 for stereo)
        /// </summary>
        public override ushort ChannelsCount {
            get { return _innerStream.ChannelsCount; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose() {
            _innerStream.Dispose();
        }

    }
}