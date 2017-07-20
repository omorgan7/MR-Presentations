using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneState : MonoBehaviour {
	void Awake(){
		var gameobjects = GameObject.FindGameObjectsWithTag("LoadingState");
		if(gameobjects.Length > 1){
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
	public int SceneIndex;
}
