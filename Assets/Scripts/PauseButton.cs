using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PauseButton : MonoBehaviour,IInteractive {
	public VideoController vp;
	public void GVRClick(){
		vp.PauseVideo();
	}
}
