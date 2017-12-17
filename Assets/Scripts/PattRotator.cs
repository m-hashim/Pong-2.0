using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PattRotator : MonoBehaviour {

	private const int SIZE = 6;
	private const float ALPHA_CHANGE_SPEED = 0.01f;
	private float ALPHA_DIRECTION;

	public Sprite[] patt = new Sprite[SIZE];

	private Color temp;

	void Start () {
		ALPHA_DIRECTION = 1f;
		InvokeRepeating ("pattChanger", 0f, 5f);
		InvokeRepeating ("directionChanger", 0f, 7f);
		temp = GetComponent<Image> ().color;
	}

	void FixedUpdate () {
		transform.Rotate (new Vector3(0f,0f,0.1f));
		temp.a = Mathf.Min (1.0f, temp.a + ALPHA_CHANGE_SPEED * Time.deltaTime * ALPHA_DIRECTION);
		GetComponent<Image> ().color = temp;
	}

	void directionChanger()
	{
		ALPHA_DIRECTION *= -1;
	}

	void pattChanger()
	{
		GetComponent<Image> ().sprite = patt [Random.Range (0, SIZE)];
	}
}
