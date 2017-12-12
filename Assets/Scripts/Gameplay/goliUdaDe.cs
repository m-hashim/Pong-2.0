using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goliUdaDe : MonoBehaviour {

	private bool turn;

	private GameManager GMScript;
	void Start(){
		if (gameObject.CompareTag ("player")) {
			turn = true;
		} else {
			turn = false;
		}
	}
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag ("Block")) {
			col.GetComponent<Block> ().ResetBlock (turn);

		} 
		else if (col.gameObject.name.Contains ("Wall")||col.gameObject.CompareTag("AI")||col.gameObject.CompareTag("player")) {
			Destroy (this.gameObject);
		}

	}

}
