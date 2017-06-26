using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSpawner : MonoBehaviour {

	public GameObject ButtonPrefab;
	public int NumButtons;
	// Use this for initialization
	void Start () {
		CreateButtonGrid(NumButtons);
	}
	
	// Update is called once per frame
	void Update () {
	}
	void CreateButtonGrid(int NumButtons){
		if(NumButtons > 9 | NumButtons < 1){
			return;
		}
		int NumRows = NumButtons/3;
		int NumExtraCols = NumButtons % 3;
		NumExtraCols += NumExtraCols == 0 ? 3:0;

		print(NumRows);
		print(NumExtraCols);
		int i = 0;
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
	}
	void SpawnButton(float x, float y){
		GameObject ButtonClone = Instantiate(ButtonPrefab);
		ButtonClone.transform.parent = gameObject.transform;
		RectTransform rt = ButtonClone.GetComponent<RectTransform>();
		rt.anchoredPosition3D = Vector3.zero;
		ButtonClone.transform.localScale = Vector3.one;
		rt.anchorMin = new Vector2(x,y);
		rt.anchorMax = new Vector2(x,y);
	}
}
