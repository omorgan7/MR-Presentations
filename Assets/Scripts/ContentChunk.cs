using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContentChunk{

	public string tagID;
	public ContentChunk nextChunk;


	//data
	float[] timestamps;
	ParseEnums.Instructions[] instructions;
	ParseEnums.SlideType[] slides;
	List<Vector2>[] drawpaths;

	int idx =-1;
	int drawidx = 0;
	float starttime= 0f;
	float videooffset;
	float elapsedtime = 0f;

	//states
	bool changed = false;

	SlideController sc;

	//Constructor
	//note that Play() must also be called before Update.
	//though update will simply do nothing if this has not happened.

	public ContentChunk
		(
		float[] _timestamps, 
		ParseEnums.Instructions[] _instructions, 
		ParseEnums.SlideType[] _slides,
		List<Vector2>[] _drawpaths, 
		float _videooffset,
		string _tagID
		)
	{
		videooffset = _videooffset;
		timestamps = _timestamps;
		for(int i = 0; i<timestamps.Length; i++){
			timestamps[i] -= videooffset;
		}
		instructions = _instructions;
		tagID = _tagID;
		drawpaths = _drawpaths;
		nextChunk = null;
		slides = _slides;
	}

	public void Play(VideoController vc, SlideController _sc){
		sc = _sc;
		vc.StartCoroutine(DelayedPlay(vc));
		IEnumerable temp = instructions.Zip(timestamps,(first,second) => first + " " + second);
		foreach(var i in temp){
			Debug.Log(i);
		}
		Debug.Log(tagID);
	}

	IEnumerator DelayedPlay(VideoController vc){
		while(vc.hasStarted == false){
			yield return null;
		}
		vc.PlayVideo(tagID);
		vc.SeekVideo(videooffset);
		Debug.Log(videooffset);
		starttime= Time.time;
	}

	public void SetNextVideo(ContentChunk next){
		nextChunk = next;
	}

	//called every frame by the controller
	//return true if finished playing.

	public bool UpdateState(float time){
		if(idx+1 < timestamps.Length){
			elapsedtime += time;
			if(elapsedtime >= timestamps[idx+1]){
				changed = true;
			}
			if(changed == true){
				++idx;
				ParseInstruction(instructions[idx]);
				changed = false;
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
			case ParseEnums.Instructions.stop:
				sc.Stop();
				break;
			default:
				break;
		}
	}
}
