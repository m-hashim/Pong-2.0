using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockToggle : MonoBehaviour {
	public Material invisibleMat;
	private Material original;
	// Use this for initialization
	void Start () {
		original = this.GetComponent<Renderer> ().material;
	}
	void OnCollisionEnter(Collision col){
		if (gameObject.GetComponent<Block> ().blockType != BlockTypes.Toggle)
			return;
		if (col.gameObject.CompareTag ("Ball") || col.gameObject.CompareTag ("Bullet")||col.gameObject.CompareTag("padGoli")) {
			gameObject.GetComponent<Collider>().isTrigger=true;
			this.GetComponent<Renderer> ().material = invisibleMat;

		}

	}
	void OnTriggerExit(Collider col){
		if (gameObject.GetComponent<Block> ().blockType != BlockTypes.Toggle)
			return;
		if (col.gameObject.CompareTag ("Ball") || col.gameObject.CompareTag ("Bullet")||col.gameObject.CompareTag("padGoli")) {
			gameObject.GetComponent<Collider>().isTrigger=false;
			this.GetComponent<Renderer> ().material = original;
		}
	}
}
