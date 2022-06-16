using System;
using UnityEngine;
namespace FuzzPhyte.Chain
{
    /// <summary>
    /// Core 'status' for all things sequence related
    /// Will be used heavily across sequence logic
    /// </summary>
    [Serializable]
    [SerializeField]
    public enum SequenceStatus
    {
        None = 0,
        Locked = 1,
        Unlocked = 2,
        Active = 3,
        Finished = 4,
    }
    /// <summary>
    /// Unity's weird way of referencing an enum as a passed component
    /// </summary>
    public class SequenceStatusComponent : Component
    {
        public SequenceStatus Status;
    }
    /// <summary>
    /// Core 'placeholder' for all things firing of sequence to help
    /// with monobehavior and the editor to help with the FireSequence.cs
    /// </summary>
    [Serializable]
    [SerializeField]
    public enum FireSequenceEventType
    {
        StateUpdate =0,
        Start = 1,
        End = 2
    }
    /// <summary>
    /// Unity's weird way of referencing an enum as a passed component
    /// </summary>
    public class FireSequenceComponent : Component
    {
        public FireSequenceEventType EventType;
    }
    /// <summary>
    /// Root Utility class for enums and/or other misc. needs associated with this package
    /// </summary>
    public static class Utility 
    {
        private readonly static char[] invalidFilenameChars;
        private readonly static char[] invalidPathChars;
        private readonly static char[] parseTextImagefileChars;

        static Utility()
        {
            invalidFilenameChars = System.IO.Path.GetInvalidFileNameChars();
            invalidPathChars = System.IO.Path.GetInvalidPathChars();
            parseTextImagefileChars = new char[1] { '~' };
            Array.Sort(invalidFilenameChars);
            Array.Sort(invalidPathChars);
        }
        /// <summary>
        /// Return color by status
        /// Aligns with our editor script
        /// Don't use this method if we are referencing editor scripts
        /// </summary>
        /// <param name="status">Sequence state/status</param>
        /// <returns></returns>

        public static Color ReturnColorByStatus(SequenceStatus status)
        {
            switch (status)
            {
                case SequenceStatus.None:
                    return Color.white;
                case SequenceStatus.Locked:
                    return Color.red;
                case SequenceStatus.Unlocked:
                    return Color.yellow;
                case SequenceStatus.Active:
                    return Color.green;
                case SequenceStatus.Finished:
                    return Color.cyan;
                default:
                    return Color.white;
            }
        }
    }
}
