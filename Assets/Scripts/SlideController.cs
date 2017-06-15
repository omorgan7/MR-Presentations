using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SlideController : MonoBehaviour {

	public ParseEnums.SlideType Slide = ParseEnums.SlideType.left;
	public VideoPlayer vp; 

	Renderer rend;
	Material mat;
	Texture2D maintexture;
	List<float> timestamps;
	List<List<Vector2> > drawingpaths;
	List<ParseEnums.Instructions> instruction;
	List<ParseEnums.SlideType> SlideOrder;
	int slideNumber = 0;
	float deltaTime = 0f;
	float startTime;
	int timestampindex = 0;
	int drawIndex = 0;
	bool hasChanged = true;
	bool once = false;

	// Use this for initialization
	void Start () {
		InstructionParser IP = transform.parent.parent.gameObject.GetComponent<InstructionParser>();
		// while(IP.isDone == false){
		// 	//wait.
		// }
		timestamps = IP.timestamps;
		drawingpaths = IP.drawingpaths;
		instruction = IP.instruction;
		SlideOrder = IP.SlideOrder;
		rend = gameObject.GetComponent<Renderer>();
		mat = rend.material;
		maintexture = rend.material.mainTexture as Texture2D;
	}
	
	void Update(){
		if(vp.isPlaying == false){
			return;
		}
		if(once == false){
			once = true;
			startTime = Time.time;
		}
		
		if(timestampindex < timestamps.Count-1){
			float elapsedTime = Time.time - startTime;
			if(hasChanged == true){
				ParseInstruction(instruction[timestampindex]);
				hasChanged = false;
			}
			if(elapsedTime >= timestamps[timestampindex+1]){
				hasChanged = true;
				timestampindex++;
			}
		}
	}
	
	void ParseInstruction(ParseEnums.Instructions inst){
		switch(inst){
			case ParseEnums.Instructions.start:
				ChangeSlide(slideNumber.ToString());
				break;
			case ParseEnums.Instructions.stop:
				slideNumber = 0;
				ChangeSlide(slideNumber.ToString());
				break;
			case ParseEnums.Instructions.slide:
				if(SlideOrder[timestampindex] == Slide){
					slideNumber++;
					ChangeSlide(slideNumber.ToString());
				}
				break;
			case ParseEnums.Instructions.video:
				break;
			case ParseEnums.Instructions.draw:
				if(SlideOrder[timestampindex] == Slide){
					Draw(slideNumber.ToString());
					drawIndex++;
				}
				break;
			case ParseEnums.Instructions.clear:
				break;
		}
	}

	void ChangeSlide(string slideName){
		Texture slide = Resources.Load("Slides/"+Slide.ToString()+"/"+slideName) as Texture;
		mat.SetTexture("_MainTex",slide);
		maintexture = slide as Texture2D;
		float aspectRatio = (float)slide.width/(float)slide.height;
		transform.localScale = new Vector3(transform.localScale.y * aspectRatio, transform.localScale.y,transform.localScale.z);
	}

	void Draw(string slideName){
		Texture2D maintexture = Resources.Load("Slides/"+Slide.ToString()+"/"+slideName) as Texture2D;

		for(int i = 0; i< drawingpaths[drawIndex].Count; i++){
			Vector2 coords = drawingpaths[drawIndex][i];
			Color[] color = new Color[20*20];
			for(int j = 0; j<20*20; j++){
				color[j] = Color.black;
			}
			maintexture.SetPixels((int)coords.x,(int)coords.y,20,20,color,0); 
		}
		maintexture.Apply();
		Texture newTexture = maintexture as Texture;
		mat.SetTexture("_MainTex",newTexture);

	}

}
