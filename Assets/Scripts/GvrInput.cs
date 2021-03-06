﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GvrInput : MonoBehaviour {
	
	float zPos;
	Vector3 end;
	Vector3 goal;
	GameObject heldobject;
	Rigidbody heldrb;
	RaycastHit hit;
	float length = 0f;
	GvrLaserPointer laserpointer;
	Vector3 prevPos = Vector3.zero;
	Vector3 currentPos = Vector3.zero;

	const int layerMask = (1 << 8) | (1<<9); 
	//This is a bit shift to select for layer 8 or 9.
	//Interactive on 8, floor on 9.
	public GameObject player;
	public FadeController fadecontroller;
	public float startingreticledistance = 4f;
	public float shortduration = 0.5f;
	const float rayLength = 100f;

	void Start(){
		laserpointer = gameObject.GetComponent<GvrLaserPointer>();
	}
	
	void FixedUpdate(){

		GvrLaserPointerImpl laserpointerimpl = (GvrLaserPointerImpl)GvrPointerManager.Pointer;
		Ray ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(ray, out hit, rayLength,layerMask)) {
			GameObject objectDetected = hit.collider.gameObject;
			laserpointer.maxReticleDistance = (ray.origin-objectDetected.transform.position).magnitude;
			if (GvrController.ClickButtonUp) {
				if(objectDetected.layer == 9){
					fadecontroller.FadeOut(shortduration);
					StartCoroutine(MovePlayer(hit.point));
				}else{
					objectDetected.SendMessage("GVRClick");
				}
			}
			if(GvrController.AppButtonUp){
				if(heldobject == null && objectDetected.layer == 8){
					heldobject = objectDetected;
					heldrb = heldobject.GetComponent<Rigidbody>();
					heldrb.useGravity = false;
					length = (ray.origin-heldobject.transform.position).magnitude;
				}
				else if(heldobject != null){
					Drop();
				}
			}
			goal = hit.point;
		}
		else if(GvrController.AppButtonUp && heldobject !=null){
			Drop();
		}
		else{
			laserpointer.maxReticleDistance = startingreticledistance;
		}
		if(heldobject !=null){
			prevPos = currentPos;
			heldrb.MovePosition(length*transform.forward + transform.position);
			currentPos = heldrb.position;
		}
	}
	void Drop(){
		heldrb.useGravity = true;
		heldrb.velocity = (currentPos-prevPos)/Time.deltaTime;
		heldobject = null;
		heldrb = null;
	}
	void _MovePlayer(Vector3 pos){
		player.transform.position = new Vector3(pos.x,player.transform.position.y,pos.z);
	}
	IEnumerator MovePlayer(Vector3 pos){
		while(fadecontroller.isDone == false){
			yield return null;
		}
		_MovePlayer(pos);
		fadecontroller.FadeIn(shortduration);
	}
}
