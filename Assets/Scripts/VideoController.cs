using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour {

	
	// Use this for initialization
	void Start () {
		Transform t = gameObject.GetComponent<Transform>();
		VideoPlayer vp = gameObject.GetComponent<VideoPlayer>();
		vp.clip = Resources.Load("Shia") as VideoClip;
		print(vp.clip);
		VideoClip clip = vp.clip;
		float aspectRatio = (float)clip.width/(float)clip.height;
		t.localScale = new Vector3(t.localScale.y * aspectRatio, t.localScale.y,t.localScale.z);
	}
}
