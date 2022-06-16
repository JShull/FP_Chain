using UnityEngine;
using System.Collections.Generic;
using System;
namespace FuzzPhyte.Chain
{
    [Serializable]
    [CreateAssetMenu(fileName = "SquenceDetails", menuName = "ScriptableObjects/FuzzPhyte/Sequence/Details", order = 0)]
    public class SequenceDetails : ScriptableObject
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
        /// List of other sequence names that have to be finished before this can be activated
        /// </summary>
        [Tooltip("List of other sequences that must be finished before this can be activated, if left blank will ignore")]
        public List<string>SequenceRequirements = new List<string>();
        [Tooltip("List of sequences we will go find and unlock when this sequence is finished")]
        public List<string> SequenceUnlocks = new List<string>();
       
        /// <summary>
        /// First Sequence of the Chapter?
        /// </summary>
        public bool FirstSequence;
        /// <summary>
        /// Last Sequence of the Chapter?
        /// </summary>
        public bool LastSequence;
        
        [Tooltip("Are we required to use the Sequence Requirements")]
        public bool UseSequenceRequirements;
        [Tooltip("Sequence goes immediately from Unlock to active")]
        public bool AutomaticUnlockToActive;
        /// <summary>
        /// Notes tied to the sequence 
        /// </summary>
        [TextArea(5, 10)]
        public string SequenceNotes;

        /// <summary>
        /// Initialize Parameters
        /// </summary>
        /// <param name="uniqueName">We need a name</param>
        /// <param name="sequenceChapter">We need a chapter</param>
        public void Init(string uniqueName, string sequenceChapter, bool firstSequence =false, bool lastSequence = false, bool sequenceRequirements=false,bool automaticUnlock = false)
        {
            SequenceUniqueName = uniqueName;
            SequenceChapter = sequenceChapter;
            FirstSequence = firstSequence;
            LastSequence = lastSequence;
            UseSequenceRequirements = sequenceRequirements;
            AutomaticUnlockToActive = automaticUnlock;
        }
        public static SequenceDetails CreateInstance(string name,string chapter)
        {
            var data = ScriptableObject.CreateInstance<SequenceDetails>();
            data.Init(name, chapter);
            return data;
        }
    }

}
