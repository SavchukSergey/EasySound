namespace EasySound {
    /// <summary>
    /// Represents wrapper under audio stream which delays audio stream before playing it.
    /// </summary>
    public class DelayedAudioStream : WrappedAudioStreamBase {
        private readonly double _delaySeconds;

        /// <summary>
        /// Creates an instance of DelayedAudioStream.
        /// </summary>
        /// <param name="innerStream">Underlying audio stream.</param>
        /// <param name="delaySeconds">Delay in seconds.</param>
        public DelayedAudioStream(AudioStream innerStream, double delaySeconds)
            : base(innerStream) {
            _delaySeconds = delaySeconds;
            SamplesCount = (uint)(delaySeconds * _innerStream.SampleRate + _innerStream.SamplesCount);
            SampleRate = _innerStream.SampleRate;
        }

        /// <summary>
        /// Reads next available sample.
        /// </summary>
        /// <param name="sample">Sample to be filled with data.</param>
        public override void ReadSample(out Sample sample) {
            if (_position < _delaySeconds) {
                ReadNullSample(out sample);
            } else {
                _innerStream.ReadSample(out sample);
                sample.Time += _delaySeconds;
                _position += SampleDuration;
            }
        }
    }
}