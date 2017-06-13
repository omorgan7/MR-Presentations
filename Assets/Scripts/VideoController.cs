using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoController : MonoBehaviour {
	string[] FileNames = {"Leonardo", "Shia", "Stormtrooper","Ken"};
	public enum VideoFiles {Leonardo,Shia,Stormtrooper,Ken};
	public VideoFiles VideoToPlay = VideoFiles.Shia;
	private AudioSource audioSource;
	private GvrAudioSource GVRAS;
	private Transform t;
	private VideoPlayer vp;

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
		GVRAS = gameObject.GetComponent<GvrAudioSource>();
		t = gameObject.GetComponent<Transform>();
		vp = gameObject.GetComponent<VideoPlayer>();
		PlayVideo(VideoToPlay);
	}
	void PlayVideo(VideoFiles vFile){
		StopVideo();
		vp.clip = Resources.Load("Videos/"+FileNames[(int)vFile]) as VideoClip;
		VideoClip clip = vp.clip;
		float aspectRatio = (float)clip.width/(float)clip.height;
		t.localScale = new Vector3(t.localScale.y * aspectRatio, t.localScale.y,t.localScale.z);
		vp.EnableAudioTrack(0, true);
    	vp.SetTargetAudioSource(0, audioSource);
		vp.Play();
		audioSource.Play();
		if(GVRAS !=null){
			GVRAS.Play();
		}
	}
	void StopVideo(){
		vp.Stop();
		audioSource.Stop();
		if(GVRAS !=null){
			GVRAS.Stop();
		}
	}
	void PauseVideo(){
		vp.Pause();
		audioSource.Pause();
		if(GVRAS !=null){
			GVRAS.Pause();
		}
	}
}
