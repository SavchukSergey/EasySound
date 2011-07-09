using NUnit.Framework;

namespace EasySound.Tests {
    public abstract class WavAudioStreamBits8TestBase : WavAudioStreamTestBase {

        public virtual void GetSampleTest() {
            using (var stream = OpenTestStream()) {
                AssertSample(stream, SAMPLE_DURATION_44 * 0, -129, -129);
                AssertSample(stream, SAMPLE_DURATION_44 * 1, -129, -129);
                AssertSample(stream, SAMPLE_DURATION_44 * 2, -129, -129);
                AssertSample(stream, SAMPLE_DURATION_44 * 3, -129, -129);
            }
        }

        public virtual void BitsPerSampleTest() {
            using (var stream = OpenTestStream()) {
                Assert.AreEqual(8, stream.BitsPerSample);
            }
        }

    }
}