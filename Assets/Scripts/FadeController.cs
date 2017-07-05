using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour {

	public float duration = 1f;
	public bool isDone = false;

	Renderer renderer;
	float timestart = 0f;
	enum Fade {Black, White,Nothing};
	Color startcolor;
	float startduration;
	Fade fade;


	void Start () {
		startduration = duration;
		renderer = gameObject.GetComponent<Renderer>();
		FadeIn();
	}

	public void Update(){
		switch(fade){
			case Fade.White:
				FadeTo(startcolor,new Color(0f,0f,0f,0f));
				break;
			case Fade.Black:
				FadeTo(startcolor,Color.black);
				break;
			default:
				break;
		}
	}

	void FadeTo(Color from, Color to){
		float lerp = (Time.time - timestart)/duration;
		if(lerp >1f){
			isDone = true;
			fade = Fade.Nothing;
			duration = startduration;
			return;
		}
		renderer.material.SetColor("_Color",Color.Lerp(from,to,lerp));
	}

	public void FadeIn(){
		fade = Fade.White;
		FadeInit();
	}
	public void FadeIn(float dur){
		duration = dur;
		FadeIn();
	}
	public void FadeOut(float dur){
		duration = dur;
		FadeOut();
	}
	public void FadeOut(){
		fade = Fade.Black;
		FadeInit();
	}
	void FadeInit(){
		isDone = false;
		startcolor = renderer.material.GetColor("_Color");
		timestart = Time.time;
	}
}
