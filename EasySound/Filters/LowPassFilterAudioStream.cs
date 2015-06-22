﻿namespace EasySound.Filters {
    public class LowPassFilterAudioStream : BaseRCFilterAudioStream {

        public LowPassFilterAudioStream(AudioStream innerStream, double freq)
            : base(innerStream, freq) {
        }

        public override void ReadSample(out Sample sample) {
            Sample sourceSample;
            _innerStream.ReadSample(out sourceSample);
            
            var sourceLeft = sourceSample.Left;
            var iLeft = (sourceLeft - _ucLeft) / _resistance;
            var dqLeft = iLeft * _dt;
            _ucLeft += dqLeft / _capacity;
            var outputLeft = _ucLeft;
            sample.Left = (short) outputLeft;

            var sourceRight = sourceSample.Right;
            var iRight = (sourceRight - _ucRight) / _resistance;
            var dqRight = iRight * _dt;
            _ucRight += dqRight / _capacity;
            var outputRight = _ucRight;
            sample.Right = (short)outputRight;

            sample.Time = sourceSample.Time;
        }
    }
}
