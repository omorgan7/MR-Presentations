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
		QuizParser QP = gameObject.transform.parent.gameObject.GetComponent<QuizParser>();
		quizanswers = QP.quizanswers;
	}

	public void CreateButtonGrid(){
		NumButtons = quizanswers[quizIndex].Count;
		if(NumButtons > 6 | NumButtons < 2 | quizIndex == quizanswers.Count){
			return;
		}
		DestroyButtons();
		ButtonClones = new GameObject[NumButtons];
		buttoncount = 0;
		float yCoord =0.5f, xCoord;
		float spacer = 1.0f/((float)NumButtons + 1f);
		for(int i = 0; i<NumButtons; i++){
			xCoord=(i+1)*spacer;
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
	public void DestroyButtons(){
		if(ButtonClones == null){
			return;
		}
		buttoncount = 0;
		foreach(GameObject b in ButtonClones){
			Destroy(b);
		}
	}
}
