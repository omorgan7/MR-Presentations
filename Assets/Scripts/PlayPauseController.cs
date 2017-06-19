using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayPauseController : MonoBehaviour {

	public GameObject play;
	public GameObject pause;
	public VideoPlayer vp;

	private Renderer playRend;
	private Renderer pauseRend;

	void Start(){
		play = play.transform.GetChild(0).gameObject;
		pause = pause.transform.GetChild(0).gameObject;

		playRend = play.GetComponent<Renderer>();
		pauseRend = pause.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(vp.isPlaying){
			playRend.material.SetColor("_Color",Color.green);
			pauseRend.material.SetColor("_Color",Color.red);
		}
		else{
			playRend.material.SetColor("_Color",Color.red);
			pauseRend.material.SetColor("_Color",Color.green);
		}
	}
}
