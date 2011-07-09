using NUnit.Framework;

namespace EasySound.Tests {
    [TestFixture]
    public class SampleRateAdjustmentAudioStreamTest : AudioStreamTestBase {
        private static SampleRateAdjustmentAudioStream GetTestStream(uint originalRate, uint targetRate, ushort bitsPerSample, ushort channelsCount) {
            var innerStream = new StubAudioStream(originalRate, bitsPerSample, channelsCount);
            innerStream.AddSample(1200, -5000);
            innerStream.AddSample(-17600, 4200);
            innerStream.AddSample(17600, 3200);
            return new SampleRateAdjustmentAudioStream(innerStream, targetRate);
        }

        [Test]
        public void SampleRateTest() {
            var stream = GetTestStream(SAMPLE_RATE_22, SAMPLE_RATE_44, 16, 2);
            Assert.AreEqual(SAMPLE_RATE_44, stream.SampleRate);
        }

        [Test]
        public void LengthTest() {
            var stream = GetTestStream(SAMPLE_RATE_22, SAMPLE_RATE_44, 16, 2);
            Assert.AreEqual(6 * SAMPLE_DURATION_44, stream.Length);
        }

        [Test]
        public void SampleCountTest() {
            var stream = GetTestStream(SAMPLE_RATE_22, SAMPLE_RATE_44, 16, 2);
            Assert.AreEqual(6u, stream.SamplesCount);
        }

        [Test]
        public void BitsPerSampleTest() {
            var stream = GetTestStream(16, 8, 8, 2);
            Assert.AreEqual(8, stream.BitsPerSample);
            stream = GetTestStream(16, 8, 16, 2);
            Assert.AreEqual(16, stream.BitsPerSample);
        }

        [Test]
        public void ChannelsCountTest() {
            var stream1 = GetTestStream(SAMPLE_RATE_22, SAMPLE_RATE_44, 16, 1);
            Assert.AreEqual(1u, stream1.ChannelsCount);
            var stream2 = GetTestStream(SAMPLE_RATE_22, SAMPLE_RATE_44, 16, 2);
            Assert.AreEqual(2u, stream2.ChannelsCount);
        }

        [Test]
        public void GetSampleTest() {
            var stream = GetTestStream(SAMPLE_RATE_22, SAMPLE_RATE_44, 16, 2);
            AssertSample(stream, SAMPLE_DURATION_44 * 0, Interpolate44(0.0, 0.0, 1.0, 1200, -17600), Interpolate44(0.0, 0.0, 1.0, -5000, 4200));
            AssertSample(stream, SAMPLE_DURATION_44 * 1, Interpolate44(0.5, 0.0, 1.0, 1200, -17600), Interpolate44(0.5, 0.0, 1.0, -5000, 4200));
            AssertSample(stream, SAMPLE_DURATION_44 * 2, Interpolate44(1.0, 1.0, 2.0, -17600, 17600), Interpolate44(1.0, 1.0, 2.0, 4200, 3200));
            AssertSample(stream, SAMPLE_DURATION_44 * 3, Interpolate44(1.5, 1.0, 2.0, -17600, 17600), Interpolate44(1.5, 1.0, 2.0, 4200, 3200));
            AssertSample(stream, SAMPLE_DURATION_44 * 4, Interpolate44(2.0, 2.0, 3.0, 17600, 0), Interpolate44(2.0, 2.0, 3.0, 3200, 0));
            AssertSample(stream, SAMPLE_DURATION_44 * 5, Interpolate44(2.5, 2.0, 3.0, 17600, 0), Interpolate44(2.5, 2.0, 3.0, 3200, 0));
            AssertSample(stream, SAMPLE_DURATION_44 * 6, 0, 0);
        }
    }
}