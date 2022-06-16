using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FuzzPhyte.Chain
{
    public class FireSequence : MonoBehaviour
    {
        [HideInInspector]
        [Tooltip("Data Reference")]
        public SequenceDetails DataRef;
        [HideInInspector]
        [Tooltip("Is there a local requirement/reference to a sequenceItem?")]
        public SequenceItem PossibleRequirement;
        [HideInInspector]
        [Tooltip("This is overwritten if we have a data file")]
        public string SequenceName;
        [HideInInspector]
        [Tooltip("This is overwritten if we have a data file")]
        public string SequenceChapter;
        [HideInInspector]
        public SequenceStatus LockStatus;
        [HideInInspector]
        [TextArea(4, 8)]
        public string FireSequenceNotes;
        SequenceArgument activationMessage;
        [HideInInspector]
        [Tooltip("Should only be called once at the very beginning to setup everything")]
        public bool AwakeSetupSequences;
        [HideInInspector]
        [Tooltip("If true, activate from Unity Start")]
        public bool FireOnStart;
        [HideInInspector]
        [Tooltip("If we are going to use the data file")]
        public bool UseDataFile;
        [HideInInspector]
        [Tooltip("If we are going to use the requirements file")]
        public bool UseRequirementFile;
        public void OnEnable()
        {
            if (DataRef != null)
            {
                SequenceName = DataRef.SequenceUniqueName;
                SequenceChapter = DataRef.SequenceChapter;
            }
            activationMessage = new SequenceArgument()
            {
                SequenceUniqueName = SequenceName,
                SequenceChapter = SequenceChapter,
                MessageContent = FireSequenceNotes,
                SequenceStatus = LockStatus,
                SequenceRequirement = PossibleRequirement,
            };
        }
        public void OnDisable()
        {
            activationMessage = null;
        }
        public void Awake()
        {
            if (AwakeSetupSequences)
            {
                SequenceManager.Manager.AwakeFindSequences();
            }
        }
        public void Start()
        {
            if (FireOnStart)
            {
                SendUpdateSequenceMessage();
            }
        }
        /// <summary>
        /// Called from any sort of event to then send the current details
        /// used for activating state sequences
        /// </summary>
        public void SendUpdateSequenceMessage()
        {
            SequenceManager.Manager.CallSequenceUpdate(activationMessage);
        }
        /// <summary>
        /// Used if we are starting a sequence
        /// </summary>
        public void SendStartSequenceMessage()
        {
            SequenceManager.Manager.CallSequenceStart(activationMessage);
        }
        /// <summary>
        /// Used if we are ending a sequence
        /// </summary>
        public void SendEndSequenceMessage()
        {
            SequenceManager.Manager.CallSequenceEnd(activationMessage);
        }
    }

}
