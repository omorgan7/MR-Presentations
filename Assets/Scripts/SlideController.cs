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

	// Use this for initialization
	void Start () {
		InstructionParser IP = transform.parent.parent.gameObject.GetComponent<InstructionParser>();
		if(IP.isDone == false){
			Debug.LogError("Something bad happened to the parser. Are you in parser debug mode?");
			return;
		}
		
		timestamps = IP.timestamps;
		drawingpaths = IP.drawingpaths;
		instruction = IP.instruction;
		SlideOrder = IP.SlideOrder;
		videotoplay = IP.videotoplay;
		tags = IP.tags;

		rend = gameObject.GetComponent<Renderer>();
		mat = rend.material;
		maintexture = rend.material.mainTexture as Texture2D;
		quizcontroller = GameObject.Find("Quizzes/"+Slide.ToString());
		startTime = Time.time;
	}
	
	void Update(){
		if(timestampindex < timestamps.Count-1){
			elapsedTime = Time.time - startTime + timeoffset;
			print("elapsed time is"+elapsedTime);
			print(vc.isDone);
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
				break;
			case ParseEnums.Instructions.tag:
				Video(tagIndex);
				tagIndex++;
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
		Texture slide = Resources.Load("Slides/"+Slide.ToString()+"/"+slideName) as Texture;
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
	void Video(int i){
		vc.PlayVideo(videotoplay,tags[i]);
	}
}
