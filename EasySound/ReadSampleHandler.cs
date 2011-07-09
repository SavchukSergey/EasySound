namespace EasySound {
    /// <summary>
    /// Represents delegate for reading audio samples.
    /// </summary>
    /// <param name="sample">Sample to be filled with data.</param>
    public delegate void ReadSampleHandler(out Sample sample);
}