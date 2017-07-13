using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InstructionParser : MonoBehaviour {

	public List<float> timestamps;
	public List<List<Vector2> > drawingpaths = new List<List<Vector2> >();
	public List<ParseEnums.Instructions> instruction = new List<ParseEnums.Instructions>();
	public List<ParseEnums.SlideType> SlideOrder = new List<ParseEnums.SlideType>();
	public List<string> tags = new List<string>();
	public VideoEnums.VideoFiles videotoplay = VideoEnums.VideoFiles.Tutorial;

	public string SlideScript = "script.txt";
	public bool isDone = false;
	// Use this for initialization
	void Awake () {
		Parse(SlideScript);
		isDone = true;

	}

	void Parse(string slidescriptlocation){
		string line;
		//temp timestamps returns the timestamps in milliseconds since the Unix epoch.
		List<long> temptimestamps = new List<long>(); 
		int i = 0;

		#if UNITY_ANDROID && !UNITY_EDITOR

		string path = "jar:file://" + Application.dataPath + "!/assets/"+slidescriptlocation;
		WWW file = new WWW(path);
		while(file.isDone == false){
		}
		string contents = file.text;
		StringReader reader = new StringReader(contents);

		#else

		StreamReader reader = File.OpenText("Assets/StreamingAssets/"+slidescriptlocation);

		#endif

		while((line = reader.ReadLine()) != null){
			string[] items = line.Split(' ');
			temptimestamps.Add(long.Parse(items[0]));
			instruction.Add((ParseEnums.Instructions) System.Enum.Parse(typeof(ParseEnums.Instructions),items[1]));
			if(instruction[i] == ParseEnums.Instructions.slide || instruction[i] == ParseEnums.Instructions.draw || instruction[i] == ParseEnums.Instructions.clear || instruction[i] == ParseEnums.Instructions.quiz){
				SlideOrder.Add((ParseEnums.SlideType) System.Enum.Parse(typeof(ParseEnums.SlideType),items[2]));
			}
			else{
				SlideOrder.Add(ParseEnums.SlideType.none);
			}
			if(instruction[i] == ParseEnums.Instructions.draw){
				ParseDrawingInstruction(items[4]);
			}
			if(instruction[i] == ParseEnums.Instructions.tag){
				tags.Add(items[2]);
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

	void ParseDrawingInstruction(string sketch){
		string[] delimiter = new string[] {"x","l"};
		string[] result = sketch.Split(delimiter,System.StringSplitOptions.RemoveEmptyEntries);
		drawingpaths.Add(new List<Vector2>());
		for(int i = 0; i<result.Length; i+=2){
			drawingpaths[drawingpaths.Count - 1].Add(new Vector2((float)int.Parse(result[i]),(float)int.Parse(result[i+1])));
		}
	}
}
