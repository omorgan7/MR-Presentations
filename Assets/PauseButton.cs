using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PauseButton : MonoBehaviour,IInteractive {
	public GameObject vp;
	public void GVRClick(){
		vp.GetComponent<VideoController>().PauseVideo();
	}
}
