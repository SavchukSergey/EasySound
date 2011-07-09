using NUnit.Framework;

namespace EasySound.Tests {
    public abstract class CompositeAudioStreamTestBase : AudioStreamTestBase {

        protected static StubAudioStream GetTestStream1(uint sampleRate, ushort bitsPerSample, ushort channelsCount) {
            var stream = new StubAudioStream(sampleRate, bitsPerSample, channelsCount);
            stream.AddSample(1200, -5000);
            stream.AddSample(-17600, 4200);
            stream.AddSample(17600, 3200);
            return stream;
        }

        protected static StubAudioStream GetTestStream2(uint sampleRate, ushort bitsPerSample, ushort channelsCount) {
            var stream = new StubAudioStream(sampleRate, bitsPerSample, channelsCount);
            stream.AddSample(7895, -5000);
            stream.AddSample(4765, 12345);
            return stream;
        }

        public virtual void ChannelsCountTest() {
            var stream1 = GetTestStream1(SAMPLE_RATE_44, 16, 1);
            var stream2 = GetTestStream2(SAMPLE_RATE_22, 16, 2);

            var stream = JoinStreams(stream1, stream2);
            Assert.AreEqual(2u, stream.ChannelsCount);
            stream = JoinStreams(stream2, stream1);
            Assert.AreEqual(2u, stream.ChannelsCount);

            stream2 = GetTestStream2(SAMPLE_RATE_22, 8, 1);
            stream = JoinStreams(stream1, stream2);
            Assert.AreEqual(1u, stream.ChannelsCount);
        }

        public virtual void BitsPerSampleTest() {
            var stream1 = GetTestStream1(SAMPLE_RATE_44, 8, 1);
            var stream2 = GetTestStream2(SAMPLE_RATE_22, 16, 2);

            var stream = JoinStreams(stream1, stream2);
            Assert.AreEqual(16u, stream.BitsPerSample);
            stream = JoinStreams(stream2, stream1);
            Assert.AreEqual(16u, stream.BitsPerSample);

            stream2 = GetTestStream2(SAMPLE_RATE_22, 8, 2);
            stream = JoinStreams(stream1, stream2);
            Assert.AreEqual(8u, stream.BitsPerSample);
        }

        protected abstract AudioStream JoinStreams(AudioStream first, AudioStream second);

    }
}