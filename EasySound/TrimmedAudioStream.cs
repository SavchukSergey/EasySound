using System;

namespace EasySound {
    /// <summary>
    /// Represents audio stream which is trimmed at the start and at the end.
    /// </summary>
    public class TrimmedAudioStream : WrappedAudioStreamBase {

        private readonly double _start;
        private readonly double _end;

        /// <summary>
        /// Creates an instance of TrimmedAudioStream
        /// </summary>
        /// <param name="innerStream">Stream to be processed.</param>
        /// <param name="start">Time in seconds where to start playing.</param>
        /// <param name="end">Time in seconds where to finish playing.</param>
        public TrimmedAudioStream(AudioStream innerStream, double start, double end)
            : base(innerStream) {
            _start = start;
            _end = end;
            SamplesCount = (uint)Math.Ceiling((end - start) * _innerStream.SampleRate) + 1u;
            SampleRate = _innerStream.SampleRate;
        }

        /// <summary>
        /// Reads next available sample.
        /// </summary>
        /// <param name="sample">Sample to be filled with data.</param>
        public override void ReadSample(out Sample sample) {
            while (_innerStream.Position < _start) {
                _innerStream.ReadSample(out sample);
            }
            if (_innerStream.Position > _end) {
                ReadNullSample(out sample);
                return;
            }
            _innerStream.ReadSample(out sample);
            sample.Time = Position;
            _position += SampleDuration;
        }
    }
}