using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

	private const int NO_OF_ROWS = 4;
	private const int NO_OF_COLUMNS = 5;
	private const int NO_OF_PAGES = 20;
	private const int POWERUP_COUNT = 5;
	private const int GROUND_COUNT = 4;
	private const int BLOKES_COUNT = 3;
	private const int SPECIAL_COUNT = 2;
	private const int PAD_COUNT = 1;

	private bool blockMovement,firstBlock;
	private int levelCount=1;
	private int levelsUnlocked = 52;
	private int lastSwitch=0;
	private int tempSound = 1;

	private string[] parsed;

	private TextAsset txt;

	public Text  descriptionText;

	public GameObject mainPanel, gameSelectionPanel, settingsPanel, aboutPanel, helpPanel, shopPanel;
	public GameObject LevelPage, LevelButton, LevelRow, StoreButton;
	public GameObject GroundsPanel, BlokesPanel, PowerupsPanel, PadsPanel, SpecialPanel;
	public GameObject soundButton, cameraButton, controlButton;

	public Sprite[] powerUpSprites = new Sprite[POWERUP_COUNT];
	public Sprite[] groundSprites = new Sprite[GROUND_COUNT];
	public Sprite[] blokeSprites = new Sprite[BLOKES_COUNT];
	public Sprite[] specialSprites = new Sprite[SPECIAL_COUNT];
	public Sprite[] padSprites = new Sprite[PAD_COUNT];

	private GameObject[] row = new GameObject[NO_OF_ROWS];
	private GameObject[,] levels = new GameObject[NO_OF_ROWS,NO_OF_COLUMNS];

	void Awake () 
	{
		parsed = new string[15];			
		firstBlock=false;
		makeAPage (LevelPage.transform.position);									//Campaign levels
		shopInstantiator ("Text/grounds",GROUND_COUNT,GroundsPanel,groundSprites);
		shopInstantiator ("Text/blokes",BLOKES_COUNT,BlokesPanel,blokeSprites);
		shopInstantiator ("Text/specials",SPECIAL_COUNT,SpecialPanel,specialSprites);
		shopInstantiator ("Text/pads",PAD_COUNT,PadsPanel,padSprites);
		shopInstantiator ("Text/powerups",POWERUP_COUNT,PowerupsPanel,powerUpSprites);
	}


	void shopInstantiator(string address, int count, GameObject parentGameObject, Sprite[] images)
	{
		txt = (TextAsset)Resources.Load (address,typeof(TextAsset));
		parser (txt.text);
		RectTransform rowRectTransform = StoreButton.GetComponent<RectTransform>();
		RectTransform containerRectTransform = parentGameObject.GetComponent<RectTransform>();

		float width = containerRectTransform.rect.width / NO_OF_COLUMNS;
		float ratio = width / rowRectTransform.rect.width;
		float height = rowRectTransform.rect.height * ratio;
		GameObject temp;
		int i = 1;
		for (int j = 0; j < count; j++) {
			temp = GameObject.Instantiate (StoreButton, parentGameObject.transform.position, Quaternion.identity);
			temp.transform.position = parentGameObject.transform.position;
			temp.transform.SetParent (parentGameObject.transform);
			int z = j;
			temp.gameObject.GetComponent<Button> ().onClick.AddListener (() => StoreItem (z));
			temp.transform.GetChild (0).GetComponent<Text> ().text = parsed [j + 1];
			temp.GetComponent<Image> ().sprite = images [j];
		

			float offsetRatio = 0.1f;
			RectTransform rectTransform = temp.GetComponent<RectTransform> ();

			float x = -containerRectTransform.rect.width / 2 + width * (j) + width * offsetRatio;
			float y = containerRectTransform.rect.height / 2 - height * (i + 1) + height * offsetRatio;
			rectTransform.offsetMin = new Vector2 (x, y);

			x = rectTransform.offsetMin.x + width - 2 * width * offsetRatio;
			y = rectTransform.offsetMin.y + height - 2 * height * offsetRatio;
			rectTransform.offsetMax = new Vector2 (x, y);

			rectTransform.localScale = new Vector3 (1f, 1f, 1f);
		}
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
	}

	private void StoreItem(int itemNo)
	{
		print (itemNo);
		descriptionText.text = parsed [itemNo + 1];
	}

	private void LoadMenu(int level)
	{
		print (level);
        youdidthistoher.Instance.currentPlayingLevel = level;
        SceneManager.LoadScene("Pong_Breaker");

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
                levels[i, j].transform.parent = LevelPage.transform;

                levels[i, j].transform.GetChild(1).GetComponent<Text>().text = levelCount++.ToString();
                int temp = levelCount - 1;
                levels[i, j].GetComponent<Button>().onClick.AddListener(() => LoadMenu(temp));

                if (levelCount <= levelsUnlocked)
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
    /*
    void makeAPage(Vector3 parentOfRows)
    {
        for (int i = 0; i < NO_OF_ROWS; i++)
        {
            row[i] = GameObject.Instantiate(LevelRow, parentOfRows, Quaternion.identity);
            row[i].transform.parent = LevelPage.transform;
            for (int j = 0; j < NO_OF_COLUMNS; j++)
            {
                levels[i, j] = GameObject.Instantiate(LevelButton, row[i].transform.position, Quaternion.identity);
                levels[i, j].transform.parent = row[i].transform;
                if (levelCount - 1 == levelsUnlocked)
                {                                                           //select component
                    levels[i, j].transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    levels[i, j].transform.GetChild(0).gameObject.SetActive(false);
                }
                levels[i, j].transform.GetChild(1).GetComponent<Text>().text = levelCount++.ToString(); //text component
                int temp = levelCount - 1;
                levels[i, j].GetComponent<Button>().onClick.AddListener(() => LoadMenu(temp));
                if (levelCount - 1 <= levelsUnlocked)
                {                                                           //lock component
                    levels[i, j].transform.GetChild(2).gameObject.SetActive(false);
                    blockMovement = false;
                }
                else
                {
                    levels[i, j].transform.GetChild(2).gameObject.SetActive(true);
                    blockMovement = true;
                }
            }
        }

    }*/
    void Update () {
		
	}

    public void Campaign()
    {
        youdidthistoher.Instance.gameplayType = 0;
        
    }
    public void Endless()
    {
        youdidthistoher.Instance.gameplayType = 1;
        SceneManager.LoadScene("Pong_Breaker");
    }

    public void Practice()
    {
        youdidthistoher.Instance.gameplayType = 2;
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
					if (levelCount-1 <= levelsUnlocked) {
						levels [i, j].transform.GetChild (2).gameObject.SetActive (false);
						blockMovement = false;
					} else {
						levels [i, j].transform.GetChild (2).gameObject.SetActive (true);
						blockMovement = true;
					}
				}
			}
		}
	}

	public void previousPageLevel()
	{
		if (levelCount !=1) {
			blockMovement = false;
			if (lastSwitch == 1)
				levelCount -= 20;
			lastSwitch = -1;
			for (int i = NO_OF_ROWS - 1; i >= 0; i--) {
				for (int j = NO_OF_COLUMNS - 1; j >= 0; j--) {	
					levels [i, j].transform.GetChild (1).GetComponent<Text> ().text = (--levelCount).ToString ();
					if (levelCount-1 == levelsUnlocked) {															//select component
						levels [i, j].transform.GetChild (0).gameObject.SetActive (true);
					} else {
						levels [i, j].transform.GetChild (0).gameObject.SetActive (false);
					}
					levels [i, j].GetComponent<Button> ().onClick.RemoveAllListeners ();
					int temp = levelCount - 1;
					levels [i, j].GetComponent<Button> ().onClick.AddListener (() => LoadMenu (temp));

					if (levelCount-1 <= levelsUnlocked) {
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
		txt = (TextAsset)Resources.Load ("Text/powerups",typeof(TextAsset));
		parser (txt.text);
	}

	public void pads()
	{
		resetShop ();
		PadsPanel.SetActive (true);
		txt = (TextAsset)Resources.Load ("Text/pads",typeof(TextAsset));
		parser (txt.text);
	}

	public void specials()
	{
		resetShop ();
		SpecialPanel.SetActive (true);
		txt = (TextAsset)Resources.Load ("Text/specials",typeof(TextAsset));
		parser (txt.text);
	}

	public void blokes()
	{
		resetShop ();
		BlokesPanel.SetActive (true);
		txt = (TextAsset)Resources.Load ("Text/blokes",typeof(TextAsset));
		parser (txt.text);
	}

	public void grounds()
	{
		resetShop ();
		GroundsPanel.SetActive (true);
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
		if (tempSound == 1) {
			tempSound = 0;
			soundButton.transform.GetChild (0).gameObject.SetActive (false);
			soundButton.transform.GetChild (1).gameObject.SetActive (true);
		} else {
			tempSound = 1;
			soundButton.transform.GetChild (1).gameObject.SetActive (false);
			soundButton.transform.GetChild (0).gameObject.SetActive (true);
		}
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
		resetCameraButtons ();
		switch (param) 
		{
		case 0:
			cameraButton.transform.GetChild (0).transform.GetChild(0).gameObject.SetActive (true);
			break;
		case 1:
			cameraButton.transform.GetChild (1).transform.GetChild(0).gameObject.SetActive (true);
			break;
		case 2:
			cameraButton.transform.GetChild (2).transform.GetChild(0).gameObject.SetActive (true);
			break;
		}
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
		resetControlButtons ();
		switch (param) 
		{
		case 0:
			controlButton.transform.GetChild (0).transform.GetChild(0).gameObject.SetActive (true);
			break;
		case 1:
			controlButton.transform.GetChild (1).transform.GetChild(0).gameObject.SetActive (true);
			break;
		case 2:
			controlButton.transform.GetChild (2).transform.GetChild(0).gameObject.SetActive (true);
			break;
		}
	}
}
