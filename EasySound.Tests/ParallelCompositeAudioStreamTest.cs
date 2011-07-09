using System;
using NUnit.Framework;

namespace EasySound.Tests {
    [TestFixture]
    public class ParallelCompositeAudioStreamTest : CompositeAudioStreamTestBase {

        [Test]
        public void SampleRateTest() {
            var stream1 = GetTestStream1(SAMPLE_RATE_44, 16, 2);
            var stream2 = GetTestStream2(SAMPLE_RATE_22, 16, 2);
            var stream = new ParallelCompositeAudioStream(stream1, stream2);
            Assert.AreEqual(SAMPLE_RATE_44, stream.SampleRate);
            stream = new ParallelCompositeAudioStream(stream2, stream1);
            Assert.AreEqual(SAMPLE_RATE_44, stream.SampleRate);
        }

        [Test]
        public void LengthTest() {
            var stream1 = GetTestStream1(SAMPLE_RATE_44, 16, 2);
            var stream2 = GetTestStream2(SAMPLE_RATE_22, 16, 2);

            double length = Math.Max(stream1.Length, stream2.Length);
            var stream = new ParallelCompositeAudioStream(stream1, stream2);
            Assert.AreEqual(length, stream.Length);
            stream = new ParallelCompositeAudioStream(stream2, stream1);
            Assert.AreEqual(length, stream.Length);
        }

        [Test]
        public void SampleCountTest() {
            var stream1 = GetTestStream1(SAMPLE_RATE_44, 16, 1);
            var stream2 = GetTestStream2(SAMPLE_RATE_22, 16, 2);

            var stream = new ParallelCompositeAudioStream(stream1, stream2);
            Assert.AreEqual(2u, stream.ChannelsCount);
            stream = new ParallelCompositeAudioStream(stream2, stream1);
            Assert.AreEqual(2u, stream.ChannelsCount);
        }

        [Test]
        public override void ChannelsCountTest() {
            base.ChannelsCountTest();
        }

        [Test]
        public override void BitsPerSampleTest() {
            base.BitsPerSampleTest();
        }

        [Test]
        public void GetSampleSameRateTest() {
            var stream1 = GetTestStream1(SAMPLE_RATE_44, 16, 2);
            var stream2 = GetTestStream2(SAMPLE_RATE_44, 16, 2);

            var stream = new ParallelCompositeAudioStream(stream1, stream2);
            AssertSample(stream, SAMPLE_DURATION_44 * 0, 1200 + 7895, -5000 - 5000);
            AssertSample(stream, SAMPLE_DURATION_44 * 1, -17600 + 4765, 4200 + 12345);
            AssertSample(stream, SAMPLE_DURATION_44 * 2, 17600, 3200);
            AssertSample(stream, SAMPLE_DURATION_44 * 3, 0, 0);
        }

        [Test]
        public void GetSampleDifferentRateTest() {
            var stream1 = GetTestStream1(SAMPLE_RATE_22, 16, 2);
            var stream2 = GetTestStream2(SAMPLE_RATE_44, 16, 2);

            var stream = new ParallelCompositeAudioStream(stream1, stream2);
            AssertSample(stream, SAMPLE_DURATION_44 * 0, Interpolate44(0.0, 0.0, 1.0, 1200, -17600) + 7895, Interpolate44(0.0, 0.0, 1.0, -5000, 4200) - 5000);
            AssertSample(stream, SAMPLE_DURATION_44 * 1, Interpolate44(0.5, 0.0, 1.0, 1200, -17600) + 4765, Interpolate44(0.5, 0.0, 1.0, -5000, 4200) + 12345);
            AssertSample(stream, SAMPLE_DURATION_44 * 2, Interpolate44(1.0, 1.0, 2.0, -17600, 17600) + 0, Interpolate44(1.0, 1.0, 2.0, 4200, 3200) + 0);
            AssertSample(stream, SAMPLE_DURATION_44 * 3, Interpolate44(1.5, 1.0, 2.0, -17600, 17600) + 0, Interpolate44(1.5, 1.0, 2.0, 4200, 3200) + 0);
            AssertSample(stream, SAMPLE_DURATION_44 * 4, Interpolate44(2.0, 2.0, 3.0, 17600, 0) + 0, Interpolate44(2.0, 2.0, 3.0, 3200, 0) + 0);
            AssertSample(stream, SAMPLE_DURATION_44 * 5, Interpolate44(2.5, 2.0, 3.0, 17600, 0) + 0, Interpolate44(2.5, 2.0, 3.0, 3200, 0) + 0);
            AssertSample(stream, SAMPLE_DURATION_44 * 6, 0, 0);
        }

        protected override AudioStream JoinStreams(AudioStream first, AudioStream second) {
            return new ParallelCompositeAudioStream(first, second);
        }
    }
}