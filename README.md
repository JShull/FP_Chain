# Fuzzphyte Unity Tools

## Chain

Chain is designed and built to help coordinate sequence based logic that occurs in most simple games. To help decouple this tool from specific game patterns, to provide built-in testing, and to provide custom editor scripts this tool is being built with education in mind.

## Setup & Design

Chain is designed to be driven by a data file and finalized in the editor. Please see the ScriptableObjects class called 'SequenceDetails'. Please install the samples when setting up this package within the Unity package manager. The concept is to eventually allow users to build simple sequences all from data and all to be generated and setup at runtime. In the unreleased version 0.1.0 we are getting the initial foundational pieces put together.

### Software Architecture

There are currently four major parts to the Chain tool.

* Sequence Data in the form of ScriptableObjects under the 'SequenceDetails.cs' file
  * This data file represents all of the information and potential sequence connections we need. To do this currently agnostic of types/classes we are using unique naming conventions to look for matches upon initialization
* Sequence Components in Unity that are MonoBehaviours 'SequenceItem'
* Sequence Firing Components in Unity that are MonoBehaviors 'FireSequence'
* Static class that SequenceManager.cs uses a custom eventHandler via a delegate to setup and notify all potential sequences through a derived custom class from EventArgs
  * The Sequence Manager is really in charge of two major pieces:
    * the initial setup and parsing of the SequenceDetails to populate our Unity Components and then the matching and finding of relative Unity GameObjects with SequenceItem components and their associated in game Unity GameObject next sequences and/or required sequences.
    * the eventhandler to allow other components to be notified of updates and or exchanges
      * A custom class derived from EventArgs, SequenceArgument.cs, is all of the information needed for when we need to either look for our next sequence, notify other sequences of a completion, and/or end everything and is passed in the Events associated with our SequenceManager.

To help lay the groundwork for the concept of general sequences there is an internal simple state machine that every sequence could go through. This state machine is more/less the core assumptions built into this architecture and design. As the chain tool scales up we might have to revisit this concept but for the time being there is a controlled flow to the current states and their transitions.

* Current States
  * None
    * Can only go to Unlock and we never come back to this state
    * Primary Use: Initialization and Setup
  * Unlock
    * Can go to Active or Locked
    * Primary Use: Stand-by, primed and ready to do something
  * Active
    * can go to locked or finished
    * Primary Use: this sequence is engaged and in the middle of doing
  * Locked
    * Can only go to unlocked
  * Finished
    * Can go no where, we never come back to this state
    * Primary Use: finish up what we need to do before we either destroy it, turn it off, or leave it to the ether.

As we go through each state there are already built in Unity Events that can be activated or left alone. This was utilized to allow an easier way to add new/additional content outside of the data file and/or linked to other gameobjects/components in the scene. The ScriptableObject data file also gives you a way to override and/or ignore these if needed.

In this current version we require a SequenceDetails ScriptableObject data class for each possible sequence. Make sure to use unique naming convention and follow the comments in the SequenceDetails.cs.

Once you have a Scriptable Object 'SequenceDetails.cs' file for your sequence you can then create a gameobject in Unity and attach the SequenceItems.cs component to the gameobject. From here the custom editor scripts should help demonstrate what you have to do.

In order to enact and/or activate/fire one of these sequences see the FireSequence.cs for an example of how you could do this. You will also notice that this component has some higher level options built-in aka if you want to setup your sequence and then immediately activate it when Unity starts you can do this with any FireSequence.cs component - but you can only do this on one of them. The Editor Scripts are going to double check and make sure you're aware if you have multiple FireSequence.cs initializations. This will eventually be modified later to include the Chapter information as right now we are not querying/filtering against the Chapter parameters within the SequenceDetails information.

## Dependencies

Please see the package.json file for more information.

## License Notes

* This software running a dual license
* Most of the work this repository holds is driven by the development process from the team over at Unity3D :heart: to their never ending work on providing fantastic documentation and tutorials that have allowed this to be born into the world.
* I personally feel that software and it's practices should be out in the public domain as often as possible, I also strongly feel that the capitalization of people's free contribution shouldn't be taken advantage of.
  * If you want to use this software to generate a profit for you/business I feel that you should equally 'pay up' and in that theory I support strong copyleft licenses.
  * If you feel that you cannot adhere to the GPLv3 as a business/profit please reach out to me directly as I am willing to listen to your needs and there are other options in how licenses can be drafted for specific use cases, be warned: you probably won't like them :rocket:

### Educational and Research Use MIT Creative Commons

* If you are using this at a Non-Profit and/or are you yourself an educator and want to use this for your classes and for all student use please adhere to the MIT Creative Commons License
* If you are using this back at a research institution for personal research and/or funded research please adhere to the MIT Creative Commons License
  * If the funding line is affiliated with an [SBIR](https://www.sbir.gov) be aware that when/if you transfer this work to a small business that work will have to be moved under the secondary license as mentioned below.

### Commercial and Business Use GPLv3 License

* For commercial/business use please adhere by the GPLv3 License
* Even if you are giving the product away and there is no financial exchange you still must adhere to the GPLv3 License

## Contact

* [John Shull](mailto:the.john.shull@gmail.com)
* [Twitter](https://twitter.com/TheJohnnyFuzz)