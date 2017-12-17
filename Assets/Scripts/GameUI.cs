using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

	public GameObject pausePanel, gameoverPanel;
	public GameObject soundButton;

	void Start () {
		
	}
	
	void Update () {
		
	}

	public void pause()
	{
		Time.timeScale = 0f;
		pausePanel.SetActive (true);
	}

	public void continueFromPause()
	{
		Time.timeScale = 1f;
		pausePanel.SetActive (false);
	}

	public void restart()
	{
		SceneManager.LoadScene ("Pong_Breaker");
	}

	public void soundButtonController()
	{
		if (youdidthistoher.Instance.soundOn == 1) {
			youdidthistoher.Instance.soundOn = 0;
			soundButton.transform.GetChild (0).gameObject.SetActive (false);
			soundButton.transform.GetChild (1).gameObject.SetActive (true);
		} else {
			youdidthistoher.Instance.soundOn = 1;
			soundButton.transform.GetChild (1).gameObject.SetActive (false);
			soundButton.transform.GetChild (0).gameObject.SetActive (true);
		}
		youdidthistoher.Instance.Save ();
	}
}
