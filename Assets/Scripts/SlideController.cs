using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SlideController : MonoBehaviour {

	public GameObject left,right;
	public VideoController vc;

	int leftidx = 0;
	int rightidx =0;

	InstructionParser IP;
	Hashtable ContentDatabase;
	string currenttag = "1";
	ContentChunk current;

	Texture2D[] maintextures = new Texture2D[2];
	Material[] materials = new Material[2];
	ButtonSpawner[] buttonspawners = new ButtonSpawner[2];

	bool isfinished = false;

	// Use this for initialization
	void Start () {
		IP = gameObject.GetComponent<InstructionParser>();
		ContentDatabase = IP.ContentDatabase;
		current = ContentDatabase["1"] as ContentChunk;
		current.Play(vc,this);
		GetTexture(left,(int)ParseEnums.SlideType.left);
		GetTexture(right,(int)ParseEnums.SlideType.right);
		buttonspawners[(int)ParseEnums.SlideType.left] = GameObject.Find("Quizzes/"+ParseEnums.SlideType.left.ToString()).GetComponent<ButtonSpawner>();
		buttonspawners[(int)ParseEnums.SlideType.right] = GameObject.Find("Quizzes/"+ParseEnums.SlideType.right.ToString()).GetComponent<ButtonSpawner>();;
	}

	void Update(){
		
		if(isfinished == false){
			isfinished = current.Update();
		}
		else{
			//recieve quiz input
			//etc.
		}
	}

	void GetTexture(GameObject obj, int idx){
		Renderer rend = obj.GetComponent<Renderer>();
		materials[idx] = rend.material;
		maintextures[idx] = rend.material.mainTexture as Texture2D;
	}

	public void ChangeSlide(ParseEnums.SlideType type){
		//type is confined to 0,1 or undefined, and we check for those cases here.
		switch(type){
			case(ParseEnums.SlideType.right):
				Texture slide = Resources.Load("Slides/"+IP.videotoplay.ToString()+"/"+Slide.ToString()+"/"+rightidx.ToString()) as Texture;
				_ChangeSlide(slide,materials[(int) type],maintextures[(int) type],right);
				++rightidx;
				break;
			case(ParseEnums.SlideType.left):
				Texture slide = Resources.Load("Slides/"+IP.videotoplay.ToString()+"/"+Slide.ToString()+"/"+leftidx.ToString()) as Texture;
				_ChangeSlide(slide,materials[(int) type],maintextures[(int) type],left);
				++leftidx;
				break;
			default:
				return;
		}
	}

	private void _ChangeSlide(Texture slide, Material mat, Texture2D maintexture, GameObject obj){
		mat.SetTexture("_MainTex",slide);
		maintexture = slide as Texture2D;
		float aspectRatio = (float)slide.width/(float)slide.height;
		obj.transform.localScale = new Vector3(obj.transform.localScale.y * aspectRatio, obj.transform.localScale.y,obj.transform.localScale.z);
	}

	public void Quiz(ParseEnums.SlideType type){
		if(type == ParseEnums.SlideType.none){
			return;
		}
		buttonspawners[(int) type].CreateButtonGrid();
	}
	public void Clear(ParseEnums.SlideType type){
		materials[(int) type].SetTexture("_MainTex",maintexture);
	}
	public void Draw(ParseEnums.SlideType type, List<Vector2> drawcoords){
		Texture2D maintextureclone = Instantiate(materials[(int) type].mainTexture) as Texture2D;
		for(int i = 0; i< drawcoords.Count; i++){

			Vector2 coords = drawcoords[i];
			Color[] color = new Color[20*20];
			for(int j = 0; j<20*20; j++){
				color[j] = Color.black;
			}
			maintextureclone.SetPixels((int)coords.x,(int)coords.y,20,20,color,0); 
		}
		maintextureclone.Apply();
		Texture newTexture = maintextureclone as Texture;
		materials[(int) type].SetTexture("_MainTex",newTexture);
	}
	public void RecieveAnswer(string ans){

	}
}
