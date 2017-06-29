using System.Collections;
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

	const int layerMask = (1 << 8) | (1<<9); 
	//This is a bit shift to select for layer 8 or 9.
	//Interactive on 8, floor on 9.
	public GameObject player;
	const float rayLength = 100f;

	void FixedUpdate(){

		GvrLaserPointerImpl laserpointerimpl = (GvrLaserPointerImpl)GvrPointerManager.Pointer;
		Ray ray = new Ray(transform.position, transform.forward);

		if (Physics.Raycast(ray, out hit, rayLength,layerMask)) {
			GameObject objectDetected = hit.collider.gameObject;
			if (GvrController.ClickButtonUp) {
				if(objectDetected.layer == 9){
					player.transform.position = new Vector3(hit.point.x,player.transform.position.y,hit.point.z);
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
				else{
					heldrb.useGravity = true;
					Drop();
				}
			}
			goal = hit.point;
		}
		else if(GvrController.AppButtonUp){
			Drop();
		}
		if(heldobject !=null){
			heldrb.MovePosition(length*transform.forward + transform.position);//new Vector3(transform.position.x + length*transform.forward.x,transform.position.y + length*transform.forward.y,zPos);
		}
	}
	void Drop(){
		heldobject = null;
		heldrb = null;
	}
}
