using NUnit.Framework;

namespace EasySound.Tests {
    [TestFixture]
    public class TrimmedAudioStreamTest : AudioStreamTestBase {

        private static TrimmedAudioStream GetTestStream(int startSamples, int endSamples, ushort bitsPerSample, ushort channelsCount) {
            using (var innerStream = new StubAudioStream(SAMPLE_RATE_44, bitsPerSample, channelsCount)) {
                innerStream.AddSample(100, -100);
                innerStream.AddSample(200, -200);
                innerStream.AddSample(300, -300);
                innerStream.AddSample(400, -400);

                innerStream.AddSample(1200, -5000);
                innerStream.AddSample(-17600, 4200);
                innerStream.AddSample(17600, 3200);

                innerStream.AddSample(500, -500);
                innerStream.AddSample(600, -600);
                innerStream.AddSample(700, -700);

                return new TrimmedAudioStream(innerStream, startSamples * SAMPLE_DURATION_44,
                                              endSamples * SAMPLE_DURATION_44);
            }
        }

        [Test]
        public void SampleRateTest() {
            var stream = GetTestStream(4, 6, 16, 2);
            Assert.AreEqual(SAMPLE_RATE_44, stream.SampleRate);
        }

        [Test]
        public void LengthTest() {
            var stream = GetTestStream(4, 6, 16, 2);
            Assert.AreEqual(3 * SAMPLE_DURATION_44, stream.Length);
        }

        [Test]
        public void SampleCountTest() {
            var stream = GetTestStream(4, 6, 16, 2);
            Assert.AreEqual(3u, stream.SamplesCount);
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
            var stream1 = GetTestStream(4, 6, 16, 1);
            Assert.AreEqual(1u, stream1.ChannelsCount);
            var stream2 = GetTestStream(4, 6, 16, 2);
            Assert.AreEqual(2u, stream2.ChannelsCount);
        }

        [Test]
        public void GetSampleTest() {
            var stream = GetTestStream(4, 6, 16, 2);
            AssertSample(stream, SAMPLE_DURATION_44 * 0, 1200, -5000);
            AssertSample(stream, SAMPLE_DURATION_44 * 1, -17600, 4200);
            AssertSample(stream, SAMPLE_DURATION_44 * 2, 17600, 3200);
            AssertSample(stream, SAMPLE_DURATION_44 * 3, 0, 0);
        }

    }
}