using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlateauMove : MonoBehaviour
{

		bool isdragging = false;
		private List<Move> moveList = new List<Move> ();
		private Vector3 whiteForward = new Vector3 (0, 0, 1);
		private List<Movewithmouse> playingPieces;
		private List<Movewithmouse> deadPieces = new List<Movewithmouse> ();

		// Use this for initialization
		void Start ()
		{
				playingPieces = new List<Movewithmouse> (FindObjectsOfType<Movewithmouse> ());
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void beginDragging (bool begin)
		{
				isdragging = begin;
		}

		public bool isDragging ()
		{
				return isdragging;
		}

		/**
	 * 
	 * Register a movement if allowed
	 * 
	 * Return startPosition, if move is not allowed.
	 * 
	 * */
		public Vector3 moveToTile (Movewithmouse obj, Vector3 destination)
		{
				if (canMoveToTile (obj, destination)) {
						moveList.Add (new Move (obj, obj.getStartPosition (), destination));
						return destination;
				}
				return obj.getStartPosition ();
		}

		bool canMoveToTile (Movewithmouse obj, Vector3 destination)
		{
				if (destination.x < 0 || destination.x > 7 || destination.z < 0 || destination.z > 7)
						return false;

				switch (obj.Piece) {
				case TypePiece.Pion:
						return PionCanMoveToTile (obj, destination);
				case TypePiece.Tour:
						break;
				case TypePiece.Cavalier:
						break;
				case TypePiece.Fou:
						break;
				case TypePiece.Reine:
						break;
				case TypePiece.Roi:
						break;
				default:
						return false;
				}
				return false;
		}


		/**
	 * 
	 * Comportement du pion
	 * 
	 * */
		bool PionCanMoveToTile (Movewithmouse obj, Vector3 destination)
		{

				Vector3 forward = obj.isWhite ? whiteForward : -whiteForward;
				Vector3 delta = destination - obj.getStartPosition ();
				if (((int)delta.z == forward.z && (int)delta.x == 0)
		   				|| (!obj.hasMoved () && (int)delta.z == 2 * forward.z && delta.x == 0)
						|| priseEnPassant (obj, destination, delta))
						return true;
				return false;
		}
	
		bool priseEnPassant (Movewithmouse obj,Vector3 destination, Vector3 delta)
		{
				if (obj.Piece == TypePiece.Pion 
				//Move in diagonal
						&& (int)Mathf.Abs (delta.x) == 1 && (int)Mathf.Abs (delta.z) == 1
		    		) {
						
						// Checking if neighbor is a Pion and is of the opposite color
						Movewithmouse neighbor = PieceAtPosition ((int)Mathf.Round(obj.getStartPosition().x + delta.x),
			                                          (int)Mathf.Round(obj.getStartPosition().z), obj);

						if (neighbor != null && neighbor.Piece == TypePiece.Pion && neighbor.isWhite != obj.isWhite) {
								if (moveList.Count > 0 && moveList [moveList.Count - 1].Piece.Piece == TypePiece.Pion 
				    				) {
									Move lastMove = moveList[moveList.Count -1];
									//Check that last Piece move from start position (ie Pion moved of 2 tiles)
									if(Vector3.Distance(neighbor.transform.position, lastMove.Piece.transform.position)<0.5
					   					&& (int)Mathf.Abs ((lastMove.toPoint - lastMove.fromPoint).z)== 2)
										return true;
								}
						}
								
				}
				return false;
		}


		/**
	 * 
	 * Outils
	 * 
	 * */
		Movewithmouse PieceAtPosition (int x, int z, Movewithmouse ignoredPiece)
		{
				foreach (Movewithmouse piece in playingPieces) {
						if (ignoredPiece != piece && (int)piece.transform.position.x == x && (int)piece.transform.position.z == z) {
								return piece;
						}
				}
				return null;
		}
}

