using System;

namespace EasySound {
    /// <summary>
    /// Represents audio stream which adjust sample rate to specific value.
    /// </summary>
    public class SampleRateAdjustmentAudioStream : WrappedAudioStreamBase {
        private bool _hasCurrentSample;
        private bool _hasNextSample;
        private Sample _currentSample;
        private Sample _nextSample;

        private readonly ReadSampleHandler _handler;

        /// <summary>
        /// Creates an instance of SampleRateAdjustmentAudioStream.
        /// </summary>
        /// <param name="innerStream">Stream to be processed.</param>
        /// <param name="sampleRate">Sample rate to be adjusted to.</param>
        public SampleRateAdjustmentAudioStream(AudioStream innerStream, uint sampleRate)
            : base(innerStream) {
            SampleRate = sampleRate;
            SamplesCount = (uint)(innerStream.Length * sampleRate);
            if (innerStream.SampleRate == sampleRate) {
                _handler = EqualRate;
            } else if (innerStream.SampleRate < sampleRate) {
                _handler = IncreaseRate;
            } else {
                _handler = DecreaseRate;
            }
        }

        private void EqualRate(out Sample sample) {
            _innerStream.ReadSample(out sample);
        }

        private void IncreaseRate(out Sample sample) {
            if (!_hasCurrentSample) {
                _innerStream.ReadSample(out _currentSample);
                _hasCurrentSample = true;
            }
            if (!_hasNextSample) {
                _innerStream.ReadSample(out _nextSample);
                _hasNextSample = true;
            }

            if (_nextSample.Time < _position) {
                _currentSample = _nextSample;
                _innerStream.ReadSample(out _nextSample);
            }

            var sampleDuration = _innerStream.SampleDuration;

            double relTime = (_position - _currentSample.Time) / sampleDuration;
            double dLeft = _nextSample.Left - _currentSample.Left;
            double dRight = _nextSample.Right - _currentSample.Right;
            double left = _currentSample.Left + dLeft * relTime;
            double right = _currentSample.Right + dRight * relTime;

            sample.Time = _position;
            sample.Left = (short)left;
            sample.Right = (short)right;
            _position += SampleDuration;
        }

        private void DecreaseRate(out Sample sample) {
            //TODO: decrease rate if needed
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads next available sample.
        /// </summary>
        /// <param name="sample">Sample to be filled with data.</param>
        public override void ReadSample(out Sample sample) {
            _handler(out sample);
        }
    }
}