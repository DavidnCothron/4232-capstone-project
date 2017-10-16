using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAILogic : MonoBehaviour {

	[SerializeField]
	private AstarUser aStarUser;
	private Transform playerTransform;

	public enum IntelligenceType
	{
		shamblingBones = 0,
		cryptBat,
		undeadKnight = 2

	}

	public Vector2 AIMove(int intLevel)
	{
		switch (intLevel)
		{
		case (int)IntelligenceType.shamblingBones:
			return ShamblingBones ();
			break;
		case (int)IntelligenceType.cryptBat:
			return CryptBat ();
			break;
		case (int)IntelligenceType.undeadKnight:
			return UndeadKnight ();
			break;
		default :
			Debug.Log ("Not Implemented Intelligence Type");
			return Vector2.zero;
			break;
		}
	}

	public Vector2 ShamblingBones()
	{
		playerTransform = GameControl.control.GetPlayerTransform ();
		return (Vector2)playerTransform.position;
	}

	public Vector2 CryptBat()
	{
		return Vector2.zero;
	}

	public Vector2 UndeadKnight ()
	{
		return Vector2.zero;
	}
}
