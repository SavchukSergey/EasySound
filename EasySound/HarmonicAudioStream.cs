using System;

namespace EasySound {
    /// <summary>
    /// Represents audio stream of harmonic oscillations.
    /// </summary>
    public class HarmonicAudioStream : AudioStream {
        private readonly double _frequency;
        private readonly double _amplitude;
        private readonly ushort _bitsPerSample;
        private readonly ushort _channelsCount;
        private readonly double _coeff;

        /// <summary>
        /// Creates an instance of HarmonicAudioStream.
        /// </summary>
        /// <param name="frequency">Frequency of oscillations</param>
        /// <param name="amplitude">Ampliture of oscillations. (1.0 is Max amplitude).</param>
        /// <param name="length">Length in seconds of oscillations.</param>
        /// <param name="sampleRate">Sample rate (44100 Hz, 22050 Hz, etc)/</param>
        /// <param name="bitsPerSample">Bits per sample/</param>
        /// <param name="channelsCount">Channels count.</param>
        public HarmonicAudioStream(double frequency, double amplitude, double length, uint sampleRate, ushort bitsPerSample, ushort channelsCount) {
            _frequency = frequency;
            _amplitude = amplitude;
            _bitsPerSample = bitsPerSample;
            _channelsCount = channelsCount;
            _coeff = 2 * Math.PI * _frequency;
            SampleRate = sampleRate;
            SamplesCount = (uint)(length * sampleRate);
        }



        /// <summary>
        /// Creates an instance of HarmonicAudioStream.
        /// </summary>
        /// <param name="frequency">Frequency of oscillations</param>
        /// <param name="amplitude">Ampliture of oscillations. (1.0 is Max amplitude).</param>
        /// <param name="length">Length in seconds of oscillations.</param>
        public HarmonicAudioStream(double frequency, double amplitude, double length)
            : this(frequency, amplitude, length, 44100, 16, 2) {
        }

        /// <summary>
        /// Gets count of bits per sample. (16 or 8)
        /// </summary>
        public override ushort BitsPerSample {
            get { return _bitsPerSample; }
        }

        /// <summary>
        /// Gets count of channels. (1 for mono, 2 for stereo)
        /// </summary>
        public override ushort ChannelsCount {
            get { return _channelsCount; }
        }

        /// <summary>
        /// Reads next available sample.
        /// </summary>
        /// <param name="sample">Sample to be filled with data.</param>
        public override void ReadSample(out Sample sample) {
            if (_position >= Length) {
                ReadNullSample(out sample);
                return;
            }
            var angle = _position * _coeff;
            var res = _amplitude * Math.Sin(angle);
            short value = (short)(res * 32767);
            sample.Time = _position;
            sample.Left = value;
            sample.Right = value;
            _position += SampleDuration;
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose() {
        }
    }
}