using System.Linq;

namespace EasySound {
    /// <summary>
    /// Represents composite audio stream which can play several audio streams simultaeously.
    /// </summary>
    public class ParallelCompositeAudioStream : CompositeAudioStream {

        /// <summary>
        /// Creates an instance of ParallelCompositeAudioStream
        /// </summary>
        /// <param name="streams">Streams to be added.</param>
        public ParallelCompositeAudioStream(params AudioStream[] streams) {
            AddStreams(streams);
        }

        /// <summary>
        /// Adds audio stream to the list.
        /// </summary>
        /// <param name="stream">Stream to be added.</param>
        public override void AddStream(AudioStream stream) {
            base.AddStream(stream);
            double len = _items.Max(item => item.SamplesCount * item.SampleDuration);
            SamplesCount = (uint)(len * SampleRate);
        }

        /// <summary>
        /// Reads next available sample.
        /// </summary>
        /// <param name="sample">Sample to be filled with data.</param>
        public override void ReadSample(out Sample sample) {
            sample.Time = Position;
            double totalLeft = 0.0;
            double totalRight = 0.0;
            for (int i = 0; i < AdjustedStreams.Count; i++) {
                var stream = AdjustedStreams[i];
                Sample subSample;
                stream.ReadSample(out subSample);
                totalLeft += subSample.Left;
                totalRight += subSample.Right;

            }
            sample.Left = (short)totalLeft;
            sample.Right = (short)totalRight;
            _position += SampleDuration;
        }
    }
}