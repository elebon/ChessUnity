using UnityEngine;
using System.Collections;

public class Move
{

	private Vector3 from;
	private Vector3 to;

	public Vector3 fromPoint{ get{return from;} private set{from = value;} }
	public Vector3 toPoint{ get{return to;} private set{to = value;} }
	public Movewithmouse Piece;

		public Move (Movewithmouse obj, Vector3 from, Vector3 to)
		{
				this.Piece = obj;
				this.toPoint = to;
				this.fromPoint = from;
		}
}

