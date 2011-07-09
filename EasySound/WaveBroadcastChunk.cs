namespace EasySound {
    /// <summary>
    /// Represents Broadcast Wave format data. See http://www.aesnashville.org/PDFs/Broadcast%20Wave%20Specs.pdf for the reference.
    /// </summary>
    public struct WaveBroadcastChunk {

        /// <summary>
        /// Description of the sound sequence.
        /// </summary>
        public string Description;

        /// <summary>
        /// Name of the originator.
        /// </summary>
        public string Originator;

        /// <summary>
        /// Reference of the originator.
        /// </summary>
        public string OriginatorReference;

        /// <summary>
        /// Origination date yyyy:mm:dd.
        /// </summary>
        public string OriginationDate;

        /// <summary>
        /// Origination time hh-mm-ss.
        /// </summary>
        public string OriginationTime;

        /// <summary>
        /// First sample count since midnight, low word.
        /// </summary>
        public uint TimeReferenceLow;

        /// <summary>
        /// First sample count since midnight, high word.
        /// </summary>
        public uint TimeReferenceHigh;

        /// <summary>
        /// An unsigned binary number giving the version of the BWF, particularly the contents of the Reserved field.
        /// For Version 1, this is set to 0001h.
        /// </summary>
        public ushort Version;

        /// <summary>
        /// 64 bytes containing a UMID (Unique Material Identifier) to the SPMTE 330M standard.
        /// If only a 32 byte basic UMID is used, the last 32 bytes should be set to zero.
        /// (The length of the UMID is given internally.)
        /// </summary>
        public byte[] UMID;

        /// <summary>
        /// 190 bytes reserved for extensions.
        /// If the Version field is set to 0001h, these 190 bytes must be set to a NULL (zero) value.
        /// </summary>
        public byte[] Reserved;

        /// <summary>
        /// Non-restricted ASCII characters, containing a collection of strings terminated by CR/LF.
        /// Each string contains a description of a coding process applied to the audio data.
        /// Each new coding application is required to add a new string with the appropriate information.
        /// </summary>
        public string CodingHistory;

    }
}