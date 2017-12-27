using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTouchAnim : MonoBehaviour {

	void OnCollisionEnter(Collision col)
	{
		GetComponent<Animation> ().Play ();
	}
}
