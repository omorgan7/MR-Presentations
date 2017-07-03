using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoController : MonoBehaviour {
	//string[] FileNames = {"Leonardo", "Shia", "Stormtrooper","Ken"};
	private AudioSource audioSource;
	private GvrAudioSource GVRAS;
	private Transform t;
	private VideoPlayer vp;
	private VideoEnums.VideoFiles videoplaying;
	

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
		GVRAS = gameObject.GetComponent<GvrAudioSource>();
		t = gameObject.GetComponent<Transform>();
		vp = gameObject.GetComponent<VideoPlayer>();
		vp.Prepare();
	}
	public void PlayVideo(VideoEnums.VideoFiles vfile){
		if(vfile == videoplaying){
			return;
		}
		StopVideo();
		vp.clip = Resources.Load("Videos/"+vfile.ToString()) as VideoClip;
		VideoClip clip = vp.clip;
		float aspectRatio = (float)clip.width/(float)clip.height;
		transform.localScale = new Vector3(t.localScale.y * aspectRatio, t.localScale.y,t.localScale.z);
		vp.EnableAudioTrack(0, true);
    	vp.SetTargetAudioSource(0, audioSource);
		ResumeVideo();
		videoplaying = vfile;
	}
	public void StopVideo(){
		vp.Stop();
		audioSource.Stop();
		if(GVRAS !=null){
			GVRAS.Stop();
		}
	}
	public void PauseVideo(){
		vp.Pause();
		audioSource.Pause();
		if(GVRAS !=null){
			GVRAS.Pause();
		}
	}
	public void ResumeVideo(){
		vp.Play();
		audioSource.Play();
		if(GVRAS !=null){
			GVRAS.Play();
		}
	}
}
