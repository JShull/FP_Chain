using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;
namespace FuzzPhyte.Chain
{
    [Serializable]
    public class SequenceItem : MonoBehaviour
    {
        [Header("Sequence Details")]
        public SequenceDetails DataReference;
        [Tooltip("Humble Object Representation")]
        private SequenceLogic sequenceSystem;
        public SequenceLogic SequenceSystem {
            get {return sequenceSystem;} 
            set {sequenceSystem = value;}
        }

        [Tooltip("Sequence State - don't change")]
        [SerializeField]
        private SequenceStatus sequenceStatus;
        public SequenceStatus Status
        {
            get { return sequenceStatus; }
            set { sequenceStatus = value; }
        }
        [Tooltip("If we have some in our data file these will be automatically built out")]
        public List<SequenceItem>NextSequences = new List<SequenceItem>();
        [Tooltip("If we have some in our data file these will be automatically built out")]
        public List<SequenceItem> RequiredSequences = new List<SequenceItem>();
        //[HideInInspector]
        //[Tooltip("Falls right from Unlock to active")]
        //public bool AutoActivate;
        [HideInInspector]
        [Tooltip("Editor based")]
        public bool UseEvents;
        [HideInInspector]
        [Tooltip("Editor based")]
        public bool UseUnlockEvent;
        [HideInInspector]
        [Tooltip("Editor based")]
        public bool UseLockEvent;
        [HideInInspector]
        [Tooltip("Editor based")]
        public bool UseActiveEvent;
        [HideInInspector]
        [Tooltip("Editor based")]
        public bool UseEndEvent;
        public UnityEvent UnlockEvent;
        [Tooltip("When we are locked: lock state")]
        public UnityEvent LockEvent;
        [Tooltip("When we begin our sequence: active state")]
        public UnityEvent ActiveEvent;
        [Tooltip("When we end our sequence: finished state")]
        public UnityEvent EndEvent;

        public void DelayAwake()
        {
            //humble setup
            if (DataReference != null)
            {
                SequenceSystem = new SequenceLogic(this.gameObject.name, DataReference.SequenceUniqueName);
            }
            else
            {
                SequenceSystem = new SequenceLogic(this.gameObject.name, "Blank");
            }

            SequenceSystem.AddReplaceEventState(SequenceStatus.Unlocked, UnlockEvent, false);
            SequenceSystem.AddReplaceEventState(SequenceStatus.Locked, LockEvent, false);
            SequenceSystem.AddReplaceEventState(SequenceStatus.Active, ActiveEvent, false);
            SequenceSystem.AddReplaceEventState(SequenceStatus.Finished, EndEvent, false);
            //need to figure out what and who is going to own these sequences...
            for (int i = 0; i < NextSequences.Count; i++)
            {
                SequenceSystem.AddNextSequenceItem(NextSequences[i]);
            }
            for (int j = 0; j < RequiredSequences.Count; j++)
            {
                SequenceSystem.AddNextSequenceItem(RequiredSequences[j]);
            }
        }
        /// <summary>
        /// Called from another event or our own invoked event
        /// Called from a UI reference
        /// Derive this class and you can then modify on the setup 
        /// </summary>
        public virtual void SetupSequence()
        {
            if (DataReference.FirstSequence)
            {
                SequenceManager.Manager.CallSequenceUpdate(new SequenceArgument()
                {
                    SequenceUniqueName = this.DataReference.SequenceUniqueName,
                    SequenceChapter = this.DataReference.SequenceChapter,
                    SequenceStatus = SequenceStatus.Unlocked,
                    MessageContent = $"Object: {this.gameObject.name}, unique sequence name: {this.DataReference.SequenceUniqueName}",
                });
            }
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            Vector3 centerP = transform.position;
            // List<Vector3> endPoints = new List<Vector3>();
            Color curColor = Utility.ReturnColorByStatus(Status);
            for (int i = 0; i < NextSequences.Count; i++)
            {
                if (NextSequences[i] != null)
                {
                    Vector3 nextS = NextSequences[i].transform.position;
                    //endPoints.Add(nextS);
                    Vector3 startTan = new Vector3(centerP.x, centerP.y + 1 + (i * 2f), centerP.z);

                    Vector3 forwardV = (nextS - startTan).normalized;
                    UnityEditor.Handles.DrawBezier(centerP, nextS - (forwardV * 0.25f), startTan, nextS, curColor, null, 2f);

                    UnityEditor.Handles.color = curColor;
                    UnityEditor.Handles.ConeHandleCap(0, nextS - (forwardV * 0.25f), Quaternion.LookRotation(forwardV), 0.25f, EventType.Repaint);
                }
            }
            for (int j = 0; j < RequiredSequences.Count; j++)
            {
                if (RequiredSequences[j] != null)
                {
                    Vector3 nextS = RequiredSequences[j].transform.position;
                    //endPoints.Add(nextS);
                    Vector3 startTan = new Vector3(centerP.x, centerP.y + 1 + (j * 2f), centerP.z);

                    Vector3 forwardV = (nextS - startTan).normalized;
                    Color fromColor = Utility.ReturnColorByStatus(RequiredSequences[j].Status);
                    UnityEditor.Handles.DrawBezier(centerP, nextS - (forwardV * 0.25f), startTan, nextS, fromColor, null, 2f);

                    UnityEditor.Handles.color = fromColor;
                    UnityEditor.Handles.DrawSolidDisc(nextS - (forwardV * 0.25f), forwardV, 0.25f);
                }
            }
#endif
        }
        /// <summary>
        /// Help with debugging sequences
        /// </summary>

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (DataReference == null)
            {
                Gizmos.DrawIcon(transform.position, "/FP/Chain/seqItem_error.png", true);
                return;
            }
            switch (sequenceStatus)
            {
                case SequenceStatus.None:
                    Gizmos.DrawIcon(transform.position, "/FP/Chain/seqItem.png", true);
                    break;
                case SequenceStatus.Locked:
                    Gizmos.DrawIcon(transform.position, "/FP/Chain/seqItem_locked.png", true);
                    break;
                case SequenceStatus.Unlocked:
                    Gizmos.DrawIcon(transform.position, "/FP/Chain/seqItem_unlocked.png", true);
                    break;
                case SequenceStatus.Active:
                    Gizmos.DrawIcon(transform.position, "/FP/Chain/seqItem_active.png", true);
                    break;
                case SequenceStatus.Finished:
                    Gizmos.DrawIcon(transform.position, "/FP/Chain/seqItem_finished.png", true);
                    break;
            }
            
#endif
        }
        /// <summary>
        /// Sequence Item is based on a strict set of states
        /// Modify and adjust these at your own risks - but other items will break.
        /// </summary>
        #region Core Function of Sequence Classes
        private void OnEnable()
        {
            if (SequenceSystem != null)
            {
                SequenceManager.Manager.SequenceUpdate += (sender, e) => OnUpdateCheckCallback(sender, e);
                SequenceManager.Manager.SequenceStart += (sender, e) => OnStartCheckCallback(sender, e);
                SequenceManager.Manager.SequenceEnd += (sender, e) => OnEndCheckCallback(sender, e);
            }
            
            if (DataReference == null)
            {
                this.gameObject.SetActive(false);
                this.enabled = false;
                
            }
        }
        private void OnDisable()
        {
            if (SequenceSystem != null)
            {
                SequenceManager.Manager.SequenceUpdate -= (sender, e) => OnUpdateCheckCallback(sender, e);
                SequenceManager.Manager.SequenceStart -= (sender, e) => OnStartCheckCallback(sender, e);
                SequenceManager.Manager.SequenceEnd -= (sender, e) => OnEndCheckCallback(sender, e);
            }
            
        }
        #region Callbacks
        private void OnUpdateCheckCallback(object sender, SequenceArgument response)
        {
            //if I'm the sequence that's called do my thing - activate whatever I need to activate
            //change my status
            if (DataReference == null)
            {
                return;
            }
            if (response.SequenceUniqueName == DataReference.SequenceUniqueName)
            {
               
                //Debug.LogWarning($"{this.gameObject.name} is currently {sequenceStatus}, getting passed {response.SequenceStatus}");
                switch (sequenceStatus)
                {
                    case SequenceStatus.None:
                        switch (response.SequenceStatus)
                        {
                            case SequenceStatus.Unlocked:
                                if (DataReference.UseSequenceRequirements)
                                {
                                    //get the status of all the previous requirements
                                    var allFinished = RequiredSequences.Where(a => a.Status == SequenceStatus.Finished).ToList();
                                    if (allFinished.Count == RequiredSequences.Count)
                                    {
                                        //all have finished
                                        //unlocked
                                        InternalStateProcedure(UseUnlockEvent, SequenceStatus.Unlocked);
                                    }
                                }
                                else
                                {
                                    InternalStateProcedure(UseUnlockEvent, SequenceStatus.Unlocked);

                                }
                                CheckAutoActivate();
                                break;
                        }
                        break;
                    case SequenceStatus.Unlocked:
                        switch (response.SequenceStatus)
                        {
                            case SequenceStatus.Locked:
                                InternalStateProcedure(UseLockEvent, SequenceStatus.Locked);
                                break;
                            case SequenceStatus.Active:
                                InternalStateProcedure(UseActiveEvent, SequenceStatus.Active);
                                break;
                        }
                        break;
                    case SequenceStatus.Active:
                        switch (response.SequenceStatus)
                        {
                            case SequenceStatus.Finished:
                                InternalStateProcedure(UseEndEvent, SequenceStatus.Finished);
                                break;
                            case SequenceStatus.Locked:
                                InternalStateProcedure(UseLockEvent, SequenceStatus.Locked);
                                break;
                        }
                        break;
                    case SequenceStatus.Locked:
                        switch (response.SequenceStatus)
                        {
                            case SequenceStatus.Unlocked:
                                //continue
                                InternalStateProcedure(UseUnlockEvent, SequenceStatus.Unlocked);
                                break;
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// internal call from user setting the option to go immediately from Unlock to Active without having to pass a secondary State update
        /// </summary>
        private void CheckAutoActivate()
        {
            if (DataReference == null)
            {
                return;
            }
            if (DataReference.AutomaticUnlockToActive && sequenceStatus == SequenceStatus.Unlocked)
            {
                sequenceStatus = SequenceStatus.Active;
                if (UseActiveEvent)
                {
                    ActiveEvent.Invoke();
                }
            }
        }
        /// <summary>
        /// Callback for when we receive a OnStartCheckCallback
        /// We don't care about the state information being passed
        /// Only care if our sequenceStatus is None or Unlocked to progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response">Sequence Argument information, if we have a match with our data ref we proceed</param>
        private void OnStartCheckCallback(object sender, SequenceArgument response)
        {
            if (DataReference == null)
            {
                return;
            }
            if(response.SequenceUniqueName == DataReference.SequenceUniqueName)
            {
                switch (sequenceStatus)
                {
                    case SequenceStatus.None:
                        InternalStateProcedure(UseUnlockEvent, SequenceStatus.Unlocked);
                        CheckAutoActivate();
                        break;
                    case SequenceStatus.Unlocked:
                        InternalStateProcedure(UseActiveEvent, SequenceStatus.Active);
                        break;
                }
            }
        }
        /// <summary>
        /// Callback for when we receive a OnEndCheck from our Manager
        /// We dont care about the state information being passed in response
        /// Only care if our sequenceState is Active and we match the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response">Sequence Argument information, if we have a match with our data ref we proceed</param>
        private void OnEndCheckCallback(object sender, SequenceArgument response)
        {
            if(DataReference == null)
            {
                return;
            }
            if(response.SequenceUniqueName == DataReference.SequenceUniqueName)
            {
                switch (sequenceStatus)
                {
                    case SequenceStatus.Active:
                        InternalStateProcedure(UseEndEvent, SequenceStatus.Finished);
                        
                        if (DataReference.LastSequence)
                        {
                            Debug.LogWarning($"Last Item: Sequence Chapter: {this.DataReference.SequenceChapter}, {this.DataReference.SequenceUniqueName}");
                        }
                        break;
                }
            }
        }
        
        /// <summary>
        /// Called when we are finished
        /// Mainly responsible for activating next sequences
        /// </summary>
        private void FinishedActivateNextSequences()
        {
            for (int i = 0; i < NextSequences.Count; i++)
            {
                var nextSequence = NextSequences[i];
                nextSequence.gameObject.SetActive(true);
                SequenceManager.Manager.CallSequenceUpdate(new SequenceArgument()
                {
                    SequenceUniqueName = nextSequence.DataReference.SequenceUniqueName,
                    SequenceChapter = nextSequence.DataReference.SequenceChapter,
                    SequenceStatus = SequenceStatus.Unlocked,
                    MessageContent = $"Object: {this.gameObject.name}, sequence name:{this.DataReference.SequenceUniqueName}"
                });
            }
        }
        
        /// <summary>
        /// Function to modify the sequence status and fire off events if needed
        /// Really shouldnt be messed with :)
        /// </summary>
        /// <param name="useEvent">Editor boolean to fire event by state</param>
        /// <param name="status">state we are going to be setting to</param>
        private protected void InternalStateProcedure(bool useEvent, SequenceStatus status)
        {
            sequenceStatus = status;
            //process chain sequences  before invoking unity event handlers
            /*
            if (sequenceStatus == SequenceStatus.Finished)
            {
                FinishedActivateNextSequences();
            }
            */
            SequenceSystem.InternalStateProcedure(sequenceStatus);
            //if we are using the added events, invoke them now
            if (useEvent)
            {
                var eventsToInvoke = SequenceSystem.ReturnStateEvent(sequenceStatus);
                for(int i=0;i< eventsToInvoke.Count; i++)
                {
                    eventsToInvoke[i].Invoke();
                }
                /*
                switch (status)
                {
                    case SequenceStatus.Locked:
                        LockEvent.Invoke();
                        break;
                    case SequenceStatus.Unlocked:
                        UnlockEvent.Invoke();
                        break;
                    case SequenceStatus.Active:
                        ActiveEvent.Invoke();
                        break;
                    case SequenceStatus.Finished:
                        EndEvent.Invoke();
                        break;
                }
                */
            }
        }

        #endregion
        #endregion
    }

    /// <summary>
    /// Humble Object
    /// Class to hold Sequence Item Systems and Logic
    /// </summary>
    public class SequenceLogic
    {
        private string GObjectName;
        private string DataRefUniqueName;
        private Dictionary<SequenceStatus, List<UnityEvent>> sequenceEvents;
        
        private List<SequenceItem> nextSequences = new List<SequenceItem>();
        //[Tooltip("If we have some in our data file these will be automatically built out")]
        private List<SequenceItem> requiredSequences = new List<SequenceItem>();
        public SequenceLogic(string GameObjectName, string UniqueDataName)
        {
            sequenceEvents = new Dictionary<SequenceStatus, List<UnityEvent>>();
            GObjectName = GameObjectName;
            DataRefUniqueName = UniqueDataName;
        }
        public void AddNextSequenceItem(SequenceItem theItem)
        {
            nextSequences.Add(theItem);
        }
        public void AddRequiredSequence(SequenceItem theItem)
        {
            requiredSequences.Add(theItem);
        }
        /// <summary>
        /// Update and/or replace our event list by state
        /// </summary>
        /// <param name="status"></param>
        /// <param name="newEvents"></param>
        /// <param name="replace"></param>
        public void AddReplaceEventState(SequenceStatus status, List<UnityEvent>newEvents,bool replace)
        {
            
            if (sequenceEvents.TryGetValue(status, out List<UnityEvent>val))
            {
                if (replace)
                {
                    sequenceEvents[status] = newEvents;
                }
                else
                {
                    List<UnityEvent> combined = val.Concat(newEvents).ToList();
                    sequenceEvents[status] = combined;
                }
            }
            else
            {
                sequenceEvents.Add(status, newEvents);
            }
        }
        public void AddReplaceEventState(SequenceStatus status, UnityEvent newEvent, bool replace)
        {
            if (sequenceEvents.TryGetValue(status, out List<UnityEvent>val))
            {
                if (replace)
                {
                    sequenceEvents[status] = new List<UnityEvent>() { newEvent };
                }
                else
                {
                    val.Add(newEvent);
                    sequenceEvents[status] = val;
                }
            }
            else
            {
                sequenceEvents.Add(status, new List<UnityEvent>() { newEvent });
            }
        }
        public void InternalStateProcedure(SequenceStatus status)
        {
            if(status == SequenceStatus.Finished)
            {
                for (int i = 0; i < nextSequences.Count; i++)
                {
                    var nextSequence = nextSequences[i];
                    nextSequence.gameObject.SetActive(true);
                    SequenceManager.Manager.CallSequenceUpdate(new SequenceArgument()
                    {
                        SequenceUniqueName = nextSequence.DataReference.SequenceUniqueName,
                        SequenceChapter = nextSequence.DataReference.SequenceChapter,
                        SequenceStatus = SequenceStatus.Unlocked,
                        MessageContent = $"Object: {GObjectName}, sequence name:{DataRefUniqueName}"
                    });
                }
            }
        }
        public List<UnityEvent> ReturnStateEvent(SequenceStatus status)
        {
            if(sequenceEvents.TryGetValue(status,out List<UnityEvent> val))
            {
                return val;
            }
            else
            {
                return null;
            }
        }
    }
}
