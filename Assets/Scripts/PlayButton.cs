using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayButton : MonoBehaviour,IInteractive {
	public VideoController vp;
	public void GVRClick(){
		vp.ResumeVideo();
	}
}
