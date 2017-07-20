using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SlideController : MonoBehaviour {

	public ParseEnums.SlideType Slide = ParseEnums.SlideType.left;
	public VideoController vc;

	VideoEnums.VideoFiles videotoplay;
	GameObject quizcontroller;
	Renderer rend;
	Material mat;
	Texture2D maintexture;
	List<float> timestamps;
	List<List<Vector2> > drawingpaths;
	List<ParseEnums.Instructions> instruction;
	List<ParseEnums.SlideType> SlideOrder;
	InstructionParser IP;
	List<string> tags;
	int slideNumber = 1;
	float startTime;
	int timestampindex = 0;
	int drawIndex = 0;
	int tagIndex = 0;
	bool hasChanged = true;
	float elapsedTime = 0f;
	float timeoffset = 0f;
	bool userhaspaused = false;
	bool loaded = false;
	bool invideofork = false;
	string currenttag;
	string searchpattern = "([a-f])";

	// Use this for initialization
	void Start () {
		rend = gameObject.GetComponent<Renderer>();
		mat = rend.material;
		maintexture = rend.material.mainTexture as Texture2D;
		quizcontroller = GameObject.Find("Quizzes/"+Slide.ToString());
		IP = transform.parent.parent.gameObject.GetComponent<InstructionParser>();
		StartCoroutine(GetInstructions());
	}

	void AssignVariables(){
		timestamps = IP.timestamps;
		drawingpaths = IP.drawingpaths;
		instruction = IP.instruction;
		SlideOrder = IP.SlideOrder;
		videotoplay = IP.videotoplay;
		tags = IP.tags;
		startTime = Time.time;
	}

	void Update(){
		if(loaded == false){
			return;
		}
		if(timestampindex < timestamps.Count-1){
			elapsedTime = Time.time - startTime + timeoffset;
			if(hasChanged == true){
				ParseInstruction(instruction[timestampindex]);
				hasChanged = false;
			}
			if(elapsedTime >= timestamps[timestampindex+1]){
				hasChanged = true;
				timestampindex++;
			}
		}
		if(vc.userhaspaused || vc.isDone == false){
			timeoffset -= Time.deltaTime;
		}
	}

	IEnumerator GetInstructions(){
		while(IP.isDone == false){
			yield return null;
		}
		AssignVariables();
		loaded = true;
	}
	
	void ParseInstruction(ParseEnums.Instructions inst){
		switch(inst){
			case ParseEnums.Instructions.video:
				NextInstruction();
				break;
			case ParseEnums.Instructions.start:
				ChangeSlide(slideNumber.ToString());
				vc.SeekVideo(timestamps[timestampindex]);
				vc.ResumeVideo();
				break;
			case ParseEnums.Instructions.stop:
				vc.StopVideo();
				startTime = Time.time;
				timeoffset = 0f;
				if(invideofork){
					NextInstruction();
				}
				break;
			case ParseEnums.Instructions.tag:
				if(invideofork){
					//while in a forked tag, search for the next non-forked tag
					do{
						currenttag = tags[tagIndex];
						tagIndex++;
					}
					while(System.Text.RegularExpressions.Regex.IsMatch(currenttag,searchpattern));
					invideofork = false;
				}
				else{
					currenttag = tags[tagIndex];
					tagIndex++;
				}
				Video();
				NextInstruction();
				break;
			default:
				break;
		}
		if(SlideOrder[timestampindex] == Slide){
			switch(inst){
				case ParseEnums.Instructions.slide:
					slideNumber++;
					ChangeSlide(slideNumber.ToString());
				break;
			case ParseEnums.Instructions.draw:
					Draw(slideNumber.ToString());
					drawIndex++;
				break;
			case ParseEnums.Instructions.clear:
					Clear();
				break;
			case ParseEnums.Instructions.quiz:
					Quiz();
				break;
			default:
				break;
			}
		}
	}

	void NextInstruction(){
		timeoffset += timestamps[timestampindex+1]-timestamps[timestampindex]; //goto next instruction
	}

	void ChangeSlide(string slideName){
		Texture slide = Resources.Load("Slides/"+IP.videotoplay.ToString()+"/"+Slide.ToString()+"/"+slideName) as Texture;
		mat.SetTexture("_MainTex",slide);
		maintexture = slide as Texture2D;
		float aspectRatio = (float)slide.width/(float)slide.height;
		transform.localScale = new Vector3(transform.localScale.y * aspectRatio, transform.localScale.y,transform.localScale.z);
	}

	void Draw(string slideName){
		Texture2D maintextureclone = Instantiate(mat.mainTexture) as Texture2D;
		for(int i = 0; i< drawingpaths[drawIndex].Count; i++){

			Vector2 coords = drawingpaths[drawIndex][i];
			Color[] color = new Color[20*20];
			for(int j = 0; j<20*20; j++){
				color[j] = Color.black;
			}
			maintextureclone.SetPixels((int)coords.x,(int)coords.y,20,20,color,0); 
		}
		maintextureclone.Apply();
		Texture newTexture = maintextureclone as Texture;
		mat.SetTexture("_MainTex",newTexture);
	}

	void Clear(){
		mat.SetTexture("_MainTex",maintexture);
	}
	void Quiz(){
		quizcontroller.SendMessage("CreateButtonGrid");
	}
	void Video(){
		vc.PlayVideo(videotoplay,currenttag);
	}
	public void RecieveAnswer(string ans){
		string newtag = currenttag + ans;
		int i;
		for(i = tagIndex; i<tags.Count; i++){
			if(tags[i].Contains(currenttag) && tags[i].Contains(ans)){
				break;
			}
		}
		currenttag = tags[i];
		//HACK HERE
		//assumption that we're calling this command whilst on a stop.
		int j;
		int tagcount = 0;
		for(j=timestampindex; j<instruction.Count; ++j){
			if(instruction[j] == ParseEnums.Instructions.tag){
				++tagcount;
			}
			if(tagcount == i-tagIndex){
				break;
			}
		}
		//j now contains the precise instruction to jump to.
		timestampindex = j;
		tagIndex = i;
		ParseInstruction(ParseEnums.Instructions.tag);
		invideofork = true;
	}
}
