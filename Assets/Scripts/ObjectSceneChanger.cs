using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectSceneChanger : MonoBehaviour, IInteractive {
	
	public enum Scenes {Loading=0,Tutorial=1,Lecture=2};
	public Scenes SceneToLoad = Scenes.Lecture; //hope that the tutorial is at one.
	public FadeController fadecontroller;
	private SceneState scenestate;

	void Start(){
		var temp = GameObject.Find("SceneStatePreserver");
		if(temp != null){
			scenestate = temp.GetComponent<SceneState>();
		}
	}

	public void GVRClick(){
		print("click");
		if(scenestate != null){
			StartCoroutine(Fade());
		}

	}
	IEnumerator Fade(){
		if(scenestate.SceneIndex != (int) SceneToLoad){
			fadecontroller.FadeOut();
			while(fadecontroller.isDone == false){
				yield return null;
			}
			scenestate.SceneIndex = (int) SceneToLoad;
			SceneManager.LoadScene(0);
		}
		
	}
}
