﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
	
	private const int noOfWinText=20;
	private const int noOfLoseText=20;
	private float alphaVal=0.25f;
	private float alphaStepFactor = 0.15f;
	private const int noOfPowerUps = 7;
	private const float moveSpeed = 2f;

	public GameObject pausePanel, gameoverPanel, pauseButton, gameoverAnimationButton;
	public GameObject soundButton, magnetButton, gunButton, multiBallButton, vipButton, bigBallButton, padLongButton, flareButton, gridLock;

	public Text goText, descriptionText;

	private int[] isSelected;
	private bool secondPause=false;

	private string[] parsed;
	private TextAsset txt;

	private Color tempCol;

	void Start () {
		isSelected = new int[noOfPowerUps];
		parsed = new string[noOfWinText];
		tempCol = gridLock.GetComponent<Image> ().color;
	}

	public void gameOver(int state, int coinCount)
	{
		switch (state) 
		{
		case 0:
			txt = (TextAsset)Resources.Load ("TextAssets/win", typeof(TextAsset));
			parsed = txt.text.Split ("\n" [0]);
			descriptionText.text = parsed [Random.Range (0, noOfWinText)];
			goText.text = "SUSTAINED";
			break;
		case 1:
			txt = (TextAsset)Resources.Load ("TextAssets/lose",typeof(TextAsset));
			parsed=txt.text.Split ("\n"[0]);
			descriptionText.text = parsed [Random.Range (0, noOfLoseText)];
			goText.text = "THUMPED";
			break;
		}
		youdidthistoher.Instance.currency+=coinCount;
		youdidthistoher.Instance.Save ();
		Time.timeScale = 0f;
	}
		
	public void pause()
	{
		Time.timeScale = 0f;
		pauseButton.GetComponent<Button> ().interactable = false;
		loadPowerUpCount ();
		if(secondPause)
			resetPowerUps ();
	}

	public void unPause()
	{
		secondPause = true;
		for (int i = 0; i < noOfPowerUps; i++) {
			if (isSelected [i] == 1) {
				switch (i) 
				{
				case 0:	
					PowerUp.bigBallPurchased = true;
					break;
				case 1:
					PowerUp.flareBallPurchased = true;
					break;
				case 2:
					PowerUp.gunPadPurchased = true;
					break;
				case 3:
					PowerUp.padLongPurchased = true;
					break;
				case 4:
					PowerUp.magnetPadPurchased = true;
					break;
				case 5:
					PowerUp.VIPBallPurchased = true;
					break;
				case 6:
					PowerUp.multiBallPurchased = true;			
					break;
				}
				isSelected[i] = 0;
			}
		}
		youdidthistoher.Instance.Save ();

		Time.timeScale = 1f;
		pauseButton.GetComponent<Button> ().interactable = true;
		youdidthistoher.Instance.Save ();
	}

	public void restart()
	{
		Time.timeScale = 1.0f;
		gameoverAnimationButton.GetComponent<UIAnimController>().PanelInactive();
		SceneManager.LoadScene ("Pong_Breaker");
	}

	public void back()
	{
		for (int i = 0; i < noOfPowerUps; i++) {
			if (isSelected [i] == 1) {
				youdidthistoher.Instance.powerUpArray [i]++;
			}
		}
		Time.timeScale = 1f;
		SceneManager.LoadScene ("Main Scene");	
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

	public void powerUpSelected(int buttonNo)
	{
		if (youdidthistoher.Instance.powerUpArray [buttonNo] > 0) 
		{
	//		print (buttonNo);
			print ("alphaVal"+alphaVal);
			if (isSelected [buttonNo] == 0) {
				isSelected [buttonNo] = 1;
				youdidthistoher.Instance.powerUpArray [buttonNo]--;
				alphaVal += alphaStepFactor;
				switch (buttonNo) {
				case 1:		//flare
					flareButton.GetComponent<Animator> ().ResetTrigger ("flareDown");
					flareButton.GetComponent<Animator> ().SetTrigger ("flareUp");
					if (isSelected [5] == 1) {
						isSelected [5] = 0;
						alphaVal -= alphaStepFactor;
						youdidthistoher.Instance.powerUpArray [5]++;
						vipButton.GetComponent<Animator>().ResetTrigger("vipUp");
						vipButton.GetComponent<Animator>().SetTrigger("vipDown");
					}
					break;
				case 2:	//gun
					gunButton.GetComponent<Animator>().ResetTrigger("gunDown");
					gunButton.GetComponent<Animator>().SetTrigger("gunUp");
					if (isSelected [4] == 1) {
						isSelected [4] = 0;
						alphaVal -= alphaStepFactor;
						youdidthistoher.Instance.powerUpArray [4]++;
						magnetButton.GetComponent<Animator>().ResetTrigger("magnetUp");
						magnetButton.GetComponent<Animator>().SetTrigger("magnetDown");
					}
					break;
				case 4:	//magnet
					magnetButton.GetComponent<Animator>().ResetTrigger("magnetDown");
					magnetButton.GetComponent<Animator>().SetTrigger("magnetUp");
					if (isSelected [2] == 1) {
						isSelected [2] = 0;
						alphaVal -= alphaStepFactor;
						youdidthistoher.Instance.powerUpArray [2]++;
						gunButton.GetComponent<Animator>().ResetTrigger("gunUp");
						gunButton.GetComponent<Animator>().SetTrigger("gunDown");
					}
					break;
				case 5: //vip
					vipButton.GetComponent<Animator>().ResetTrigger("vipDown");
					vipButton.GetComponent<Animator>().SetTrigger("vipUp");
					if (isSelected [1] == 1) {
						isSelected [1] = 0;
						alphaVal -= alphaStepFactor;
						youdidthistoher.Instance.powerUpArray [1]++;
						flareButton.GetComponent<Animator>().ResetTrigger("flareUp");
						flareButton.GetComponent<Animator>().SetTrigger("flareDown");
					}
					break;
				case 3: //padlong
					padLongButton.GetComponent<Animator>().ResetTrigger("padLongDown");
					padLongButton.GetComponent<Animator>().SetTrigger("padLongUp");
					break;
				case 0: //big ball
					bigBallButton.GetComponent<Animator>().ResetTrigger("bigBallDown");
					bigBallButton.GetComponent<Animator>().SetTrigger("bigBallUp");
					break;
				case 6: //multi ball
					multiBallButton.GetComponent<Animator>().ResetTrigger("multiBallDown");
					multiBallButton.GetComponent<Animator>().SetTrigger("multiBallUp");
					break;
				default:
					break;
				}
				tempCol.a = alphaVal;
				gridLock.GetComponent<Image> ().color = tempCol;
			} else 
			{
				switch(buttonNo)
				{
				case 0:
					bigBallButton.GetComponent<Animator>().ResetTrigger("bigBallUp");
					bigBallButton.GetComponent<Animator>().SetTrigger("bigBallDown");
					break;
				case 1:
					flareButton.GetComponent<Animator>().ResetTrigger("flareUp");
					flareButton.GetComponent<Animator>().SetTrigger("flareDown");
					break;
				case 2:
					gunButton.GetComponent<Animator>().ResetTrigger("gunUp");
					gunButton.GetComponent<Animator>().SetTrigger("gunDown");
					break;
				case 3:
					padLongButton.GetComponent<Animator>().ResetTrigger("padLongUp");
					padLongButton.GetComponent<Animator>().SetTrigger("padLongDown");
					break;
				case 4:
					magnetButton.GetComponent<Animator>().ResetTrigger("magnetUp");
					magnetButton.GetComponent<Animator>().SetTrigger("magnetDown");
					break;
				case 5:
					vipButton.GetComponent<Animator>().ResetTrigger("vipUp");
					vipButton.GetComponent<Animator>().SetTrigger("vipDown");
					break;
				case 6:
					multiBallButton.GetComponent<Animator>().ResetTrigger("multiBallUp");
					multiBallButton.GetComponent<Animator>().SetTrigger("multiBallDown");
					break;
				default:
					break;
				}
				isSelected [buttonNo] = 0;
				youdidthistoher.Instance.powerUpArray [buttonNo]++;
				alphaVal -= alphaStepFactor;
				tempCol.a = alphaVal;
				gridLock.GetComponent<Image> ().color = tempCol;
			}

		}
	}

	private void loadPowerUpCount()
	{
		bigBallButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text> ().text = youdidthistoher.Instance.powerUpArray [0].ToString();//youdidthistoher.Instance.p_bigBall.ToString();
		flareButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [1].ToString();//youdidthistoher.Instance.p_flareBall.ToString();
		gunButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [2].ToString();//youdidthistoher.Instance.p_gun.ToString();
		magnetButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [4].ToString();//youdidthistoher.Instance.p_magnet.ToString();
		vipButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [5].ToString();//youdidthistoher.Instance.p_VIPBall.ToString();
		padLongButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [3].ToString();//youdidthistoher.Instance.p_padLong.ToString();
		multiBallButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [6].ToString();//youdidthistoher.Instance.p_multiBall.ToString();
	}

	private void resetPowerUps ()
	{
		bigBallButton.GetComponent<Animator>().SetTrigger("bigBallDown");
		flareButton.GetComponent<Animator>().SetTrigger("flareDown");
		gunButton.GetComponent<Animator>().SetTrigger("gunDown");
		padLongButton.GetComponent<Animator>().SetTrigger("padLongDown");
		magnetButton.GetComponent<Animator>().SetTrigger("magnetDown");
		vipButton.GetComponent<Animator>().SetTrigger("vipDown");
		multiBallButton.GetComponent<Animator>().SetTrigger("multiBallDown");
	}

	public void backGO()
	{	/////
		//	AdManager.Instance.HideBanner ();
		/////
		Time.timeScale = 1.0f;
		gameoverAnimationButton.GetComponent<UIAnimController>().PanelInactive();
		//PlayerPrefs.SetInt ("currentPlayingLevel", PlayerPrefs.GetInt ("currentPlayingLevel") + 1);
		youdidthistoher.Instance.currentPlayingLevel++;
		youdidthistoher.Instance.Save ();
		SceneManager.LoadScene ("Main Scene");
	}

	public void nextLevelGO()
	{	/////
		//	AdManager.Instance.HideBanner ();
		/////
		//	
		Time.timeScale = 1.0f;
		gameoverAnimationButton.GetComponent<UIAnimController>().PanelInactive();
		youdidthistoher.Instance.currentPlayingLevel++;
		youdidthistoher.Instance.Save ();
		SceneManager.LoadScene ("Pong_Breaker");

	}
}