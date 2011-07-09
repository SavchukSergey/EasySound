using NUnit.Framework;

namespace EasySound.Tests {
    public static class AssertExt {

        public static void AreEqual(Sample expected, Sample actual) {
            Assert.AreEqual(expected.Time, actual.Time);
            Assert.AreEqual(expected.Left, actual.Left);
            Assert.AreEqual(expected.Right, actual.Right);
        }
    }
}