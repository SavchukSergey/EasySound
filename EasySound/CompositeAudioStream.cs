using System.Collections.Generic;
using System.Linq;

namespace EasySound {
    /// <summary>
    /// Repleasents base class for composite audio stream.
    /// </summary>
    public abstract class CompositeAudioStream : AudioStream {

        /// <summary>
        /// Audio items to be used as source.
        /// </summary>
        protected readonly IList<AudioStream> _items = new List<AudioStream>();

        private ushort _bitsPerSample;
        private ushort _numOfChannels;

        private IList<AudioStream> _adjustedItems;

        /// <summary>
        /// Adds audio streams to the list.
        /// </summary>
        /// <param name="streams">Streams to be added.</param>
        public void AddStreams(params AudioStream[] streams) {
            foreach (var stream in streams) {
                AddStream(stream);
            }
        }

        /// <summary>
        /// Adds audio stream to the list.
        /// </summary>
        /// <param name="stream">Stream to be added.</param>
        public virtual void AddStream(AudioStream stream) {
            _items.Add(stream);
            _bitsPerSample = _items.Max(item => item.BitsPerSample);
            _numOfChannels = _items.Max(item => item.ChannelsCount);
            SampleRate = _items.Max(item => item.SampleRate);
            _adjustedItems = null;
        }

        /// <summary>
        /// Gets list of audio streams with have adapted sample rate.
        /// </summary>
        protected IList<AudioStream> AdjustedStreams {
            get {
                if (_adjustedItems == null) {
                    _adjustedItems = new List<AudioStream>();
                    foreach (var item in _items) {
                        var newItem = item.SampleRate == SampleRate
                                          ? item
                                          : new SampleRateAdjustmentAudioStream(item, SampleRate)
                            ;
                        _adjustedItems.Add(newItem);
                    }
                }
                return _adjustedItems;
            }
        }
        /// <summary>
        /// Gets count of bits per sample. (16 or 8)
        /// </summary>
        public override ushort BitsPerSample { get { return _bitsPerSample; } }

        /// <summary>
        /// Gets count of channels. (1 for mono, 2 for stereo)
        /// </summary>
        public override ushort ChannelsCount { get { return _numOfChannels; } }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public override void Dispose() {
            foreach (var item in _items) {
                item.Dispose();
            }
        }
    }
}