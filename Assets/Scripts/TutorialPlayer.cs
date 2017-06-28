using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : MonoBehaviour, IInteractive {

	public VideoController vc;

	//this function must be defined to satisfy the interface.
	//the tutorial video doesn't exist yet though: hence it does nothing.
	public void GVRClick(){
		//vc.PlayVideo("Tutorial");
	}

}
