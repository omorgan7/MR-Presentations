using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InstructionParser : MonoBehaviour {

	public VideoEnums.VideoFiles videotoplay = VideoEnums.VideoFiles.Tutorial;
	public Hashtable ContentDatabase;

	public string SlideScript = "script.txt";
	public bool isDone = false;

	// Use this for initialization
	void Start () {
		Parse(SlideScript);
		isDone = true;

	}

	//this function is filthy
	//turn back now before it's too late
	//it is well commented though, just a beastly function.
	//google would not approve.

	void Parse(string slidescriptlocation){
		List<float> timestamps;
		List<List<Vector2> > drawingpaths = new List<List<Vector2> >();
		List<ParseEnums.Instructions> instruction = new List<ParseEnums.Instructions>();
		List<ParseEnums.SlideType> SlideOrder = new List<ParseEnums.SlideType>();
		List<string> tags = new List<string>();
		string line;
		//temp timestamps returns the timestamps in milliseconds since the Unix epoch.
		List<long> temptimestamps = new List<long>(); 
		int i = 0;

		//android needs to decompress files, have to pretend it's a url for the file system
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
			
			//certain instructions have a left or right associated with them, we need to store this.
			if(instruction[i] == ParseEnums.Instructions.slide || instruction[i] == ParseEnums.Instructions.draw || instruction[i] == ParseEnums.Instructions.clear || instruction[i] == ParseEnums.Instructions.quiz){
				SlideOrder.Add((ParseEnums.SlideType) System.Enum.Parse(typeof(ParseEnums.SlideType),items[2]));
			}
			else{
				SlideOrder.Add(ParseEnums.SlideType.none);
			}

			//read the xy data and store it.
			if(instruction[i] == ParseEnums.Instructions.draw){
				ParseDrawingInstruction(items[4],drawingpaths);
			}

			//tags are very important, and match the lecture to the filmed content.
			if(instruction[i] == ParseEnums.Instructions.tag){
				tags.Add(items[2]);
			}
			i++;
		}

		//normalise each timestamp by the one denoted by the video tag.
		timestamps = new List<float>();
		long timestampoffset = 0;
		for(int j=0; j < temptimestamps.Count; j++){
			if(instruction[j] == ParseEnums.Instructions.video){
				timestampoffset = temptimestamps[j];
			}
			temptimestamps[j] -= timestampoffset;
			timestamps.Add((float)temptimestamps[j] / 1000f);
		}

		//this very large section converts all the data into ContentChunk objects.
		//each contentchunk is a self contained manager for what happens in that specific bit of the video.
		//the data needs to be sorted and fed into the constructor for each object.
		//it's then put in a hash table with the key denoted as the tag for each piece of content.

		int index = -1;
		int startindex = 0;
		int endindex = 0;
		int drawstartindex = 0;
		int drawendindex = 0;
		float videooffset = 0f;
		int tagindex = -1;

		//number of tags is the number of pieces of content we have.
		for(int j=0; j<tags.Count; j++){
			bool drawonce = false;
			do{
				++index;
				if(instruction[index] == ParseEnums.Instructions.start){
					videooffset = timestamps[index];
				}
				bool addcondition = instruction[index] == ParseEnums.Instructions.start || instruction[index] == ParseEnums.Instructions.video || instruction[index] == ParseEnums.Instructions.tag;
				
				//this could break horribly if there's a random start/tag/video tag in the middle of the set of instructions.
				//there shouldn't be: a todo would be to verify it i suppose.

				if(addcondition){
					startindex = index;
					++startindex; //start from the next one.
				}
				if(instruction[index] == ParseEnums.Instructions.tag){
					++tagindex;
				}
				if(instruction[index] == ParseEnums.Instructions.draw){
					if(drawonce == false){
						drawstartindex = index;
						drawendindex = drawstartindex;
						drawonce = true;
					}
					else{
						++drawendindex;
					}
				}	

			} while (instruction[index] != ParseEnums.Instructions.stop);
			int arrlength = index - startindex;
			int drawarrlength = drawendindex - drawstartindex +1;

			//create temporary arrays to feed into the ContentChunk
			float[] times = new float[arrlength];
			ParseEnums.Instructions[] tempinsts = new ParseEnums.Instructions[arrlength];
			List<Vector2>[] tempdrawings = new List<Vector2>[drawarrlength];
			ParseEnums.SlideType[] tempslides = new ParseEnums.SlideType[arrlength];

			timestamps.CopyTo(startindex,times,0,arrlength);
			instruction.CopyTo(startindex,tempinsts,0,arrlength);
			SlideOrder.CopyTo(startindex,tempslides,0,arrlength);
			drawingpaths.CopyTo(drawstartindex,tempdrawings,0,drawarrlength);

			ContentChunk content = new ContentChunk(times,tempinsts,tempslides,tempdrawings,videooffset,tags[tagindex]);
			if(content.tagID.Length == 1){
				ContentDatabase.Add(tags[tagindex],content);
			}
			else{
				for(int k = 1;k<content.tagID.Length; k++){
					string key = content.tagID[0].ToString() + content.tagID[k].ToString();
					ContentDatabase.Add(key,content);
				}
			}
		}
	}

	void ParseDrawingInstruction(string sketch,List<List<Vector2> > drawingpaths){
		string[] delimiter = new string[] {"x","l"};
		string[] result = sketch.Split(delimiter,System.StringSplitOptions.RemoveEmptyEntries);
		drawingpaths.Add(new List<Vector2>());
		for(int i = 0; i<result.Length; i+=2){
			drawingpaths[drawingpaths.Count - 1].Add(new Vector2((float)int.Parse(result[i]),(float)int.Parse(result[i+1])));
		}
	}
}
