using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace FuzzPhyte.Chain.Tests
{
    public class FPSequence_Tests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void FPSequence_TestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator FPSequence_TestsWithEnumeratorPasses()
        {
            //arrange
            SequenceItem sequenceItem = new SequenceItem()
            {
                DataReference = SequenceDetails.CreateInstance("Unit Test", "Test Chapter"),
            };
            yield return new WaitForEndOfFrame();
            //set up sequence
            Assert.AreEqual(expected: sequenceItem.Status, SequenceStatus.None);
            //setup to test all enums we have
        }
    }

}
