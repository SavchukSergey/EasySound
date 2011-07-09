using System.IO;

namespace EasySound {
    /// <summary>
    /// Represents WAVE audio stream.
    /// </summary>
    public class WavAudioStream : WavAudioStreamBase {
        private readonly Stream _stream;

        private readonly BinaryReader _reader;

        /// <summary>
        /// Creates an instance of WavAudioStream.
        /// </summary>
        /// <param name="stream">Stream to read data from.</param>
        public WavAudioStream(Stream stream) {
            _stream = stream;
            _reader = new BinaryReader(stream);
            Initialize();
        }

        /// <summary>
        /// Reads 4 bytes signature from the stream. 
        /// </summary>
        /// <returns>Read 4 character String from the stream.</returns>
        protected override string ReadSignature() {
            char ch1 = (char)_reader.ReadByte();
            char ch2 = (char)_reader.ReadByte();
            char ch3 = (char)_reader.ReadByte();
            char ch4 = (char)_reader.ReadByte();
            return new string(new[] { ch1, ch2, ch3, ch4 });
        }

        /// <summary>
        /// Reads UInt32 from the stream.
        /// </summary>
        /// <returns>UInt32 value read from the stream.</returns>
        protected override uint ReadUInt32() {
            return _reader.ReadUInt32();
        }

        /// <summary>
        /// Reads UInt16 from the stream.
        /// </summary>
        /// <returns>UInt16 value read from the stream.</returns>
        protected override ushort ReadUInt16() {
            return _reader.ReadUInt16();
        }

        /// <summary>
        /// Reads Int16 from the stream.
        /// </summary>
        /// <returns>Int16 value read from the stream.</returns>
        protected override short ReadInt16() {
            return _reader.ReadInt16();
        }

        /// <summary>
        /// Reads byte from the stream.
        /// </summary>
        /// <returns>Byte value read from the stream.</returns>
        protected override byte ReadByte() {
            return _reader.ReadByte();
        }

        /// <summary>
        /// Reads bytes from the stream.
        /// </summary>
        /// <param name="count">Count of bytes to be read.</param>
        /// <returns>Array of read bytes.</returns>
        protected override byte[] ReadBytes(uint count) {
            var buffer = new byte[count];
            _reader.Read(buffer, 0, (int)count);
            return buffer;
        }

        /// <summary>
        /// Gets or sets stream position in bytes.
        /// </summary>
        protected override uint StreamPosition {
            get { return (uint)_stream.Position; }
            set { _stream.Seek(value, SeekOrigin.Begin); }
        }

        /// <summary>
        /// Gets whether stream is ended ot not.
        /// </summary>
        protected override bool IsEndOfStream {
            get { return _stream.Position >= _stream.Length; }
        }

        /// <summary>
        /// Skipps bytes in stream.
        /// </summary>
        /// <param name="bytes"></param>
        protected override void SkipBytes(uint bytes) {
            _stream.Seek(bytes, SeekOrigin.Current);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose() {
            _stream.Dispose();
        }
    }
}