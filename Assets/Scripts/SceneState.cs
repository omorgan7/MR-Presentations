using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneState : MonoBehaviour {
	void Awake(){
		DontDestroyOnLoad(gameObject);
	}
	public Object SceneToChangeTo;
}
