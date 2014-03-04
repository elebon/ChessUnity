using UnityEngine;
using System.Collections;

public class CameraTurnAround : MonoBehaviour {
	
	public Transform staticPoint;
	public bool revertX = false;
	public bool revertY = false;
	public float sensibility = 1f;
	Vector3 startMousePosition;
	Vector3 startCameraPosition;
	Quaternion startCameraRotation;
	Vector3 startEulerAngles;
	bool dragBegin = false;
	public float futurRotation;

	PlateauMove PlateauScript;
	// Use this for initialization
	void Start () {
		Vector3 relativePos = staticPoint.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		transform.rotation = rotation;

		PlateauScript =(PlateauMove) GameObject.FindGameObjectWithTag ("Plateau").GetComponent(typeof(PlateauMove));
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if (Input.GetMouseButtonDown(0) ){
			OnMouseDown();
		}
		
		
		if (dragBegin && staticPoint != null) {
			Vector3 deltaMousePosition = Input.mousePosition - startMousePosition;
			float deltaAlpha = (revertX? -1 : 1) * sensibility*deltaMousePosition.x;
			float deltaTheta = (revertY? -1 : 1) * sensibility * deltaMousePosition.y;
			float currentRotation = transform.eulerAngles.x - startEulerAngles.x;
			
			transform.position = startCameraPosition;
			transform.rotation = startCameraRotation;

			transform.RotateAround(staticPoint.position, Vector3.up, deltaAlpha);
//			startMousePosition.x  = Input.mousePosition.x;
			futurRotation = startEulerAngles.x +deltaTheta;
			if(futurRotation <=90 && futurRotation>=0){
				transform.RotateAround(staticPoint.position, transform.right, deltaTheta);
				//startMousePosition.y  = Input.mousePosition.y;
			}else{
				transform.RotateAround(staticPoint.position, transform.right,currentRotation);
			}
		}
		
		if( Input.GetMouseButtonUp(0)){
			OnMouseUp();
		}
	}
	
	void OnMouseDown(){
		if (!dragBegin && !PlateauScript.isDragging()){
			dragBegin = true;
			startMousePosition = Input.mousePosition;
			startCameraPosition = transform.position;
			startCameraRotation = transform.rotation;
			startEulerAngles = transform.eulerAngles;

		}
	}
	
	void OnMouseUp(){
		dragBegin = false;
	}
}