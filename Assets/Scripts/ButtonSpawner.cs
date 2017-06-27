using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpawner : MonoBehaviour {

	public GameObject ButtonPrefab;

	public List<List<string> > quizanswers;
	
	private GameObject[] ButtonClones;
	private int NumButtons;
	private int quizIndex = 0;
	private int buttoncount = 0;

	// Use this for initialization
	void Start () {
		QuizParser QP = gameObject.GetComponent<QuizParser>();
		quizanswers = QP.quizanswers;
	}

	public void CreateButtonGrid(){
		NumButtons = quizanswers[quizIndex].Count;
		if(NumButtons > 9 | NumButtons < 1 | quizIndex == quizanswers.Count){
			return;
		}
		DestroyButtons();
		ButtonClones = new GameObject[NumButtons];
		int NumRows = NumButtons/3;
		int NumExtraCols = NumButtons % 3;
		int i = 0;
		buttoncount = 0;
		float yCoord, xCoord;
		do{
			yCoord = 0.4f*i;
			for(int j = 0; j<3; j++){
				xCoord = 0.2f + j*0.3f;
				SpawnButton(xCoord,yCoord);
			}
			i++;
		}while(i<NumRows-1);
		yCoord = 0.4f*i;
		float start, offset;
		start = NumExtraCols == 1 ? 0.5f : 0.2f;  
		offset = NumExtraCols == 2 ? 0.15f : 0f;
		for(i = 0; i<NumExtraCols; i++){
			xCoord = start + offset + i*0.3f;
			SpawnButton(xCoord,yCoord);
		}
		quizIndex++;
	}
	void SpawnButton(float x, float y){
		ButtonClones[buttoncount] = Instantiate(ButtonPrefab);
		ButtonClones[buttoncount].transform.SetParent(gameObject.transform,false);
		RectTransform rt = ButtonClones[buttoncount].GetComponent<RectTransform>();
		rt.anchorMin = new Vector2(x,y);
		rt.anchorMax = new Vector2(x,y);
		ButtonClones[buttoncount].GetComponentInChildren<Text>().text = quizanswers[quizIndex][buttoncount];
		buttoncount++;
	}
	void DestroyButtons(){
		if(ButtonClones == null){
			return;
		}
		buttoncount = 0;
		foreach(GameObject b in ButtonClones){
			Destroy(b);
		}
	}
}
