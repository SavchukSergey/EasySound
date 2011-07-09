using NUnit.Framework;

namespace EasySound.Tests {
    [TestFixture]
    public class WavAudioStreamBits16Test : WavAudioStreamBits16TestBase {

        protected override AudioStream OpenTestStream() {
            return new WavAudioStream(OpenResource("PNTY_Radio_Music Bed_F.wav"));
        }

        [Test]
        public override void GetSampleTest() {
            base.GetSampleTest();
        }

        [Test]
        public override void BitsPerSampleTest() {
            base.BitsPerSampleTest();
        }

        [Test]
        public override void SampleRateTest() {
            base.SampleRateTest();
        }

        [Test]
        public override void LengthTest() {
            base.LengthTest();
        }

        [Test]
        public override void SampleCountTest() {
            base.SampleCountTest();
        }

        [Test]
        public override void ChannelsCountTest() {
            base.ChannelsCountTest();
        }

    }
}