using NUnit.Framework;

namespace EasySound.Tests {
    [TestFixture]
    public class VolumeAdjustmentAudioStreamTest : AudioStreamTestBase {

        private static VolumeAdjustmentAudioStream OpenTestStream(double volumeFactor, ushort bitsPerSample, ushort channelsCount) {
            using (var innerStream = new StubAudioStream(SAMPLE_RATE_44, bitsPerSample, channelsCount)) {
                innerStream.AddSample(1200, -5000);
                innerStream.AddSample(-11600, 4200);
                innerStream.AddSample(11600, 3200);

                return new VolumeAdjustmentAudioStream(innerStream, volumeFactor);
            }
        }

        [Test]
        public void LeftChannelOverflowMaxTest() {
            using (var stream = new StubAudioStream(SAMPLE_RATE_44, 16, 2)) {
                stream.AddSample(new Sample { Left = short.MaxValue, Right = 0, Time = 0 });
                var vs = new VolumeAdjustmentAudioStream(stream, 2);
                Sample res;
                vs.ReadSample(out res);
                Assert.AreEqual(short.MaxValue, res.Left);
            }
        }

        [Test]
        public void LeftChannelOverflowMinTest() {
            using (var stream = new StubAudioStream(SAMPLE_RATE_44, 16, 2)) {
                stream.AddSample(new Sample { Left = short.MinValue, Right = 0, Time = 0 });
                var vs = new VolumeAdjustmentAudioStream(stream, 2);
                Sample res;
                vs.ReadSample(out res);
                Assert.AreEqual(short.MinValue, res.Left);
            }
        }

        [Test]
        public void RightChannelOverflowMaxTest() {
            using (var stream = new StubAudioStream(SAMPLE_RATE_44, 16, 2)) {
                stream.AddSample(new Sample { Left = 0, Right = short.MaxValue, Time = 0 });
                var vs = new VolumeAdjustmentAudioStream(stream, 2);
                Sample res;
                vs.ReadSample(out res);
                Assert.AreEqual(short.MaxValue, res.Right);
            }
        }

        [Test]
        public void RightChannelOverflowMinTest() {
            using (var stream = new StubAudioStream(SAMPLE_RATE_44, 16, 2)) {
                stream.AddSample(new Sample { Left = 0, Right = short.MinValue, Time = 0 });
                var vs = new VolumeAdjustmentAudioStream(stream, 2);
                Sample res;
                vs.ReadSample(out res);
                Assert.AreEqual(short.MinValue, res.Right);
            }
        }

        [Test]
        public void SampleRateTest() {
            using (var stream = OpenTestStream(2.0, 16, 2)) {
                Assert.AreEqual(SAMPLE_RATE_44, stream.SampleRate);
            }
        }

        [Test]
        public void LengthTest() {
            var stream = OpenTestStream(2.0, 16, 2);
            Assert.AreEqual(3 * SAMPLE_DURATION_44, stream.Length);
        }

        [Test]
        public void SampleCountTest() {
            var stream = OpenTestStream(2.0, 16, 2);
            Assert.AreEqual(3u, stream.SamplesCount);
        }

        [Test]
        public void BitsPerSampleTest() {
            using (var stream = OpenTestStream(2.0, 8, 2)) {
                Assert.AreEqual(8, stream.BitsPerSample);
            }
            using (var stream = OpenTestStream(2.0, 16, 2)) {
                Assert.AreEqual(16, stream.BitsPerSample);
            }
        }

        [Test]
        public void ChannelsCountTest() {
            using (var stream1 = OpenTestStream(2.0, 16, 1)) {
                Assert.AreEqual(1u, stream1.ChannelsCount);
            }
            using (var stream2 = OpenTestStream(2.0, 16, 2)) {
                Assert.AreEqual(2u, stream2.ChannelsCount);
            }
        }

        [Test]
        public void GetSampleTest() {
            using (var stream = OpenTestStream(2.0, 16, 2)) {
                AssertSample(stream, SAMPLE_DURATION_44 * 0, 1200 * 2, -5000 * 2);
                AssertSample(stream, SAMPLE_DURATION_44 * 1, -11600 * 2, 4200 * 2);
                AssertSample(stream, SAMPLE_DURATION_44 * 2, 11600 * 2, 3200 * 2);
                AssertSample(stream, SAMPLE_DURATION_44 * 3, 0, 0);
            }
        }

    }
}