using System;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using EasySound.Filters;

namespace EasySound {
    class Program {
        static void Main(string[] args) {
            if (args.Length == 0) {
                Usage();
                return;
            }
            XDocument doc = XDocument.Load(args[0]);
            foreach (var job in doc.Root.Elements()) {
                var primElements = job.Elements();
                if (primElements.Count() != 1) throw new InvalidOperationException("Expected only one root for audio job");
                AudioStream stream = ParseStream(primElements.First());
                using (var output = File.Open(job.Attribute("target").Value, FileMode.Create)) {
                    stream.SaveWav(output);
                }
                stream.Dispose();
            }
        }

        static AudioStream ParseStream(XElement element) {
            switch (element.Name.LocalName) {
                case "harmonic":
                    return ParseHarmonicStream(element);
                case "serial":
                    return ParseSerialStream(element);
                case "parallel":
                    return ParseParallelStream(element);
                case "volume":
                    return ParseVolumeStream(element);
                case "trimmed":
                    return ParseTrimmedStream(element);
                case "lowpass":
                    return ParseLowPassStream(element);
                case "highpass":
                    return ParseHighPassStream(element);
                case "load-full":
                    return ParseWavFullReadStream(element);
            }
            throw new InvalidOperationException("Unknown stream type " + element.Name.LocalName);
        }

        private static AudioStream ParseHarmonicStream(XElement element) {
            var xFreq = element.Attribute("freq");
            if (xFreq == null) {
                throw new Exception("freq is required attribute for Harmonic stream");
            }
            double freq;
            if (!double.TryParse(xFreq.Value, out freq)) {
                throw new Exception("freq has ivalid value for Harmonic stream");
            }
            var xAmp = element.Attribute("amp");
            if (xAmp == null) {
                throw new Exception("amp is required attribute for Harmonic stream");
            }
            double amp;
            if (!double.TryParse(xAmp.Value, out amp)) {
                throw new Exception("amp has ivalid value for Harmonic stream");
            }
            var xLen = element.Attribute("len");
            if (xLen == null) {
                throw new Exception("amp is required attribute for Harmonic stream");
            }
            double length;
            if (!double.TryParse(xLen.Value, out length)) {
                throw new Exception("len has ivalid value for Harmonic stream");
            }
            return new HarmonicAudioStream(freq, amp, length);
        }

        private static AudioStream ParseWavFullReadStream(XElement element) {
            var src = element.Attribute("src").Value;
            var srcStream = File.Open(src, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return new WavFullReadAudioStream(srcStream);
        }

        private static AudioStream ParseParallelStream(XElement element) {
            var parallel = new ParallelCompositeAudioStream();
            foreach (var elem in element.Elements()) {
                parallel.AddStream(ParseStream(elem));
            }
            return parallel;
        }

        private static AudioStream ParseSerialStream(XElement element) {
            var serial = new SerialCompositeAudioStream();
            foreach (var elem in element.Elements()) {
                serial.AddStream(ParseStream(elem));
            }
            return serial;
        }

        private static AudioStream ParseVolumeStream(XElement element) {
            double factor;
            if (!double.TryParse(element.Attribute("factor").Value, out factor)) {
                throw new FormatException("factor element for trimming stream is missing or incorrect");
            }
            return new VolumeAdjustmentAudioStream(ParseSingleChildAudioStream(element), factor);
        }

        private static AudioStream ParseTrimmedStream(XElement element) {
            double start, end;
            if (!double.TryParse(element.Attribute("start").Value, out start)) {
                throw new FormatException("start attribute for trimming stream is missing or incorrect");
            }
            if (!double.TryParse(element.Attribute("end").Value, out end)) {
                throw new FormatException("end attribute for trimming stream is missing or incorrect");
            }
            return new TrimmedAudioStream(ParseSingleChildAudioStream(element), start, end);
        }

        private static AudioStream ParseLowPassStream(XElement element) {
            double freq;
            if (!double.TryParse(element.Attribute("freq").Value, out freq)) {
                throw new FormatException("freq attribute for low-pass stream is missing or incorrect");
            }
           
            return new LowPassFilterAudioStream(ParseSingleChildAudioStream(element), freq);
        }

        private static AudioStream ParseHighPassStream(XElement element) {
            double freq;
            if (!double.TryParse(element.Attribute("freq").Value, out freq)) {
                throw new FormatException("freq attribute for hi-pass stream is missing or incorrect");
            }

            return new LowPassFilterAudioStream(ParseSingleChildAudioStream(element), freq);
        }

        private static AudioStream ParseSingleChildAudioStream(XElement element) {
            return ParseStream(element.Elements().First());
        }

        static void Usage() {
            System.Console.WriteLine("es.exe job.xml");
        }

    }
}