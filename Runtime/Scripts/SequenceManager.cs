using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
namespace FuzzPhyte.Chain
{
    public class SequenceManager
    {
        /// <summary>
        /// Singleton Setup
        /// </summary>
        private static SequenceManager instance;
        public static SequenceManager Manager
        {
            get
            {
                if(instance == null)
                {
                    instance = new SequenceManager();
                }
                return instance;
            }
        }
        public delegate void EventHandler(SequenceArgument message);
        #region Event Definitions

        public event EventHandler<SequenceArgument> SequenceUpdate;
        public event EventHandler<SequenceArgument> SequenceStart;
        public event EventHandler<SequenceArgument> SequenceEnd;
        /// <summary>
        /// When we have an update to our sequence
        /// </summary>
        /// <param name="message">sequence message details for processing</param>
        public void CallSequenceUpdate(SequenceArgument message)
        {
            SequenceUpdate?.Invoke(this, message);
        }
        /// <summary>
        /// When we have a sequence start/setup
        /// </summary>
        /// <param name="message">sequence message details for starting</param>
        public void CallSequenceStart(SequenceArgument message)
        {
            SequenceStart?.Invoke(this, message);
        }
        /// <summary>
        /// When we have a sequence end/finished
        /// </summary>
        /// <param name="message">sequence message details for ending</param>
        public void CallSequenceEnd(SequenceArgument message)
        {
            SequenceEnd?.Invoke(this, message);
        }
        #endregion
        /// <summary>
        /// Find all sequences and set them up from data
        /// Must be called to setup our sequences and must be called during Unity Awake
        /// </summary>
        public void AwakeFindSequences()
        {
            //find all SequenceItems...
            var allSequenceItems = UnityEngine.Object.FindObjectsOfType(typeof(SequenceItem)) as SequenceItem[];
            var listSequence = allSequenceItems.ToList();
            //only want sequences that have a data file
            var sequenceClean = listSequence.Where(x => x.DataReference != null).ToList();
            //process each sequence and update the associated link next sequences and/or required sequences to the unity runtime references
            if (sequenceClean.Count > 0)
            {
                for (int i = 0; i < sequenceClean.Count; i++)
                {
                    var aSequence = sequenceClean[i];
                    var aSequenceName = aSequence.DataReference.SequenceUniqueName;
                    var theUnlockList = aSequence.DataReference.SequenceUnlocks;
                    var theRequiredList = aSequence.DataReference.SequenceRequirements;
                   
                    for (int j = 0; j < theUnlockList.Count; j++)
                    {
                        var aUnlockedName = theUnlockList[j];
                        if (sequenceClean.Any(ls => ls.DataReference.SequenceUniqueName == aUnlockedName))
                        {
                            aSequence.NextSequences.Add(sequenceClean.FirstOrDefault(ls=>ls.DataReference.SequenceUniqueName== aUnlockedName));
                        }
                    }
                    for(int k=0; k < theRequiredList.Count; k++)
                    {
                        var aRequiredName = theRequiredList[k];
                        if (sequenceClean.Any(rl => rl.DataReference.SequenceUniqueName == aRequiredName))
                        {
                            aSequence.RequiredSequences.Add(sequenceClean.FirstOrDefault(rl=>rl.DataReference.SequenceUniqueName== aRequiredName));
                        }
                    }
                    aSequence.DelayAwake();
                }
            }    
        }
    }
}
