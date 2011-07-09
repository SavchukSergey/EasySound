using NUnit.Framework;

namespace EasySound.Tests {
    public abstract class WavAudioStreamBits16TestBase : WavAudioStreamTestBase {

        public virtual void GetSampleTest() {
            using (var stream = OpenTestStream()) {
                AssertSample(stream, SAMPLE_DURATION_44 * 0, -1, -1);
                AssertSample(stream, SAMPLE_DURATION_44 * 1, -1, -1);
                AssertSample(stream, SAMPLE_DURATION_44 * 2, -1, -1);
                AssertSample(stream, SAMPLE_DURATION_44 * 3, -1, -1);
            }
        }

        public virtual void BitsPerSampleTest() {
            using (var stream = OpenTestStream()) {
                Assert.AreEqual(16, stream.BitsPerSample);
            }
        }

    }
}