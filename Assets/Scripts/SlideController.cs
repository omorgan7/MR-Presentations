using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class SlideController : MonoBehaviour {

	Renderer rend;
	Material mat;
	Transform transform;
	enum Instructions {start, stop, slide, video, draw, clear};
	List<float> timestamps;
	List<long> temptimestamps;
	List<Instructions> instruction = new List<Instructions>();
	int slideNumber = 0;
	float deltaTime = 0f;
	float startTime;
	int timestampindex = 0;
	bool hasChanged = true;
	bool once = false;

	public enum SlideType {left, right, none};
	public SlideType Slide = SlideType.left;
	public string SlideScript = "script.txt";
	public VideoPlayer vp; 

	private List<SlideType> SlideOrder = new List<SlideType>();

	// Use this for initialization
	void Start () {
		ParseSlideScript(SlideScript);
		rend = gameObject.GetComponent<Renderer>();
		mat = rend.material;
		transform = gameObject.GetComponent<Transform>();
		// foreach(long s in temptimestamps){
		// 	print(s);
		// }
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
	
	void ChangeSlide(string slideName){
		Texture slide = Resources.Load("Slides/"+Slide.ToString()+"/"+slideName) as Texture;
		mat.SetTexture("_MainTex",slide);
		float aspectRatio = (float)slide.width/(float)slide.height;
		transform.localScale = new Vector3(transform.localScale.y * aspectRatio, transform.localScale.y,transform.localScale.z);
	}
	
	void ParseSlideScript(string slidescriptlocation){
		StreamReader reader = File.OpenText("Assets/Resources/Slides/"+slidescriptlocation);
		string line;
		//temp timestamps returns the timestamps in milliseconds since the Unix epoch.
		temptimestamps = new List<long>(); 
		int i = 0;
		while((line = reader.ReadLine()) != null){
			string[] items = line.Split(' ');
			temptimestamps.Add(long.Parse(items[0]));
			instruction.Add((Instructions) System.Enum.Parse(typeof(Instructions),items[1]));
			if(instruction[i] == Instructions.slide || instruction[i] == Instructions.draw || instruction[i] == Instructions.clear){
				SlideOrder.Add((SlideType) System.Enum.Parse(typeof(SlideType),items[2]));
			}
			else{
				SlideOrder.Add(SlideType.none);
			}
			i++;
		}
		timestamps = new List<float>();
		timestamps.Add(0f);
		for(int j=1; j < temptimestamps.Count; j++){
			temptimestamps[j] -= temptimestamps[0];
			timestamps.Add((float)temptimestamps[j] / 1000f);
		}
	}

	void ParseInstruction(Instructions inst){
		switch(inst){
			case Instructions.start:
				ChangeSlide(slideNumber.ToString());
				break;
			case Instructions.stop:
				slideNumber = 0;
				ChangeSlide(slideNumber.ToString());
				break;
			case Instructions.slide:
				if(SlideOrder[timestampindex] == Slide){
					slideNumber++;
					ChangeSlide(slideNumber.ToString());
				}
				break;
			case Instructions.video:
				break;
			case Instructions.draw:
				break;
			case Instructions.clear:
				break;
		}
	}

}
