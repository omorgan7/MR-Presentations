using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoController : MonoBehaviour {
	//string[] FileNames = {"Leonardo", "Shia", "Stormtrooper","Ken"};

	public bool userhaspaused = false;
	public bool isDone = false;
	private AudioSource audioSource;
	private GvrAudioSource GVRAS;
	private Transform t;
	private VideoPlayer vp;
	private string videoplaying;
	

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
		GVRAS = gameObject.GetComponent<GvrAudioSource>();
		t = gameObject.GetComponent<Transform>();
		vp = gameObject.GetComponent<VideoPlayer>();
		vp.Prepare();
	}
	
	void Update(){
		isDone = vp.isPlaying;
	}

	public void PlayVideo(VideoEnums.VideoFiles vdir,string vfile){
		if(vfile == videoplaying){
			return;
		}
		videoplaying = vfile;
		StopVideo();
		vp.clip = Resources.Load("Videos/"+vdir.ToString()+"/"+vfile) as VideoClip;
		VideoClip clip = vp.clip;
		float aspectRatio = (float)clip.width/(float)clip.height;
		transform.localScale = new Vector3(t.localScale.y * aspectRatio, t.localScale.y,t.localScale.z);
		vp.EnableAudioTrack(0, true);
    	vp.SetTargetAudioSource(0, audioSource);
		vp.isLooping = false;
		GVRAS.clip = audioSource.clip;
		ResumeVideo();
	}
	public void StopVideo(){
		vp.Stop();
		if(audioSource !=null){
			audioSource.Stop();
		}
		if(GVRAS !=null){
			GVRAS.Stop();
		}
	}
	public void PauseVideo(){
		userhaspaused = true;
		vp.Pause();
		if(audioSource !=null){
			audioSource.Pause();
		}
		
		if(GVRAS !=null){
			GVRAS.Pause();
		}
	}
	public void ResumeVideo(){
		vp.Play();
		if(audioSource!=null){
			audioSource.Play();
		}
		if(GVRAS !=null){
			GVRAS.Play();
		}
		userhaspaused = false;
	}
	public void SeekVideo(float seektime){
		//this is needed because if the video hasn't loaded yet
		//then the framerate is zero and it won't seek.
		IEnumerator Videoseeker = _SeekVideo(seektime);
		StartCoroutine(Videoseeker);
	}

	IEnumerator _SeekVideo(float seektime){
		while(vp.isPlaying == false){
			yield return null;
		}
		int framenumber = (int) (vp.frameRate * seektime);
		vp.frame = framenumber;
	}
}
