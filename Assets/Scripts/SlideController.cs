using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SlideController : MonoBehaviour {

	public GameObject left,right;
	public VideoController vc;

	InstructionParser IP;
	Hashtable ContentDatabase;
	string currenttag = "1";
	ContentChunk current;

	bool isfinished = false;

	// Use this for initialization
	void Start () {
		IP = gameObject.GetComponent<InstructionParser>();
		ContentDatabase = IP.ContentDatabase;
		current = ContentDatabase["1"] as ContentChunk;
		current.Play(vc,this);
	}

	void Update(){
		
		if(isfinished == false){
			isfinished = current.Update();
		}
		else{
			//recieve quiz input
			//etc.
		}
	}
	public void ChangeSlide(ParseEnums.SlideType type){
		
	}
	public void Quiz(ParseEnums.SlideType type){
		
	}
	public void Clear(ParseEnums.SlideType type){
		
	}
	public void Draw(ParseEnums.SlideType type, List<Vector2> drawcoords){
		
	}
	public void RecieveAnswer(string ans){

	}
}
