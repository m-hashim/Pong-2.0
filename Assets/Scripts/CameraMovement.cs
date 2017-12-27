using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	private const float speed = 0.006f;

	public Vector3[] posList;
	public GameObject lookAt;

	private Vector3 targetPos;

	void Start () {
		targetPos=posList[Random.Range (0, posList.Length)];
	}
	
	void Update () {
		if (transform.position == targetPos) {
			targetPos=posList[Random.Range (0, posList.Length)];
		} else {
			transform.position = Vector3.MoveTowards (transform.position, targetPos, speed);
			transform.LookAt (lookAt.transform);
		}
	}
}
