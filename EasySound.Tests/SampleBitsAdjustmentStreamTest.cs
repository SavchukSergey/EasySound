using NUnit.Framework;

namespace EasySound.Tests {
    [TestFixture]
    public class SampleBitsAdjustmentStreamTest : AudioStreamTestBase {

        private static SampleBitsAdjustmentStream GetTestStream(ushort originalBits, ushort targetBits, ushort channelsCount) {
            var innerStream = new StubAudioStream(SAMPLE_RATE_44, originalBits, channelsCount);
            innerStream.AddSample(1200, -5000);
            innerStream.AddSample(-11600, 4200);
            innerStream.AddSample(11600, 3200);

            return new SampleBitsAdjustmentStream(innerStream, targetBits);
        }

        [Test]
        public void SampleRateTest() {
            var stream = GetTestStream(16, 8, 2);
            Assert.AreEqual(SAMPLE_RATE_44, stream.SampleRate);
        }

        [Test]
        public void LengthTest() {
            var stream = GetTestStream(16, 8, 2);
            Assert.AreEqual(3 * SAMPLE_DURATION_44, stream.Length);
        }

        [Test]
        public void SampleCountTest() {
            var stream = GetTestStream(16, 8, 2);
            Assert.AreEqual(3u, stream.SamplesCount);
        }

        [Test]
        public void BitsPerSampleTest() {
            var stream = GetTestStream(16, 8, 2);
            Assert.AreEqual(8, stream.BitsPerSample);

            stream = GetTestStream(16, 16, 2);
            Assert.AreEqual(16, stream.BitsPerSample);
        }

        [Test]
        public void ChannelsCountTest() {
            var stream1 = GetTestStream(16, 8, 1);
            Assert.AreEqual(1u, stream1.ChannelsCount);
            var stream2 = GetTestStream(16, 8, 2);
            Assert.AreEqual(2u, stream2.ChannelsCount);
        }

        [Test]
        public void GetSampleTest() {
            var stream = GetTestStream(16, 8, 2);
            AssertSample(stream, SAMPLE_DURATION_44 * 0, 1200, -5000);
            AssertSample(stream, SAMPLE_DURATION_44 * 1, -11600, 4200);
            AssertSample(stream, SAMPLE_DURATION_44 * 2, 11600, 3200);
            AssertSample(stream, SAMPLE_DURATION_44 * 3, 0, 0);
        }

    }
}