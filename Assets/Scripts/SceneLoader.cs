using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public Text loadingtext;
	SceneState scenestate;
	AsyncOperation ao;

	void Start(){
		var temp = GameObject.Find("SceneStatePreserver");
		scenestate = temp.GetComponent<SceneState>();
		StartCoroutine(LoadNextLevel());
	}

	// Update is called once per frame
	void Update () {
		loadingtext.color = new Color(loadingtext.color.r, loadingtext.color.g, loadingtext.color.b, Mathf.PingPong(Time.time, 1));
	}
	IEnumerator LoadNextLevel(){
		ao = SceneManager.LoadSceneAsync(scenestate.SceneIndex);
		yield return ao;
	}
}
