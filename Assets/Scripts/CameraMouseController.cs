using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseController : MonoBehaviour {
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	public float movesensitivityX = 1f;
	public float movesensitivityY = 1f;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public float minimumY = -60F;
	public float maximumY = 60F;
	public float speed = 0.1f;
	float rotationX = 0F;
	float rotationY = 0F;
	Quaternion originalRotation;
	Rigidbody rb;
	Transform transform;
	void Update (){
		rotationX += Input.GetAxis("Mouse X") * sensitivityX;
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationX = ClampAngle (rotationX, minimumX, maximumX);
		rotationY = ClampAngle (rotationY, minimumY, maximumY);
		Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
		Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);
		transform.localRotation = originalRotation * xQuaternion * yQuaternion;
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		Vector3 direction = transform.right*moveHorizontal*movesensitivityX + transform.forward*moveVertical*movesensitivityY;
		if(rb == null){
			direction = direction + transform.localPosition;
			transform.localPosition = new Vector3(direction.x,transform.localPosition.y,direction.z);
		}
		else{
			rb.AddForce(direction*speed);
		}
		
	}

	void Start(){
		transform = gameObject.GetComponent<Transform>();
		rb = gameObject.GetComponent<Rigidbody>();
		originalRotation = transform.localRotation;
	}
	public static float ClampAngle (float angle, float min, float max){
		if (angle < -360F){
			angle += 360F;
		}
		if (angle > 360F){
			angle -= 360F;
		}
		return Mathf.Clamp (angle, min, max);
	}
}
