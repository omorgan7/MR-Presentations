using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestionMarkController : MonoBehaviour, IInteractive {
	
	
	public int SceneIndex = 2; //hope that the tutorial is at one.
	public FadeController fadecontroller;
	private SceneState scenestate;

	void Start(){
		var temp = GameObject.Find("SceneStatePreserver");
		if(temp != null){
			scenestate = temp.GetComponent<SceneState>();
		}
	}

	public void GVRClick(){
		if(scenestate != null){
			fadecontroller.FadeOut();
			StartCoroutine(Fade());
		}

	}
	IEnumerator Fade(){
		while(fadecontroller.isDone == false){
			yield return null;
		}
		scenestate.SceneIndex = SceneIndex;
		SceneManager.LoadScene(0);
	}
}
