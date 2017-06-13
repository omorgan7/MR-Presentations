using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour {

	Renderer rend;
	Material mat;
	Transform transform;
	// Use this for initialization
	void Start () {
		rend = gameObject.GetComponent<Renderer>();
		mat = rend.material;
		transform = gameObject.GetComponent<Transform>();
		ChangeSlide("1");

	}
	
	void ChangeSlide(string slideName){
		Texture slide = Resources.Load("Slides/"+slideName) as Texture;
		mat.SetTexture("_MainTex",slide);
		float aspectRatio = (float)slide.width/(float)slide.height;
		transform.localScale = new Vector3(transform.localScale.y * aspectRatio, transform.localScale.y,transform.localScale.z);
	}
}
