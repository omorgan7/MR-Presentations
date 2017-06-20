using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GvrInput : MonoBehaviour {
	
	bool held = false;
	GameObject heldObject;
	Rigidbody rb;
	// Update is called once per frame
	void Update(){
		    GvrLaserPointerImpl laserpointerimpl = (GvrLaserPointerImpl)GvrPointerManager.Pointer;
			Vector3 end = laserpointerimpl.LineEndPoint;
			Ray ray = new Ray(transform.position, transform.forward);
			
			int layerMask = 1 << 8;
			//INTERACTIVE layer is on layer 8. 
			//This is a bit shift to select for layer 8.
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100,layerMask)) {
				GameObject objectDetected = hit.collider.gameObject;
				if (GvrController.ClickButtonUp) {
					objectDetected.SendMessage("GVRClick");
				}
				if(GvrController.AppButtonUp && held == false){
					heldObject = objectDetected;
					rb = heldObject.GetComponent<Rigidbody>();

				}
			}
			if(GvrController.AppButtonUp){
				held = !held;
			}
			if(held == true){
				rb.position = new Vector3(hit.point.x,hit.point.y,rb.position.z);
			}
	}
}
