using UnityEngine;
using System.Collections;

public enum TypePiece
{
	Pion,
	Tour,
	Cavalier,
	Fou,
	Reine,
	Roi
}

public class Movewithmouse : MonoBehaviour
{

		
		public TypePiece Piece;
		public bool isWhite;
		//True if the piece has already moved.
		bool moved = false;
		bool dragBegin = false;
		Vector3 startPosition;
		Vector3 targetPoint;
		Vector3 startDelta;
		Plane initialPlane;
		GameObject Plateau;
		PlateauMove PlateauScript;
		//Touch detect
		bool touching;

		// Use this for initialization
		void Start ()
		{
				Plateau = GameObject.FindGameObjectWithTag ("Plateau");
				PlateauScript = (PlateauMove)Plateau.GetComponent<PlateauMove> ();
		}
	
		// Update is called once per frame
		void Update ()
		{

				//Detect touch down
				if (Input.touchCount == 1) {
						// touch on screen
						if (Input.GetTouch (0).phase == TouchPhase.Began) {
								Ray touchRay = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
								RaycastHit hit = new RaycastHit ();
								touching = Physics.Raycast (touchRay, out hit);
								if (touching) {

										if (hit.transform.gameObject == this.gameObject) {
												OnMouseDown ();
										} else {
												touching = false;
										}
								}

						}
				}
				if (dragBegin) {
						Vector3 point = touching ? (Vector3)Input.GetTouch (0).position : Input.mousePosition;
						Ray ray = Camera.main.ScreenPointToRay (point);
						// plane.Raycast returns the distance from the ray start to the hit point
						float distance;
						if (initialPlane.Raycast (ray, out distance)) {
								// some point of the plane was hit - get its coordinates
								transform.position = ray.GetPoint (distance);
						}
				}


				// release touch/dragging
				//Detect touch down
				if (Input.touchCount == 1) {
						if ((Input.GetTouch (0).phase == TouchPhase.Ended || Input.GetTouch (0).phase == TouchPhase.Canceled) && touching) {
						
								touching = false;
								OnMouseUp ();
						}
				}

		}


		/************************
		 * 
		 * Events 
		 * 
		 * **********************/
		void OnMouseDown ()
		{	
				if (dragBegin == false) {
						BeginDragging (true);
						startPosition = transform.position;
						// this creates a horizontal plane passing through this object's center
						initialPlane = new Plane (Vector3.up, transform.position);
				}
		}

		void OnMouseUp ()
		{		
				if (dragBegin) {
						BeginDragging (false);
						Vector3 closestTile = getClosestTile ();
			targetPoint = PlateauScript.moveToTile (this, closestTile);
						if (targetPoint == closestTile && Vector3.Distance(closestTile, startPosition)>0.5) {
								moved = true;
						}
						StartCoroutine ("MoveToTile");
				}	
		}

		/************************
		 * 
		 * Accessors 
		 * 
		 * **********************/
	public Vector3 getStartPosition(){
		return startPosition;
	}

	public bool hasMoved(){ return moved;}


		IEnumerator MoveToTile ()
		{		
				
				Vector3 fromPoint = transform.position;
				for (float f = 1f; f >= 0; f -= 0.2f) {
						Vector3 delta = targetPoint - fromPoint;
						transform.position = targetPoint - f * delta;
						yield return new WaitForSeconds (.01f);
					
				}
				transform.position = targetPoint;
				startPosition = Vector3.zero;
		}

		Vector3 getClosestTile ()
		{
				Vector3 tile = transform.position;
				tile.x = Mathf.RoundToInt (tile.x);
				tile.z = Mathf.RoundToInt (tile.z);
				return tile;
		}

		void BeginDragging (bool begin)
		{
				dragBegin = begin;
				PlateauScript.beginDragging (begin);
		}
}
