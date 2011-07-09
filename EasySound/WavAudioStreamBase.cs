using System;
using System.Text;

namespace EasySound {
    /// <summary>
    /// Represents base class for WAVE audio stream. See https://ccrma.stanford.edu/courses/422/projects/WaveFormat/ for the reference.
    /// </summary>
    public abstract class WavAudioStreamBase : AudioStream {

        /// <summary>
        /// Reads 4 bytes signature from the stream. 
        /// </summary>
        /// <returns>Read 4 character String from the stream.</returns>
        protected abstract string ReadSignature();

        /// <summary>
        /// Reads UInt32 from the stream.
        /// </summary>
        /// <returns>UInt32 value read from the stream.</returns>
        protected abstract uint ReadUInt32();

        /// <summary>
        /// Reads UInt16 from the stream.
        /// </summary>
        /// <returns>UInt16 value read from the stream.</returns>
        protected abstract ushort ReadUInt16();

        /// <summary>
        /// Reads Int16 from the stream.
        /// </summary>
        /// <returns>Int16 value read from the stream.</returns>
        protected abstract short ReadInt16();

        /// <summary>
        /// Reads byte from the stream.
        /// </summary>
        /// <returns>Byte value read from the stream.</returns>
        protected abstract byte ReadByte();

        /// <summary>
        /// Reads bytes from the stream.
        /// </summary>
        /// <param name="count">Count of bytes to be read.</param>
        /// <returns>Array of read bytes.</returns>
        protected abstract byte[] ReadBytes(uint count);

        /// <summary>
        /// Skipps bytes in stream.
        /// </summary>
        /// <param name="bytes"></param>
        protected abstract void SkipBytes(uint bytes);

        /// <summary>
        /// Gets or sets stream position in bytes.
        /// </summary>
        protected abstract uint StreamPosition { get; set; }

        /// <summary>
        /// Gets count of channels. (1 for mono, 2 for stereo)
        /// </summary>
        public override ushort ChannelsCount {
            get { return _formatChunk.NumChannels; }
        }

        /// <summary>
        /// Gets count of bits per sample. (16 or 8)
        /// </summary>
        public override ushort BitsPerSample {
            get { return _formatChunk.BitsPerSample; }
        }

        private uint _endOfDataPosition;

        private uint _startOfDataPosition;

        private ReadSampleHandler _sampleReader;

        /// <summary>
        /// Gets whether data stream is ended or not.
        /// </summary>
        protected bool IsEndOfData { get { return StreamPosition >= _endOfDataPosition; } }

        /// <summary>
        /// Gets whether stream is ended ot not.
        /// </summary>
        protected abstract bool IsEndOfStream { get; }

        private WaveFormatChunk _formatChunk;

        private WaveBroadcastChunk _broadcastChunk;

        /// <summary>
        /// Initializes all data from stream.
        /// </summary>
        protected void Initialize() {
            string chunkId = ReadSignature();
            if (chunkId != "RIFF") throw new FormatException("Invalid magic signature");

            uint chunkSize = ReadUInt32();

            string format = ReadSignature();
            if (format != "WAVE") throw new FormatException("Invalid format signature");

            while (!IsEndOfStream) {
                string id;
                uint size;
                ReadSubchunk(out id, out size);
                var chunkStart = StreamPosition;
                switch (id) {
                    case "fmt ":
                        ReadWaveFormatChunk(out _formatChunk);
                        SampleRate = _formatChunk.SampleRate;
                        break;
                    case "data":
                        _startOfDataPosition = StreamPosition;
                        _endOfDataPosition = StreamPosition + size;
                        SamplesCount = (uint)(size / _formatChunk.NumChannels / (_formatChunk.BitsPerSample >> 3));
                        break;
                    case "bext":
                        ReadWaveBroadcastChunk(out _broadcastChunk, size);
                        break;
                }
                StreamPosition = chunkStart + size;
            }

            if (_formatChunk.AudioFormat != 1) throw new FormatException("Unknown AudioFormat");
            if (_formatChunk.NumChannels > 2) throw new FormatException("Too many channels");

            if (_formatChunk.NumChannels == 2 && _formatChunk.BitsPerSample == 16) {
                _sampleReader = ReadSamplePCM16Stereo;
            } else if (_formatChunk.NumChannels == 1 && _formatChunk.BitsPerSample == 16) {
                _sampleReader = ReadSamplePCM16Mono;
            } else if (_formatChunk.NumChannels == 2 && _formatChunk.BitsPerSample == 8) {
                _sampleReader = ReadSamplePCM8Stereo;
            } else {
                throw new NotSupportedException();
            }

            StreamPosition = _startOfDataPosition;
        }

        private void ReadWaveFormatChunk(out WaveFormatChunk chunk) {
            chunk.AudioFormat = ReadUInt16();
            chunk.NumChannels = ReadUInt16();
            chunk.SampleRate = ReadUInt32();
            chunk.ByteRate = ReadUInt32();
            chunk.BlockAlign = ReadUInt16();
            chunk.BitsPerSample = ReadUInt16();
        }

        private void ReadWaveBroadcastChunk(out WaveBroadcastChunk chunk, uint chunkSize) {
            var start = StreamPosition;
            chunk.Description = Encoding.ASCII.GetString(ReadBytes(256)).Trim('\0');
            chunk.Originator = Encoding.ASCII.GetString(ReadBytes(32)).Trim('\0');
            chunk.OriginatorReference = Encoding.ASCII.GetString(ReadBytes(32)).Trim('\0');
            chunk.OriginationDate = Encoding.ASCII.GetString(ReadBytes(10)).Trim('\0');
            chunk.OriginationTime = Encoding.ASCII.GetString(ReadBytes(8)).Trim('\0');

            chunk.TimeReferenceLow = ReadUInt32();
            chunk.TimeReferenceHigh = ReadUInt32();
            chunk.Version = ReadUInt16();
            chunk.UMID = ReadBytes(64);
            chunk.Reserved = ReadBytes(190);
            uint codingHistorySize = chunkSize - (StreamPosition - start);
            chunk.CodingHistory = Encoding.ASCII.GetString(ReadBytes(codingHistorySize)).Trim('\0');
        }

        private void ReadSubchunk(out string id, out uint size) {
            id = ReadSignature();
            size = ReadUInt32();
        }

        private void ReadSamplePCM16Stereo(out Sample sample) {
            sample.Time = _position;
            sample.Left = ReadInt16();
            sample.Right = ReadInt16();
        }

        private void ReadSamplePCM8Stereo(out Sample sample) {
            sample.Time = _position;
            sample.Left = Sample.ToBits16(ReadByte());
            sample.Right = Sample.ToBits16(ReadByte());
        }

        private void ReadSamplePCM16Mono(out Sample sample) {
            sample.Time = _position;
            sample.Left = ReadInt16();
            sample.Right = sample.Left;
        }

        /// <summary>
        /// Reads next available sample.
        /// </summary>
        /// <param name="sample">Sample to be filled with data.</param>
        public override void ReadSample(out Sample sample) {
            if (IsEndOfData) {
                ReadNullSample(out sample);
                return;
            }
            _sampleReader(out sample);
            _position += SampleDuration;
        }

    }
}