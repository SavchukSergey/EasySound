using System.Collections.Generic;

namespace EasySound.Tests {
    public class StubAudioStream : AudioStream {
        private readonly ushort _bitsPerSample;
        private readonly ushort _channelsCount;

        private readonly IList<Sample> _samples = new List<Sample>();

        private int _samplePosition;

        public StubAudioStream(uint sampleRate, ushort bitsPerSample, ushort channelsCount) {
            SampleRate = sampleRate;
            _bitsPerSample = bitsPerSample;
            _channelsCount = channelsCount;
        }

        public override ushort BitsPerSample {
            get { return _bitsPerSample; }
        }

        public override ushort ChannelsCount {
            get { return _channelsCount; }
        }

        public void AddSample(Sample sample) {
            _samples.Add(sample);
            SamplesCount = (uint)_samples.Count;
        }

        public void AddSample(short left, short right) {
            AddSample(new Sample { Left = left, Right = right, Time = (double)_samples.Count / SampleRate });
        }


        public override void ReadSample(out Sample sample) {
            if (_samplePosition < _samples.Count) {
                sample = _samples[_samplePosition++];
                _position += SampleDuration;
            } else {
                ReadNullSample(out sample);
            }
        }

        public override void Dispose() {

        }
    }
}