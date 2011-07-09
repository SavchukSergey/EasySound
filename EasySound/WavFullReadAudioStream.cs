using System.IO;

namespace EasySound {
    /// <summary>
    /// Represents WAVE audio stream with caching stream in the memory.
    /// </summary>
    public class WavFullReadAudioStream : WavAudioStreamBase {
        private readonly Stream _stream;

        private readonly byte[] _buffer;
        private uint _streamPosition;

        /// <summary>
        /// Creates an instance of WavFullReadAudioStream.
        /// </summary>
        /// <param name="stream">Stream to read data from.</param>
        public WavFullReadAudioStream(Stream stream) {
            _stream = stream;
            _buffer = new byte[stream.Length];
            _streamPosition = 0;
            _stream.Read(_buffer, 0, _buffer.Length);
            Initialize();
        }

        /// <summary>
        /// Reads 4 bytes signature from the stream. 
        /// </summary>
        /// <returns>Read 4 character String from the stream.</returns>
        protected override string ReadSignature() {
            char ch1 = (char)_buffer[_streamPosition++];
            char ch2 = (char)_buffer[_streamPosition++];
            char ch3 = (char)_buffer[_streamPosition++];
            char ch4 = (char)_buffer[_streamPosition++];
            return new string(new[] { ch1, ch2, ch3, ch4 });
        }

        /// <summary>
        /// Reads UInt32 from the stream.
        /// </summary>
        /// <returns>UInt32 value read from the stream.</returns>
        protected override uint ReadUInt32() {
            uint res = (uint)(_buffer[_streamPosition++] << 0);
            res = res | (uint)(_buffer[_streamPosition++] << 8);
            res = res | (uint)(_buffer[_streamPosition++] << 16);
            res = res | (uint)(_buffer[_streamPosition++] << 24);
            return res;
        }

        /// <summary>
        /// Reads UInt16 from the stream.
        /// </summary>
        /// <returns>UInt16 value read from the stream.</returns>
        protected override ushort ReadUInt16() {
            uint res = (uint)(_buffer[_streamPosition++] << 0);
            res = res | (uint)(_buffer[_streamPosition++] << 8);
            return (ushort)res;
        }

        /// <summary>
        /// Reads byte from the stream.
        /// </summary>
        /// <returns>Byte value read from the stream.</returns>
        protected override byte ReadByte() {
            return _buffer[_streamPosition++];
        }

        /// <summary>
        /// Reads bytes from the stream.
        /// </summary>
        /// <param name="count">Count of bytes to be read.</param>
        /// <returns>Array of read bytes.</returns>
        protected override byte[] ReadBytes(uint count) {
            byte[] res = new byte[count];
            for (var i = 0; i < count; i++) {
                res[i] = _buffer[_streamPosition++];
            }
            return res;
        }

        /// <summary>
        /// Skipps bytes in stream.
        /// </summary>
        /// <param name="bytes"></param>
        protected override void SkipBytes(uint bytes) {
            _streamPosition += bytes;
        }

        /// <summary>
        /// Reads Int16 from the stream.
        /// </summary>
        /// <returns>Int16 value read from the stream.</returns>
        protected override short ReadInt16() {
            int res = _buffer[_streamPosition++] << 0;
            res = res | _buffer[_streamPosition++] << 8;
            return (short)res;
        }

        /// <summary>
        /// Gets or sets stream position in bytes.
        /// </summary>
        protected override uint StreamPosition {
            get { return _streamPosition; }
            set { _streamPosition = value; }
        }

        /// <summary>
        /// Gets whether stream is ended ot not.
        /// </summary>
        protected override bool IsEndOfStream {
            get { return _streamPosition >= _buffer.Length; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose() {
            _stream.Dispose();
        }
    }
}