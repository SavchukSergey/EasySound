# EasySound

Easy sound is a .net tool for managing audio stream in the memory. It joins existing wav streams into single one using different methods.

You can join streams to play in parallel, subsequently, with time delay and different amplitudes.

Almost all classes represents wrapper concept where one audio stream takes another stream as a source and applyies some modification.

See example below

```c#
AudioStream stream1 = new WavAudioStream(fileStream1); //read wav stream from fileStream1
AudioStream stream2 = new WavAudioStream(fileStream2); //read wav stream from fileStream2
AudioStream stream3 = new WavAudioStream(fileStream3); //read wav stream from fileStream3

stream1 = new VolumeAdjustmentAudioStream(stream1, 0.25); //making more silent
stream2 = new DelayedAudioStream(stream2, 2, 5); //Making delay before playing stream2
stream2 = new TrimmedAudioStream(stream2, 10, 50); // There is unneeded pause in the start and in the end. Skip it

AudioStream stream4 = new ParallelCompositeAudioStream(stream1, stream2); // play stream1 and stream2 in parallel 

AudioStream stream5 = new SerialCompositeAudioStream(stream3, stream4); // play stream3 and stream4 subsequently

stream5.SaveWav(fileStream5); // Save result to the output stream
```
# Usage

## WavAudioStream

WavAudioStream provides basic wav file reading functionality. Make sure you don't close underlying stream until you finished waork with WavAudioStream.

## WavFullReadAudioStream

Works in the same as WavAudioStream but reads all the stream at the start so you can dispose underlying connection immediately.

## VolumeAdjustmentAudioStream

Changes the applitude of source stream.

## SerialCompositeAudioStream

Stream that takes several other streams and plays them subsequently

## ParallelCompositeAudioStream

Stream that takes several other streams and plays them in parallel


# Console Utility
You can also use console utility to generate file providing to it xml file.
