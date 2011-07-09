using System;
using System.IO;

namespace EasySound {
    /// <summary>
    /// Represents base class which provides stream of audio samples
    /// </summary>
    public abstract class AudioStream : IDisposable {

        /// <summary>
        /// Current position in seconds.
        /// </summary>
        protected double _position;

        private double _length;

        private uint _sampleCount;

        private uint _sampleRate;

        private double _sampleDuration;

        /// <summary>
        /// Gets count of bits per sample. (16 or 8)
        /// </summary>
        public abstract ushort BitsPerSample { get; }

        /// <summary>
        /// Gets count of channels. (1 for mono, 2 for stereo)
        /// </summary>
        public abstract ushort ChannelsCount { get; }

        /// <summary>
        /// Gets count of audio sample.
        /// </summary>
        public uint SamplesCount {
            get { return _sampleCount; }
            protected set {
                _sampleCount = value;
                RecalcValues();
            }
        }

        /// <summary>
        /// Gets sample rate. (44100 Hz, 22050 Hz, etc)
        /// </summary>
        public uint SampleRate {
            get {
                return _sampleRate;
            }
            protected set {
                _sampleRate = value;
                RecalcValues();
            }
        }

        /// <summary>
        /// Recalculates sample duration and length.
        /// </summary>
        protected void RecalcValues() {
            if (_sampleRate != 0) {
                _length = (double)_sampleCount / _sampleRate;
                _sampleDuration = 1.0 / _sampleRate;
            } else {
                _length = 0;
                _sampleDuration = double.MaxValue;
            }
        }

        /// <summary>
        /// Gets length of audio track.
        /// </summary>
        public double Length { get { return _length; } }

        /// <summary>
        /// Gets sample duration in seconds. (1 / SampleRate)
        /// </summary>
        public double SampleDuration { get { return _sampleDuration; } }

        /// <summary>
        /// Gets current position in seconds.
        /// </summary>
        public double Position { get { return _position; } }

        /// <summary>
        /// Reads next available sample.
        /// </summary>
        /// <param name="sample">Sample to be filled with data.</param>
        public abstract void ReadSample(out Sample sample);

        /// <summary>
        /// Saves audio track as wav file to stream.
        /// </summary>
        /// <param name="output">Output stream to write wav file to.</param>
        public void SaveWav(Stream output) {
            ushort channelsCount = ChannelsCount;
            uint sampleCount = SamplesCount;
            ushort bitsPerSample = BitsPerSample;
            ushort bytesPerSample = (ushort)(bitsPerSample >> 3);

            uint chunk1Size = 16;
            uint chunk2Size = sampleCount * channelsCount * bytesPerSample;
            uint chunkSize = 4 + (8 + chunk1Size) + (8 + chunk2Size);

            var writer = new BinaryWriter(output);
            WriteSignature(writer, "RIFF");
            writer.Write(chunkSize);
            WriteSignature(writer, "WAVE");

            WriteChunkInfo(writer, "fmt ", chunk1Size);
            writer.Write((ushort)1); //AudioFormat
            writer.Write(channelsCount);
            writer.Write(SampleRate);
            uint byteRate = SampleRate * channelsCount * bytesPerSample;
            writer.Write(byteRate);
            ushort blockAlign = (ushort)(channelsCount * bytesPerSample);
            writer.Write(blockAlign);
            writer.Write(bitsPerSample);

            WriteChunkInfo(writer, "data", chunk2Size);
            for (int i = 0; i < sampleCount; i++) {
                Sample sample;
                ReadSample(out sample);
                if (channelsCount == 1) {
                    if (bytesPerSample == 1) {
                        writer.Write(sample.GetMono8());
                    } else if (bytesPerSample == 2) {
                        writer.Write(sample.GetMono16());
                    } else {
                        throw new NotSupportedException();
                    }
                } else if (channelsCount == 2) {
                    if (bytesPerSample == 1) {
                        writer.Write(Sample.ToBits8(sample.Left));
                        writer.Write(Sample.ToBits8(sample.Right));
                    } else if (bytesPerSample == 2) {
                        writer.Write(sample.Left);
                        writer.Write(sample.Right);
                    } else {
                        throw new NotSupportedException();
                    }
                } else {
                    throw new NotSupportedException();
                }
            }
        }

        /// <summary>
        /// Writes subchunk info.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="id"></param>
        /// <param name="size"></param>
        private static void WriteChunkInfo(BinaryWriter writer, string id, uint size) {
            WriteSignature(writer, id);
            writer.Write(size);

        }

        /// <summary>
        /// Writes 4bytes signature to stream.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="signature"></param>
        private static void WriteSignature(BinaryWriter writer, string signature) {
            writer.Write(signature[0]);
            writer.Write(signature[1]);
            writer.Write(signature[2]);
            writer.Write(signature[3]);
        }

        /// <summary>
        /// Read null sample and advances position by SampleDuration.
        /// </summary>
        /// <param name="sample"></param>
        protected void ReadNullSample(out Sample sample) {
            sample.Time = _position;
            sample.Left = 0;
            sample.Right = 0;
            _position += SampleDuration;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public abstract void Dispose();

    }
}