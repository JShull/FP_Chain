using System;
using FuzzPhyte.Utility;
namespace FuzzPhyte.Chain
{
    /// <summary>
    /// Derived eventargs class for delegate and event handler code
    /// </summary>
    public class SequenceArgument : EventArgs
    {
        /// <summary>
        /// This must be unique across all sequences
        /// </summary>
        public string SequenceUniqueName;
        /// <summary>
        /// If we need to associate multiple unique sequences as part of a larger sequence
        /// </summary>
        public string SequenceChapter;
        /// <summary>
        /// Sequence requirement reference
        /// </summary>
        public SequenceItem SequenceRequirement;
        /// <summary>
        /// Status to check against for sequences
        /// </summary>
        public SequenceStatus SequenceStatus;
        /// <summary>
        /// Message Details
        /// </summary>
        public string MessageContent;
    }

}
