using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Drunk : MonoBehaviour {
	private Vector3 camData;
	private Transform cam;
	public Camera topDown,fP;
	void Start () {
		switch (youdidthistoher.Instance.currentCameraMode) {
		case 0:
			cam = Camera.main.transform;
			break;
		case 1:
			cam = topDown.transform;
			break;
		case 2:
			cam = fP.transform;
			break;
		}

		camData = cam.rotation.eulerAngles;

	}
	void FixedUpdate () {
		cam.transform.rotation = Quaternion.Slerp (cam.rotation, Random.rotation, 0.1f*Time.deltaTime);
		int margin = 10;
		if (cam.transform.rotation.eulerAngles.x > camData.x+margin || cam.transform.rotation.eulerAngles.x < camData.x-margin)
			cam.transform.rotation = Quaternion.Euler (camData.x, cam.transform.rotation.eulerAngles.y, cam.transform.rotation.eulerAngles.z);

		if (cam.transform.rotation.eulerAngles.y > camData.y+margin || cam.transform.rotation.eulerAngles.y < camData.y-margin)
			cam.transform.rotation = Quaternion.Euler (cam.transform.rotation.eulerAngles.x, camData.y, cam.transform.rotation.eulerAngles.z);

		if (cam.transform.rotation.eulerAngles.z > camData.z+margin || cam.transform.rotation.eulerAngles.z < camData.z-margin)
			cam.transform.rotation = Quaternion.Euler (cam.transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, camData.z);


		transform.rotation = Quaternion.identity;																//Rotates to zero

		if (Random.Range (0f, 1f) <= 0.05f)
		{
			Vector3 daruChal = new Vector3 (Random.Range (-1f,1f), 0.0f, Random.Range (-1f, 1f));
			daruChal /= 1.5f;
			transform.Translate (daruChal);
		}

	}


}