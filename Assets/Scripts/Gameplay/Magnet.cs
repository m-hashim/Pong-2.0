using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour {

	public GameObject padMagnet;
	public GameObject ballPrefab;

	private int index;
	private Vector3[] velocities = new Vector3[3];

	void FixedUpdate()
	{		if (Input.touchCount >= 2) {
				release ();
			}
			if (Input.GetMouseButtonDown (0)) {
				release();
			}
	}
	public void release(){
		Transform[] childObjects = GetComponentsInChildren<Transform> ();
		int i = 0;
		foreach (Transform temp in childObjects) {
			if (temp.CompareTag ("Ball")) {
				temp.SetParent (GameManager.Instance.BallContainer);
				temp.gameObject.GetComponent<Rigidbody> ().isKinematic = false;
				temp.GetComponent<Rigidbody> ().velocity = velocities [i++];
				temp.localScale = new Vector3 (0.75f, 0.75f, 0.75f);
			}
		}
		index = 0;
	}
	void OnTriggerEnter(Collider col)
	{	
		//	print (col.gameObject.name);
		if (col.gameObject.CompareTag ("Ball")) {
			if (gameObject.transform.parent.CompareTag("AI")) {
				Invoke("release", 1.5f);
			}
		//	print ("Ball is entered");
			col.transform.SetParent (this.transform);
			velocities [index++] = col.gameObject.GetComponent<Rigidbody> ().velocity;
			col.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
			col.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
	
			print (col.gameObject.GetComponent<BallS> ().ballRig.velocity);
		}
	}

}
