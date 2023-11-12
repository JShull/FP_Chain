using UnityEngine;
using UnityEngine.Events;

namespace FuzzPhyte.Chain.Samples
{
    public class SequenceFireExample : MonoBehaviour
    {
        [Tooltip("The item we want to talk to when it's time")]
        public FireSequence SequenceFire;
        public FireSequenceEventType FireType;
        public UnityEvent SequenceUpdateAdditional;
        public UnityEvent SequenceStartAdditional;
        public UnityEvent SequenceEndAdditional;

        /// <summary>
        /// Example of activating our FireSequence Event by our current FireType
        /// </summary>
        public void ButtonFireExampleEvent()
        {
            if (SequenceFire != null)
            {
                //SequenceFire
                switch (FireType)
                {
                    case FireSequenceEventType.StateUpdate:
                        SequenceFire.SendUpdateSequenceMessage();
                        SequenceUpdateAdditional.Invoke();
                        break;
                    case FireSequenceEventType.Start:
                        SequenceFire.SendStartSequenceMessage();
                        SequenceStartAdditional.Invoke();
                        break;
                    case FireSequenceEventType.End:
                        SequenceFire.SendEndSequenceMessage();
                        SequenceEndAdditional.Invoke();
                        break;
                }
            }
        }
        /// <summary>
        /// Example of firing an event by passed type
        /// </summary>
        /// <param name="eventT">What is our current sequence fire type?</param>
        public void ButtonFireExampleEvent(FireSequenceComponent eventT)
        {
            FireType = eventT.EventType;
            ButtonFireExampleEvent();
        }
        #region Misc. Testing Functions for added events

        public void MessageSequenceUpdateAdditional(string message)
        {
            Debug.Log($"Update Message: {message}");
        }
        public void MessageSequenceStartAdditional(string message)
        {
            Debug.Log($"Start Message: {message}");
        }
        public void MessageSequenceEndAdditional(string message)
        {
            Debug.Log($"End Message: {message}");
        }
        #endregion

    }

}

