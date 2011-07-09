namespace EasySound {
    /// <summary>
    /// Represents audio stream which adjust bits per sample to specific value. It doesn't round values.
    /// </summary>
    public class SampleBitsAdjustmentStream : WrappedAudioStreamBase {
        private readonly ushort _targetSampleBits;

        ///<summary>
        /// Creates an instanse of SampleBitsAdjustmentStream.
        ///</summary>
        ///<param name="innerStream"></param>
        ///<param name="targetSampleBits"></param>
        public SampleBitsAdjustmentStream(AudioStream innerStream, ushort targetSampleBits)
            : base(innerStream) {
            _targetSampleBits = targetSampleBits;
            SamplesCount = _innerStream.SamplesCount;
            SampleRate = _innerStream.SampleRate;
        }

        /// <summary>
        /// Gets count of bits per sample. (16 or 8)
        /// </summary>
        public override ushort BitsPerSample { get { return _targetSampleBits; } }

        /// <summary>
        /// Reads next available sample.
        /// </summary>
        /// <param name="sample">Sample to be filled with data.</param>
        public override void ReadSample(out Sample sample) {
            _innerStream.ReadSample(out sample);
        }
    }
}