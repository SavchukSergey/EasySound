using System;

namespace EasySound.Filters {
    public abstract class BaseRCFilterAudioStream : WrappedAudioStreamBase {

        protected readonly double _resistance;
        protected readonly double _capacity;
        protected readonly double _dt;
        protected double _ucLeft;
        protected double _ucRight;
        protected double _freq;

        protected BaseRCFilterAudioStream(AudioStream innerStream, double freq)
            : base(innerStream) {
            _freq = freq;
            _dt = innerStream.SampleDuration;

            SamplesCount = innerStream.SamplesCount;
            SampleRate = innerStream.SampleRate;

            _resistance = 1000;
            var filterFreqW = freq * 2 * Math.PI;
            var timeConstant = 1 / filterFreqW;
            _capacity = timeConstant / _resistance;
        }

        public double Freq { get { return _freq; } }
    }
}
