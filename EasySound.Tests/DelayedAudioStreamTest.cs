using NUnit.Framework;

namespace EasySound.Tests {
    [TestFixture]
    public class DelayedAudioStreamTest : AudioStreamTestBase {

        private static DelayedAudioStream GetStream(int delaySampleCount) {
            return GetStream(delaySampleCount, 16, 2);
        }

        private static DelayedAudioStream GetStream(int delaySampleCount, ushort bitsPerSample, ushort numChannels) {
            var innerStream = new StubAudioStream(SAMPLE_RATE_44, bitsPerSample, numChannels);
            innerStream.AddSample(1200, -5000);
            innerStream.AddSample(-17600, 4200);
            innerStream.AddSample(17600, 3200);

            return new DelayedAudioStream(innerStream, delaySampleCount * SAMPLE_DURATION_44);
        }

        [Test]
        public void SampleRateTest() {
            var stream = GetStream(5);
            Assert.AreEqual(SAMPLE_RATE_44, stream.SampleRate);
        }

        [Test]
        public void LengthTest() {
            var stream = GetStream(5);
            Assert.AreEqual((5 + 3) * SAMPLE_DURATION_44, stream.Length);
        }

        [Test]
        public void SampleCountTest() {
            var stream = GetStream(5);
            Assert.AreEqual((uint)(5 + 3), stream.SamplesCount);
        }

        [Test]
        public void BitsPerSampleTest() {
            var stream = GetStream(5, 16, 2);
            Assert.AreEqual(16, stream.BitsPerSample);
            stream = GetStream(5, 8, 2);
            Assert.AreEqual(8, stream.BitsPerSample);
        }

        [Test]
        public void ChannelsCountTest() {
            var stream1 = GetStream(5, 16, 1);
            Assert.AreEqual(1u, stream1.ChannelsCount);
            var stream2 = GetStream(5, 16, 2);
            Assert.AreEqual(2u, stream2.ChannelsCount);
        }

        [Test]
        public void GetSampleTest() {
            var stream = GetStream(5);
            AssertSample(stream, SAMPLE_DURATION_44 * 0, 0, 0);
            AssertSample(stream, SAMPLE_DURATION_44 * 1, 0, 0);
            AssertSample(stream, SAMPLE_DURATION_44 * 2, 0, 0);
            AssertSample(stream, SAMPLE_DURATION_44 * 3, 0, 0);
            AssertSample(stream, SAMPLE_DURATION_44 * 4, 0, 0);
            AssertSample(stream, SAMPLE_DURATION_44 * 5, 1200, -5000);
            AssertSample(stream, SAMPLE_DURATION_44 * 6, -17600, 4200);
            AssertSample(stream, SAMPLE_DURATION_44 * 7, 17600, 3200);
            AssertSample(stream, SAMPLE_DURATION_44 * 8, 0, 0);
        }

    }
}