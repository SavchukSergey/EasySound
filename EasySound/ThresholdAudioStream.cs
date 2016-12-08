namespace EasySound {
    public class ThresholdAudioStream : AudioStream {

        private readonly AudioStream _stream;
        private readonly short _max;
        private readonly short _min;

        public ThresholdAudioStream(AudioStream stream) {
            _stream = stream;
            _max = short.MaxValue - 10;
            _min = short.MinValue + 10;
            SamplesCount = stream.SamplesCount;
            SampleRate = stream.SampleRate;
        }

        public override ushort BitsPerSample => _stream.BitsPerSample;

        public override ushort ChannelsCount => _stream.ChannelsCount;

        public override void ReadSample(out Sample sample) {
            _stream.ReadSample(out sample);
            sample.Left = sample.Left > 0 ? _max : _min;
            sample.Right = sample.Right > 0 ? _max : _min;
        }

        public override void Dispose() {
            _stream.Dispose();
        }
    }
}
