namespace EasySound {
    /// <summary>
    /// Represents volume adjustment audio stream. All samples will be multiplied by volume factor.
    /// </summary>
    public class VolumeAdjustmentAudioStream : WrappedAudioStreamBase {
        private double _volumeFactor = 1.0;

        /// <summary>
        /// Volume factor.
        /// </summary>
        public double VolumeFactor {
            get { return _volumeFactor; }
            set { _volumeFactor = value; }
        }

        /// <summary>
        /// Creates an instance of VolumeAdjustmentAudioStream.
        /// </summary>
        /// <param name="innerStream">Stream to be processed.</param>
        /// <param name="volumeAdjustment">Volume factor.</param>
        public VolumeAdjustmentAudioStream(AudioStream innerStream, double volumeAdjustment)
            : base(innerStream) {
            _volumeFactor = volumeAdjustment;
            SamplesCount = _innerStream.SamplesCount;
            SampleRate = _innerStream.SampleRate;
        }

        /// <summary>
        /// Reads next available sample.
        /// </summary>
        /// <param name="sample">Sample to be filled with data.</param>
        public override void ReadSample(out Sample sample) {
            _innerStream.ReadSample(out sample);
            double left = sample.Left * _volumeFactor;
            double right = sample.Right * _volumeFactor;
            if (left < short.MinValue) left = short.MinValue;
            if (left > short.MaxValue) left = short.MaxValue;
            if (right < short.MinValue) right = short.MinValue;
            if (right > short.MaxValue) right = short.MaxValue;
            sample.Left = (short)left;
            sample.Right = (short)right;
        }
    }
}