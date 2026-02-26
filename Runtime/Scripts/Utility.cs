namespace FuzzPhyte.Chain
{
    using System;
    using UnityEngine;
    using FuzzPhyte.Utility;

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
}
