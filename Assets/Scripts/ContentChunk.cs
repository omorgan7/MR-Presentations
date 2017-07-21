using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentChunk{

	string tagID{
		get;
	}
	ContentChunk nextChunk{
		get;
		set;
	}

	//data
	float[] timestamps;
	ParseEnums.Instructions[] instructions;
	ParseEnums.SlideType[] slides;
	List<Vector2>[] drawpaths;

	int idx =0;
	int drawidx = 0;
	float starttime= 0f;
	float videooffset;

	//states
	bool changed = true;
	bool playing = false;
	bool infork{
		get;
	} = true;

	SlideController sc;

	//Constructor
	//note that Play() must also be called before Update.
	//though update will simply do nothing if this has not happened.

	public ContentChunk
		(
		float[] timestamps, 
		ParseEnums.Instructions[] instructions, 
		ParseEnums.SlideType[] slides,
		List<Vector2>[] drawpaths, 
		float videooffset,
		string tagID
		)
	{
		this.timestamps = timestamps;
		this.instructions = instructions;
		this.videooffset = videooffset;
		this.tagID = tagID;
		this.drawpaths = drawpaths;
		nextChunk = null;
	}

	public void Play(VideoController vc, SlideController sc){
		starttime= Time.time;
		playing = true;
		vc.PlayVideo(tagID);
		vc.SeekVideo(videooffset);
		this.sc = sc;
	}

	public void SetNextVideo(ContentChunk next){
		nextChunk = next;

	}

	//called every frame by the controller
	//return true if finished playing.

	public bool Update(){
		if(idx < timestamps.Length-1 && playing){
			float elapsedtime = Time.time - starttime;
			if(changed == true){
				ParseInstruction(instructions[idx]);
				changed = false;
			}
			if(elapsedtime >= timestamps[idx+1]){
				changed = true;
				idx++;
			}
			return false;
		}
		return true;
	}

	void ParseInstruction(ParseEnums.Instructions inst){
		switch(inst){
			case ParseEnums.Instructions.slide:
				sc.ChangeSlide(slides[idx]);
			break;
			case ParseEnums.Instructions.draw:
					sc.Draw(slides[idx],drawpaths[drawidx]);
					++drawidx;
				break;
			case ParseEnums.Instructions.clear:
					sc.Clear(slides[idx]);
				break;
			case ParseEnums.Instructions.quiz:
					sc.Quiz(slides[idx]);
				break;
			default:
				break;
		}
	}
}
