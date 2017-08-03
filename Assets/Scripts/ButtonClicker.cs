using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClicker : MonoBehaviour {
	SlideController slidecontroller;
	string response;
	void Start(){
		string name = transform.parent.name;
		slidecontroller = GameObject.Find("Screens/Slides").GetComponent<SlideController>();
		response = gameObject.GetComponentInChildren<Text>().text;
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
	public void TaskOnClick(){
		slidecontroller.RecieveAnswer(response.ToLower());
		transform.parent.gameObject.GetComponent<ButtonSpawner>().DestroyButtons();
	}
}
