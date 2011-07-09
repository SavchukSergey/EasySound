using System.IO;
using System;

namespace EasySound.Tests {
    public class AudioStreamTestBase {

        protected const uint SAMPLE_RATE_44 = 44100;
        protected const uint SAMPLE_RATE_22 = 22050;

        protected const double SAMPLE_DURATION_44 = 1.0 / SAMPLE_RATE_44;
        protected const double SAMPLE_DURATION_22 = 1.0 / SAMPLE_RATE_22;

        protected void AssertSample(AudioStream stream, double time, int left, int right) {
            Sample actual;
            stream.ReadSample(out actual);
            Sample expected;
            expected.Time = time;
            expected.Left = (short)left;
            expected.Right = (short)right;
            AssertExt.AreEqual(expected, actual);
        }

        protected short Interpolate(double relativePosition, short first, short second) {
            return (short)((second - first) * relativePosition + first);
        }

        protected short Interpolate44(double timeCurrent, double timeFirst, double timeSecond, short first, short second) {
            timeCurrent *= SAMPLE_DURATION_44;
            timeFirst *= SAMPLE_DURATION_44;
            timeSecond *= SAMPLE_DURATION_44;
            double rel = (timeCurrent - timeFirst) / (timeSecond - timeFirst);
            return Interpolate(rel, first, second);
        }

        protected Stream GetEmbeddedResource(string resourceName) {
            Type type = GetType();
            string ns = type.Namespace;
            return type.Assembly.GetManifestResourceStream(
                    ns + ".Resources." + resourceName);
        }

        protected Stream OpenResource(string resourceName) {
            string root = Path.GetDirectoryName(new Uri(this.GetType().Assembly.CodeBase).AbsolutePath);
            root = Path.Combine(root, "Resources");
            string path = Path.Combine(root, resourceName);
            return File.Open(path, FileMode.Open);
        }
    }
}