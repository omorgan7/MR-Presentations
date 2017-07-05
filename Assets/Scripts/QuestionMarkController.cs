using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestionMarkController : MonoBehaviour, IInteractive {
	
	public Object tutorialscene;
	public FadeController fadecontroller;
	public void GVRClick(){
		fadecontroller.FadeOut();
		StartCoroutine(Fade());
	}
	IEnumerator Fade(){
		while(fadecontroller.isDone == false){
			yield return null;
		}
		SceneManager.LoadScene(tutorialscene.name);
	}
}
