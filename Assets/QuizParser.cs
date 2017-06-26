using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuizParser : MonoBehaviour {

	public string quizloc = "quiz.txt";
	public bool isDone = false;
	public bool debug = true;

	public List<List<string> > quizanswers = new List<List<string> >();

	// Use this for initialization
	void Awake () {
		ParseQuizScript(quizloc);
		isDone = true;

	}
	void ParseQuizScript(string quizloc){
		if(debug == true){
			elseParse(quizloc);
		}
		else{
			AndroidParse(quizloc);
		}
	}

	void AndroidParse(string quizloc){
		string line;
		string path = "jar:file://" + Application.dataPath + "!/assets/"+quizloc;
		WWW file = new WWW(path);
		while(file.isDone == false){
		}
		//StartCoroutine(FinishDownload(path));
		string contents = file.text;
		StringReader strReader = new StringReader(contents);
		int i = 0;
		while((line = strReader.ReadLine()) != null){
			quizanswers.Add(new List<string>());
			string[] answers = line.Split('/');
			foreach(string answer in answers){
				quizanswers[i].Add(answer);
			}
			i++;
		}

	}

	void elseParse(string quizloc){
		string line;
		StreamReader reader = File.OpenText("Assets/StreamingAssets/"+quizloc);
		int i =0;
		while((line = reader.ReadLine()) != null){
			quizanswers.Add(new List<string>());
			string[] answers = line.Split('/');
			foreach(string answer in answers){
				quizanswers[i].Add(answer);
			}
			i++;
		}
	}
}
