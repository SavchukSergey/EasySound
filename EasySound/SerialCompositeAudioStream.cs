using System.Collections.Generic;
using System.Linq;

namespace EasySound {
    /// <summary>
    /// Represents composite audio stream which can play several audio streams one after another.
    /// </summary>
    public class SerialCompositeAudioStream : CompositeAudioStream {
        /// <summary>
        /// Creates an instance of SerialCompositeAudioStream.
        /// </summary>
        /// <param name="streams">Streams to be added.</param>
        public SerialCompositeAudioStream(params AudioStream[] streams) {
            AddStreams(streams);
        }
        /// <summary>
        /// Adds audio stream to the list.
        /// </summary>
        /// <param name="stream">Stream to be added.</param>
        public override void AddStream(AudioStream stream) {
            base.AddStream(stream);
            SamplesCount = (uint)(_items.Sum(item => item.Length) / SampleDuration);
        }

        private Queue<AudioStream> _pipe;
        private AudioStream _currentItem;
        private double _currentItemEnd;
        private double _relativePosition;

        /// <summary>
        /// Reads next available sample.
        /// </summary>
        /// <param name="sample">Sample to be filled with data.</param>
        public override void ReadSample(out Sample sample) {
            if (_pipe == null) {
                _pipe = new Queue<AudioStream>();
                foreach (var item in AdjustedStreams) {
                    _pipe.Enqueue(item);
                }
                _currentItem = null;
                _relativePosition = 0;
                _currentItemEnd = 0;
            }
            while (_currentItem == null || _position >= _currentItemEnd) {
                if (_pipe.Count == 0) {
                    ReadNullSample(out sample);
                    _pipe = null;
                    return;
                }
                _currentItem = _pipe.Dequeue();
                _currentItemEnd = _position + _currentItem.Length;
                _relativePosition = 0;
            }

            GetSample(_currentItem, out sample);
        }

        private void GetSample(AudioStream stream, out Sample sample) {
            stream.ReadSample(out sample);
            sample.Time = Position;
            _position += SampleDuration;
            _relativePosition += SampleDuration;
        }
    }
}