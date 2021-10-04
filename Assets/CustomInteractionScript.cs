using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

[RequireComponent(typeof(InteractionBehaviour))]
public class CustomInteractionScript : MonoBehaviour {

	private InteractionBehaviour _intObj;

	// Use this for initialization
	void Start () {
		_intObj = GetComponent<InteractionBehaviour> ();
	}
	//use InteractionBehavior class/api to get stuff i can use for this also https://github.com/leapmotion/UnityModules/wiki/Scripting-Interaction-Objects
	// Update is called once per frame
	void Update () {
		
	}
}

//on that github link look at bottom thingy about Applying forces to interaction object. might be very important