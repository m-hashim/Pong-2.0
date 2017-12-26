using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

	private const int NO_OF_ROWS = 4;
	private const int NO_OF_COLUMNS = 5;
	private const int NO_OF_PAGES = 20;
	private const int POWERUP_COUNT = 7;
	private const int GROUND_COUNT = 4;
	private const int BLOKES_COUNT = 5;
	private const int SPECIAL_COUNT = 4;
	private const int PAD_COUNT = 9;
	private const float gameSelectionWaitTime = 3.5f;
	private const int maxDescriptionSizeParsed=15;

	private bool blockMovement,firstBlock, levelSelected;
	private int levelCount, levelsUnlocked;
	private int lastSwitch=1;
	private int currentActiveStore=0;
	private int[] currentSelectedInStores =  new int[5];

	private string[] parsed;

	private TextAsset txt;

	public Text  descriptionText, itemDescriptionText, countText;


	public GameObject mainPanel, gameSelectionPanel, settingsPanel, aboutPanel, helpPanel, shopPanel, patt;
	public GameObject LevelPage, LevelButton, LevelRow, StoreButton;
	public GameObject GroundsPanel, BlokesPanel, PowerupsPanel, PadsPanel, SpecialPanel;
	public GameObject soundButton, cameraButton, controlButton;
	public GameObject practiceButton, endlessButton, campaignButton, forwardCampaignButton, backwardCampaignButton;
	public GameObject bckGameSelection, bckLevelSelection;			//back buttons that need to be disabled
	public GameObject confirmBuy;

	public Sprite[] powerUpSprites = new Sprite[POWERUP_COUNT];
	public Sprite[] groundSprites = new Sprite[GROUND_COUNT];
	public Sprite[] blokeSprites = new Sprite[BLOKES_COUNT];
	public Sprite[] padSprites = new Sprite[PAD_COUNT];

	private GameObject[] row = new GameObject[NO_OF_ROWS];
	private GameObject[,] levels = new GameObject[NO_OF_ROWS,NO_OF_COLUMNS];
	private GameObject prevButton;
	private int prevItemNo;

	void Awake () 
	{
		Time.timeScale = 1f;
		string isLoginOnce  = PlayerPrefs.GetString ("_isLoginOnce");
		if (isLoginOnce != "True") {
			PlayerPrefs.SetString ("_isLoginOnce", "True");
			cameraButtonController (2);	//fps button active
			controlButtonController (1);	//touch button active
			//		PlayerPrefs.SetInt ("PlayerModePong",0);
		} else {
			cameraButtonController (youdidthistoher.Instance.currentCameraMode);
			controlButtonController (PlayerPrefs.GetInt ("InputType"));
		}
		parsed = new string[maxDescriptionSizeParsed];			
		firstBlock=levelSelected=false;
		levelsUnlocked = youdidthistoher.Instance.campaignLevelReached;
		levelCount = (NO_OF_ROWS*NO_OF_COLUMNS)*(levelsUnlocked/(NO_OF_ROWS*NO_OF_COLUMNS))+1;
		print (levelCount);
		makeAPage (LevelPage.transform.position);									//Campaign levels
		if (levelsUnlocked % 20 == 0) {
			previousPageLevel ();
			blockMovement = true;
		}
		currentSelectedInStores[1]=youdidthistoher.Instance.skinAvailabilityPad;
		currentSelectedInStores[3]=youdidthistoher.Instance.skinAvailabilityBloke;
		currentSelectedInStores[4]=youdidthistoher.Instance.skinAvailabilityGround;
		currentActiveStore = 4;
		shopInstantiator ("Text/grounds",GROUND_COUNT,GroundsPanel,groundSprites);
		currentActiveStore = 3;
		shopInstantiator ("Text/blokes",BLOKES_COUNT,BlokesPanel,blokeSprites);
		currentActiveStore = 1;
		shopInstantiator ("Text/pads",PAD_COUNT,PadsPanel,padSprites);
		currentActiveStore = 0;
		shopInstantiator ("Text/powerups",POWERUP_COUNT,PowerupsPanel,powerUpSprites);
	}


	void shopInstantiator(string address, int count, GameObject parentGameObject, Sprite[] images)
	{
		txt = (TextAsset)Resources.Load (address,typeof(TextAsset));
		parser (txt.text);
//		RectTransform rowRectTransform = StoreButton.GetComponent<RectTransform>();
//		RectTransform containerRectTransform = parentGameObject.GetComponent<RectTransform>();

//		float width = containerRectTransform.rect.width / NO_OF_COLUMNS;
//		float ratio = width / rowRectTransform.rect.width;
//		float height = rowRectTransform.rect.height * ratio;
		int i = 1;
		for (int j = 0; j < count; j++) {
			GameObject temp = GameObject.Instantiate (StoreButton, parentGameObject.transform.position, Quaternion.identity);
			temp.transform.position = parentGameObject.transform.position;
			temp.transform.SetParent (parentGameObject.transform.GetChild(0).transform);
			temp.GetComponent<Image> ().sprite = images [j];
			int tempItemNo = j;
			temp.gameObject.GetComponent<Button> ().onClick.AddListener (() => StoreItem (temp, tempItemNo));
			if ((currentSelectedInStores [currentActiveStore] & 1 << tempItemNo) == 1 << tempItemNo && currentActiveStore != 0) {
				//if already purchased
				temp.transform.GetChild (0).gameObject.SetActive (false);
				if (tempItemNo == currentSelectedInStores [currentActiveStore])
					temp.transform.GetChild (1).gameObject.SetActive (true);
			} else if (currentActiveStore == 0) {
				temp.transform.GetChild (0).gameObject.SetActive (false);
			}
			else {
				temp.transform.GetChild (0).GetComponent<Text> ().text = (j + 1).ToString();		//enter prices here
			}
		
/*
			float offsetRatio = 0.1f;
			RectTransform rectTransform = temp.GetComponent<RectTransform> ();

			float x = -containerRectTransform.rect.width / 2 + width * (j) + width * offsetRatio;
			float y = containerRectTransform.rect.height / 2 - height * (i + 1) + height * offsetRatio;
			rectTransform.offsetMin = new Vector2 (x, y);

			x = rectTransform.offsetMin.x + width - 2 * width * offsetRatio;
			y = rectTransform.offsetMin.y + height - 2 * height * offsetRatio;
			rectTransform.offsetMax = new Vector2 (x, y);

			rectTransform.localScale = new Vector3 (1f, 1f, 1f);
*/		}
	}

	void Start()
	{
		shopStarter ();
	}

	private void parser(string str)
	{
		//	for (int i = 0; i < 10; i++)
		//		parsed [i] = null;
		parsed=str.Split ("\n"[0]);
		descriptionText.text = parsed [0];
		itemDescriptionText.text = parsed[1];
	}

	private void StoreItem(int itemNo)
	{
		print (itemNo+" "+(1 << itemNo));
		descriptionText.text = parsed [itemNo + 1];
	}

	private void StoreItem(GameObject button, int itemNo)
	{
		if (button == prevButton) {
			//purchase happens

			switch (currentActiveStore) {
			case 0:
				//powerups
				youdidthistoher.Instance.powerUpArray[itemNo] += 1;
				youdidthistoher.Instance.Save ();
				countText.text = "In Pocket: "+youdidthistoher.Instance.powerUpArray[itemNo];
				break;
			case 1:
				//pads	
				int cost = 100;																	//enter prices here
				if (youdidthistoher.Instance.currency >= cost && (youdidthistoher.Instance.skinAvailabilityPad & 1 << itemNo) != 1 << itemNo) {
					youdidthistoher.Instance.currency -= cost;
					youdidthistoher.Instance.skinAvailabilityPad += 1 << itemNo;
					youdidthistoher.Instance.currentSkinIndexPad = itemNo;
					youdidthistoher.Instance.Save ();
					button.transform.GetChild (0).gameObject.SetActive (false);
				} else {
					//insufficient funds
			//		coinPurchasePanel.SetActive(true);
			//		mastIdea.SetActive (false);
				}
				break;
			case 2:
				//specials
				//separate functions written
				break;
			case 3:
				//blokes
				int costB = 50;
				if (youdidthistoher.Instance.currency >= costB && (youdidthistoher.Instance.skinAvailabilityBloke & 1 << itemNo) != 1 << itemNo) {
					youdidthistoher.Instance.currency -= costB;
					youdidthistoher.Instance.skinAvailabilityBloke += 1 << itemNo;
					youdidthistoher.Instance.currentSkinIndexBloke = itemNo;
					youdidthistoher.Instance.Save ();
					print("bought");
					button.transform.GetChild (0).gameObject.SetActive (false);
				}else {
					//insufficient funds
//					coinPurchasePanel.SetActive(true);
//					mastIdea.SetActive (false);
				}
				break;
			case 4:
				//grounds
				int costG = 50;
				if (youdidthistoher.Instance.currency >= costG && (youdidthistoher.Instance.skinAvailabilityGround & 1 << itemNo) != 1 << itemNo) {
					youdidthistoher.Instance.currency -= costG;
					youdidthistoher.Instance.skinAvailabilityGround += 1 << itemNo;
					youdidthistoher.Instance.currentGround = itemNo;
					youdidthistoher.Instance.Save ();
					button.transform.GetChild (0).gameObject.SetActive (false);
				}else {
					//insufficient funds
					//					coinPurchasePanel.SetActive(true);
					//					mastIdea.SetActive (false);
				}
				break;
			default:
				break;
			}

		} 


		else {
			//fist click, confirmation needed or selection
			prevButton = button;
			print (itemNo + " " + (1 << itemNo)+" "+button);
			itemDescriptionText.text = parsed [itemNo + 2];
			confirmBuy.transform.parent.GetComponent<Animation> ().Play ("confirmBuy");
			switch (currentActiveStore) {
			case 0:
				//powerups
				countText.text = "In Pocket: "+youdidthistoher.Instance.powerUpArray[itemNo];
				break;
			case 1:
				//pads
				if ((youdidthistoher.Instance.skinAvailabilityPad & 1 << itemNo) == 1 << itemNo) {
					//runs to select the already bought materials
					youdidthistoher.Instance.currentSkinIndexPad = itemNo;
					youdidthistoher.Instance.Save ();			
				}
				break;
			case 2:
				//specials
				//separate functions written
				break;
			case 3:
				//blokes
				if ((youdidthistoher.Instance.skinAvailabilityBloke & 1 << itemNo) == 1 << itemNo) {
					//runs to select the already bought materials

					print("selected");
					youdidthistoher.Instance.currentSkinIndexBloke = itemNo;
					youdidthistoher.Instance.Save ();			
				}
				break;
			case 4:
				//grounds
				if ((youdidthistoher.Instance.skinAvailabilityGround & 1 << itemNo) == 1 << itemNo) {
					//runs to select the already bought materials

					youdidthistoher.Instance.currentGround = itemNo;
					youdidthistoher.Instance.Save ();			
				}
				break;
			default:
				break;
			}
		}
	}


	private void LoadMenu(int level)
	{
		if (!levelSelected) 
		{
			print ("level "+level);

			int row = (level%(NO_OF_ROWS*NO_OF_COLUMNS))/NO_OF_COLUMNS;
			if (row % NO_OF_COLUMNS == 0 && row != 0) {														//row correction
				row--;
			}
			int column = (level%(NO_OF_ROWS*NO_OF_COLUMNS))%NO_OF_COLUMNS;
			if (levelsUnlocked < ((levelCount / (NO_OF_ROWS * NO_OF_COLUMNS)) + 1) * (NO_OF_ROWS * NO_OF_COLUMNS)) {		//currentLevelPage Correction of One block ahead selection
				youdidthistoher.Instance.currentPlayingLevel = level;
				column--;
				if (column == -1) {
					column = NO_OF_COLUMNS - 1;
					row--;
					if (row == -1) {
						row = NO_OF_ROWS - 1;
					}
				}
			} else {
				youdidthistoher.Instance.currentPlayingLevel = level + 1;											//load current level
			}
			print ("row "+row+" column "+column);
			GameObject thisButton = levels [row, column];
			if (thisButton.transform.GetChild (2).gameObject.activeInHierarchy) {
				//locked Button Click
				thisButton.GetComponent<Animation> ().Play ("lockedButtonCampaign");					//Locked Anim
			} else {
				//level Selected
				thisButton.GetComponent<Animation> ().Play ("levelButtonSelect");						//ButtonSelectAnim
				bckLevelSelection.GetComponent<Button> ().interactable = false;
				levelSelected = true;
				print ("here");
				Invoke ("loadScene", 0.5f);
			}
		}
	}

    void makeAPage(Vector3 parentOfRows)
    {

        RectTransform rowRectTransform = LevelButton.GetComponent<RectTransform>();
        RectTransform containerRectTransform = LevelPage.GetComponent<RectTransform>();

        float width = containerRectTransform.rect.width / NO_OF_COLUMNS;
        float ratio = width / rowRectTransform.rect.width;
        float height = rowRectTransform.rect.height * ratio;

        for (int i = 0; i < NO_OF_ROWS; i++)
        {
            for (int j = 0; j < NO_OF_COLUMNS; j++)
            {
                levels[i, j] = Instantiate(LevelButton) as GameObject;
                levels[i, j].name = LevelPage.name + " item at (" + i + "," + j + ")";
                //levels[i, j].transform.parent = LevelPage.transform;
				levels [i, j].transform.SetParent (LevelPage.transform);

                levels[i, j].transform.GetChild(1).GetComponent<Text>().text = levelCount++.ToString();
                int temp = levelCount - 1;
                levels[i, j].GetComponent<Button>().onClick.AddListener(() => LoadMenu(temp));

                if (levelCount <= levelsUnlocked+1)
                {
                    levels[i, j].transform.GetChild(2).gameObject.SetActive(false);
                    blockMovement = false;
                }
                else
                {
                    levels[i, j].transform.GetChild(2).gameObject.SetActive(true);
                    blockMovement = true;
                }
                if (levelCount - 1 == levelsUnlocked)
                {                                                           //select component
                    levels[i, j].transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    levels[i, j].transform.GetChild(0).gameObject.SetActive(false);
                }

                float offsetRatio = 0.1f;
                RectTransform rectTransform = levels[i, j].GetComponent<RectTransform>();

                float x = -containerRectTransform.rect.width / 2 + width * (j) + width * offsetRatio;
                float y = containerRectTransform.rect.height / 2 - height * (i + 1) + height * offsetRatio;
                rectTransform.offsetMin = new Vector2(x, y);

                x = rectTransform.offsetMin.x + width - 2 * width * offsetRatio;
                y = rectTransform.offsetMin.y + height - 2 * height * offsetRatio;
                rectTransform.offsetMax = new Vector2(x, y);

                rectTransform.localScale = new Vector3(1f, 1f, 1f);

                levels[i, j].transform.GetChild(2).GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f) * ((1 - 2 * offsetRatio));
            }

        }
    
    }

    void Update () {
		
	}

    public void Campaign()
    {
        youdidthistoher.Instance.gameplayType = 0;
    }

    public void Endless()
    {
		campaignButton.GetComponent<Button> ().interactable = false;
		practiceButton.GetComponent<Button> ().interactable = false;
		bckGameSelection.GetComponent<Button> ().interactable = false;
        youdidthistoher.Instance.gameplayType = 1;
		practiceButton.transform.GetChild(1).gameObject.GetComponent<ButtonBackRotatorClockwise>().enabled = false;
		campaignButton.transform.GetChild(1).gameObject.GetComponent<ButtonBackRotatorAnticlockwise>().enabled = false;
		endlessButton.transform.GetChild (1).gameObject.GetComponent<ButtonBackRotatorClockwise> ().rateIncrease();
		endlessButton.GetComponent<Animation> ().Play("endless");
		campaignButton.GetComponent<Animation> ().Play ("campaignRight");
		practiceButton.GetComponent<Animation> ().Play ("practiceRight");
		Invoke ("loadScene", gameSelectionWaitTime);
    }

    public void Practice()
    {
		campaignButton.GetComponent<Button> ().interactable = false;
		endlessButton.GetComponent<Button> ().interactable = false;
		bckGameSelection.GetComponent<Button> ().interactable = false;
		youdidthistoher.Instance.gameplayType = 2;
		campaignButton.transform.GetChild(1).gameObject.GetComponent<ButtonBackRotatorAnticlockwise>().enabled = false;
		endlessButton.transform.GetChild(1).gameObject.GetComponent<ButtonBackRotatorClockwise>().enabled = false;
		practiceButton.transform.GetChild (1).gameObject.GetComponent<ButtonBackRotatorClockwise> ().rateIncrease();
		practiceButton.GetComponent<Animation> ().Play("practice");
		campaignButton.GetComponent<Animation> ().Play ("campaignLeft");
		endlessButton.GetComponent<Animation> ().Play ("endlessLeft");
		Invoke ("loadScene", gameSelectionWaitTime);
    }

	void loadScene()
	{
		SceneManager.LoadScene("Pong_Breaker");
	}

	public void quit()
	{
		Application.Quit ();
	}

	public void nextPageLevel()
	{
		if (!blockMovement) {
			if (lastSwitch == -1)
				levelCount += 20;
			lastSwitch = 1;
			for (int i = 0; i < NO_OF_ROWS; i++) {
				for (int j = 0; j < NO_OF_COLUMNS; j++) {
					levels [i, j].transform.GetChild (1).GetComponent<Text> ().text = levelCount++.ToString ();
					if (levelCount-1 == levelsUnlocked) {															//select component
						levels [i, j].transform.GetChild (0).gameObject.SetActive (true);
					} else {
						levels [i, j].transform.GetChild (0).gameObject.SetActive (false);
					}
					levels [i, j].GetComponent<Button> ().onClick.RemoveAllListeners ();
					int temp = levelCount - 1;
					levels [i, j].GetComponent<Button> ().onClick.AddListener (() => LoadMenu (temp));
					if (levelCount<=levelsUnlocked) {
						levels [i, j].transform.GetChild (2).gameObject.SetActive (false);
						blockMovement = false;
					} 
					else {
						if (levelCount-1 == levelsUnlocked)
							levels [i, j].transform.GetChild (2).gameObject.SetActive (false);	
						else
							levels [i, j].transform.GetChild (2).gameObject.SetActive (true);
						blockMovement = true;
					}
				}
			}
		}
	}

	public void previousPageLevel()
	{
		if (levelCount>NO_OF_ROWS*NO_OF_COLUMNS) {
				blockMovement = false;
				if (lastSwitch == 1)
					levelCount -= NO_OF_ROWS*NO_OF_COLUMNS;
				lastSwitch = -1;
				for (int i = NO_OF_ROWS - 1; i >= 0; i--) {
					for (int j = NO_OF_COLUMNS - 1; j >= 0; j--) {	
						levels [i, j].transform.GetChild (1).GetComponent<Text> ().text = (--levelCount).ToString ();
						if (levelCount == levelsUnlocked) {															//select component
							levels [i, j].transform.GetChild (0).gameObject.SetActive (true);
						} else {
							levels [i, j].transform.GetChild (0).gameObject.SetActive (false);
						}
						levels [i, j].GetComponent<Button> ().onClick.RemoveAllListeners ();
						int temp = levelCount - 1;
						levels [i, j].GetComponent<Button> ().onClick.AddListener (() => LoadMenu (temp));

						if (levelCount <= levelsUnlocked) {
							levels [i, j].transform.GetChild (2).gameObject.SetActive (false);
						} else {
							levels [i, j].transform.GetChild (2).gameObject.SetActive (true);
						}
					}
				}
			}
	}

	void resetShop()
	{
		PowerupsPanel.SetActive (false);
		PadsPanel.SetActive (false);
		SpecialPanel.SetActive (false);
		BlokesPanel.SetActive (false);
		GroundsPanel.SetActive (false);
	}

	public void shopStarter()
	{
		powerUps ();	
	}

	public void powerUps()
	{
		resetShop ();
		PowerupsPanel.SetActive (true);
		countText.gameObject.SetActive (true);
		currentActiveStore = 0;
		txt = (TextAsset)Resources.Load ("Text/powerups",typeof(TextAsset));
		parser (txt.text);
	}

	public void pads()
	{
		resetShop ();
		PadsPanel.SetActive (true);
		countText.gameObject.SetActive (false);
		currentActiveStore = 1;
		txt = (TextAsset)Resources.Load ("Text/pads",typeof(TextAsset));
		parser (txt.text);
	}

	public void specials()
	{
		resetShop ();
		SpecialPanel.SetActive (true);
		countText.gameObject.SetActive (false);
		currentActiveStore = 2;
		txt = (TextAsset)Resources.Load ("Text/specials",typeof(TextAsset));
		parser (txt.text);
	}

	public void blokes()
	{
		resetShop ();
		BlokesPanel.SetActive (true);
		countText.gameObject.SetActive (false);
		currentActiveStore = 3;
		txt = (TextAsset)Resources.Load ("Text/blokes",typeof(TextAsset));
		parser (txt.text);
	}

	public void grounds()
	{
		resetShop ();
		GroundsPanel.SetActive (true);
		countText.gameObject.SetActive (false);
		currentActiveStore = 4;
		txt = (TextAsset)Resources.Load ("Text/grounds",typeof(TextAsset));
		parser (txt.text);
	}

	public void OpenWebsite()
	{
		Debug.Log ("hello");
		Application.OpenURL ("http://www.bizarregamestudios.com");
	}

	public void OpenFacebook()
	{
		Application.OpenURL ("https://www.facebook.com/bizarregamestudios");
	}

	public void OpenInsta()
	{
		Application.OpenURL ("https://www.instagram.com/bizarregamestudios");
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

	void resetCameraButtons()
	{
		for (int i = 0; i < 3; i++) 
		{
			cameraButton.transform.GetChild (i).transform.GetChild(0).gameObject.SetActive (false);
		}
	}

	public void cameraButtonController(int param)
	{
		// 0 = dynamic, 1= topdown, 2= fps
		resetCameraButtons ();
		cameraButton.transform.GetChild (param).transform.GetChild (0).gameObject.SetActive (true);
		youdidthistoher.Instance.currentCameraMode = param;
		youdidthistoher.Instance.Save ();
	//	print (param);
	}

	void resetControlButtons()
	{
		for (int i = 0; i < 3; i++) 
		{
			controlButton.transform.GetChild (i).transform.GetChild(0).gameObject.SetActive (false);
		}
	}

	public void controlButtonController(int param)
	{
		// 0 = joystick, 1= hand, 2= gyro
		resetControlButtons ();
		controlButton.transform.GetChild (param).transform.GetChild(0).gameObject.SetActive (true);
		PlayerPrefs.SetInt ("InputType",param);
	//	print (param);
	}

	public void pattButt()
	{
		patt.SetActive (true);
	}
}
