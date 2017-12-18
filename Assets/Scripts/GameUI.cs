using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	private const int noOfPowerUps = 7;
	private const float moveSpeed = 2f;

	public GameObject pausePanel, gameoverPanel;
	public GameObject soundButton, magnetButton, gunButton, multiBallButton, vipButton, bigBallButton, padLongButton, flareButton;

	private int[] isSelected;

	void Start () {
		isSelected = new int[noOfPowerUps];
		bigBallButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text> ().text = "5";//youdidthistoher.Instance.powerUpArray [0].ToString();//youdidthistoher.Instance.p_bigBall.ToString();
		flareButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [1].ToString();//youdidthistoher.Instance.p_flareBall.ToString();
		gunButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [2].ToString();//youdidthistoher.Instance.p_gun.ToString();
		magnetButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [4].ToString();//youdidthistoher.Instance.p_magnet.ToString();
		vipButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [5].ToString();//youdidthistoher.Instance.p_VIPBall.ToString();
		padLongButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [3].ToString();//youdidthistoher.Instance.p_padLong.ToString();
		multiBallButton.transform.GetChild (1).transform.GetChild (0).GetComponent<Text>().text = youdidthistoher.Instance.powerUpArray [6].ToString();//youdidthistoher.Instance.p_multiBall.ToString();
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
		youdidthistoher.Instance.Save ();
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

	public void powerUpSelected(int buttonNo)
	{
	//	if (youdidthistoher.Instance.powerUpArray [buttonNo] > 0) 
		{
			print (buttonNo);
			if (isSelected [buttonNo] == 0) {
				youdidthistoher.Instance.powerUpArray [buttonNo]--;
				isSelected [buttonNo] = 1;
				youdidthistoher.Instance.powerUpArray [buttonNo]--;
				switch (buttonNo) {
				case 1:		//flare
					flareButton.GetComponent<Animation>().Play("flareUp");
					if (isSelected [5] == 1) {
						isSelected [5] = 0;
						youdidthistoher.Instance.powerUpArray [5]++;
						vipButton.GetComponent<Animation>().Play("vipDown");
					}
					break;
				case 2:	//gun
					gunButton.GetComponent<Animation>().Play("gunUp");
					if (isSelected [4] == 1) {
						isSelected [4] = 0;
						youdidthistoher.Instance.powerUpArray [4]++;
						magnetButton.GetComponent<Animation>().Play("magnetDown");
					}
					break;
				case 4:	//magnet
					magnetButton.GetComponent<Animation>().Play("magnetUp");
					if (isSelected [2] == 1) {
						isSelected [2] = 0;
						youdidthistoher.Instance.powerUpArray [2]++;
						gunButton.GetComponent<Animation>().Play("gunDown");
					}
					break;
				case 5: //vip
					vipButton.GetComponent<Animation>().Play("vipUp");
					if (isSelected [1] == 1) {
						isSelected [1] = 0;
						youdidthistoher.Instance.powerUpArray [1]++;
						flareButton.GetComponent<Animation>().Play("flareDown");
					}
					break;
				case 3: //padlong
					padLongButton.GetComponent<Animation>().Play("padLongUp");
					break;
				case 0: //big ball
					bigBallButton.GetComponent<Animation>().Play("bigBallUp");
					break;
				case 6: //multi ball
					multiBallButton.GetComponent<Animation>().Play("multiBallUp");
					break;
				default:
					break;
				}
			} else 
			{
				switch(buttonNo)
				{
				case 0:
					bigBallButton.GetComponent<Animation>().Play("bigBallDown");
					break;
				case 1:
					flareButton.GetComponent<Animation>().Play("flareDown");
					break;
				case 2:
					gunButton.GetComponent<Animation>().Play("gunDown");
					break;
				case 3:
					padLongButton.GetComponent<Animation>().Play("padLongDown");
					break;
				case 4:
					magnetButton.GetComponent<Animation>().Play("magnetDown");
					break;
				case 5:
					vipButton.GetComponent<Animation>().Play("vipDown");
					break;
				case 6:
					multiBallButton.GetComponent<Animation>().Play("multiBallDown");
					break;
				default:
					break;
				}
				isSelected [buttonNo] = 0;
				youdidthistoher.Instance.powerUpArray [buttonNo]++;
			}

		}
	}
}
