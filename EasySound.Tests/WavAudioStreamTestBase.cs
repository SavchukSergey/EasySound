using NUnit.Framework;

namespace EasySound.Tests {
    public abstract class WavAudioStreamTestBase : AudioStreamTestBase {

        protected abstract AudioStream OpenTestStream();

        public virtual void SampleRateTest() {
            using (var stream = OpenTestStream()) {
                Assert.AreEqual(SAMPLE_RATE_44, stream.SampleRate);
            }
        }

        public virtual void LengthTest() {
            using (var stream = OpenTestStream()) {
                Assert.AreEqual(30, stream.Length);
            }
        }

        public virtual void SampleCountTest() {
            using (var stream = OpenTestStream()) {
                Assert.AreEqual(1323000u, stream.SamplesCount);
            }
        }

        public virtual void ChannelsCountTest() {
            using (var stream = OpenTestStream()) {
                Assert.AreEqual(2u, stream.ChannelsCount);
            }
        }

    }
}