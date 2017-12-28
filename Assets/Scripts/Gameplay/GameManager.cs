using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private static GameManager instance;
	public static GameManager Instance{get{ return instance;}}
	private const float COIN_PROB=0.3f;
	private const float POWER_UP_PROB = 0.12f;
	private const float SPAWN_RATE = 3f;
	private const float BLOKE_MIN_SPAWNX = -3f;
	private const float BLOKE_MAX_SPAWNX = 3f;
	private const float	BLOKE_MIN_SPAWNZ = -7.5f;
	private const float BLOKE_MAX_SPAWNZ = 5f;
	private const int WIN_LIMIT = 7;
	private const float BLOKE_MULTIPLIER = 1f;
	private const float WALL_MULTIPLIER = 3f;
	private float PAD_MULTIPLIER = 1f;
	private const float BLOKE_HEIGHT = 1f;
	private const float BLOKE_WIDTH=0.5f;
	private const float BLOKE_HEIGHT_FROM_GROUND = 0.4f;
	private const int TYPE_OF_BLOKE=7;
	private const int CORD_X_MAX = 12 ;
	private const int CORD_Z_MAX= 16;
    
	public bool BlokeHit;
	public bool GameOver;

	public GameObject padLong,padShort,bigBall,speedUp,speedDown,flareBall,multiBall,magnetPad,gunPad,VIPBall;
	public GameObject coin;
	public Transform BlokeGroup,DeadPool,BallContainer;
	public GameObject g,w1,w2,w3,w4;
	public GameObject camera1,camera2,camera3;

	private int currentPlayingLevel;
    private float initialTime;

	private List  <GameObject>  BlastList; 

    private GameObject[] ballList;
    public GameObject specials, MCD, drunk;
	public Material[] blockMaterial;

	public AudioSource a;
	public AudioClip a1,a2,a3,a4;
	public GameObject BlastAnim;

	public GameObject gameCanvas, powerup, gameoverAnimationButton;

	public int coinCount;
	private float AI_Point, player_Point;
	public float AI_BlokePoint,player_BlokePoint;
	public float AI_WallPoint, player_WallPoint;
	public GameObject ground;
	public GameObject AI_Score,player_Score;
	private bool ShowInterAd;
	private float gameStartTime;

	public Text coinsEarned;

	public float HighScore;
	private bool HighScoreTuta;

    void Start () {
		gameStartTime = Time.time;
		AI_Point = player_Point = AI_BlokePoint = player_BlokePoint = AI_WallPoint = player_WallPoint =0;
		GameOver = false;
		if (youdidthistoher.Instance.MCDActive == 1) {
			PAD_MULTIPLIER = 0.5f;
		} else if (youdidthistoher.Instance.DrunkActive == 1) {
			PAD_MULTIPLIER = 2f;
		}
		instance = this;
		DeadPool = GameObject.Find ("DeadPool").transform;
		resetTriggers ();												//flare powerup reset correction
		BlastList = new List<GameObject> ();
        Time.timeScale = 1f;
		////////
		/// 
//		AdManager.Instance.HideBanner();
		/// /////
		//youdidthistoher.Instance.loader ();
		g.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [youdidthistoher.Instance.currentGround];
	//	w1.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [youdidthistoher.Instance.currentWall];
	//	w2.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [youdidthistoher.Instance.currentWall];
	//	w3.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [youdidthistoher.Instance.currentWall];
	//	w4.GetComponent<Renderer> ().material = youdidthistoher.Instance.extraMaterials [youdidthistoher.Instance.currentWall];

		camReset ();
		switch (youdidthistoher.Instance.currentCameraMode) {
		case 0:
			camera1.SetActive (true);
			break;
		case 1:
			camera2.SetActive (true);
			break;
		case 2:
			camera3.SetActive (true);
			break;
		}

		if(youdidthistoher.Instance.gameplayType == 0){
			currentPlayingLevel = youdidthistoher.Instance.currentPlayingLevel;
            LevelMaker(currentPlayingLevel);
            ExtraFeatures();
        }
		else if(youdidthistoher.Instance.gameplayType==1){
			InvokeRepeating ("BlokeSpawner", SPAWN_RATE, SPAWN_RATE);
		}

		if (youdidthistoher.Instance.gameplayType<=1)
        {
            if (youdidthistoher.Instance.MCDActive == 1)
            {
                GameObject.FindGameObjectWithTag("player").SetActive(false);
                MCD.SetActive(true);
            }
            else if (youdidthistoher.Instance.DrunkActive == 1)
            {
                GameObject.FindGameObjectWithTag("player").SetActive(false);
                drunk.SetActive(true);
            }
        }
      
    }

	void resetTriggers()
	{
		foreach (Transform t in DeadPool) 
		{
			t.gameObject.GetComponent<BoxCollider> ().isTrigger = false;
			t.SetParent (DeadPool);
		}
	}

	void camReset()
	{
		camera1.SetActive (false);
		camera2.SetActive (false);
		camera3.SetActive (false);
	}

    void Update()
	{	if(GameOver) return;

		if (youdidthistoher.Instance.gameplayType==0) {
			//current Level
			coinsEarned.text = coinCount.ToString ();
			player_Point = player_BlokePoint * BLOKE_MULTIPLIER + (AI_WallPoint)*WALL_MULTIPLIER;
			AI_Point = AI_BlokePoint * BLOKE_MULTIPLIER+(player_WallPoint)*WALL_MULTIPLIER;
			player_Point *= PAD_MULTIPLIER;

			player_Score.GetComponent<Text> ().text = player_Point + "";
			AI_Score.GetComponent<Text> ().text = AI_Point+"";
			if (player_Point > AI_Point) {
				player_Score.GetComponent<Text> ().color = Color.green;
			} else {
				player_Score.GetComponent<Text> ().color = Color.red;
			}

			if (BlokeRemaining() == 0) {
				//if(false){	

				int levelLooseCount=PlayerPrefs.GetInt("levelLooseCount");
				levelLooseCount++;												//level end count
				if (levelLooseCount % 4 == 0) {
					ShowInterAd = true;
				}
				youdidthistoher.Instance.LevelLooseCount = levelLooseCount;
				GameOver = true;
				if (player_Point > AI_Point) {

					int levelReached =PlayerPrefs.GetInt ("campaignLevelReached");
					int currentLevel = PlayerPrefs.GetInt ("currentPlayingLevel");

					if(currentLevel ==levelReached){
						youdidthistoher.Instance.campaignLevelReached = ++levelReached;
						youdidthistoher.Instance.Save ();
						if ((levelReached) % 5 == 0) {
							//////
							//							AdManager.Instance.ShowVideo ();
							/////
							ShowInterAd=false;

						}
					}
					//player is winner
					gogoScreen(0);
				}else{
					gogoScreen(1);
				}				
			}
		} else if (youdidthistoher.Instance.gameplayType==1) {
			if (Time.time - gameStartTime > 100f) {
				ShowInterAd = true;
			}

			coinsEarned.text = coinCount.ToString ();
			player_Point = player_BlokePoint * BLOKE_MULTIPLIER + (AI_WallPoint)*WALL_MULTIPLIER ;
			AI_Point = AI_BlokePoint * BLOKE_MULTIPLIER + (player_WallPoint )*WALL_MULTIPLIER;
			player_Point *= PAD_MULTIPLIER;
			player_Score.GetComponent<Text> ().text = player_Point + "";			
			AI_Score.GetComponent<Text> ().text = AI_Point+"";
			if (player_WallPoint >= WIN_LIMIT) {
				GameOver = true;
				if (youdidthistoher.Instance.HighScore < player_Point) {
					youdidthistoher.Instance.HighScore = (int)player_Point;
					youdidthistoher.Instance.Save ();
				}
				if (player_Point >= AI_Point) {
					gogoScreen(0);
				}else{
					gogoScreen(1);
				}
			}		
		}
		EmptyBlastList ();
    }

	void BlokeSpawner() {
		int CordX, CordZ,CordType;
		CordX = Random.Range (0, CORD_X_MAX);
		CordZ = Random.Range (0, CORD_Z_MAX);
		CordType = Random.Range (0,3);

		Vector3 pos = new Vector3 (BLOKE_MIN_SPAWNX + CordX * BLOKE_WIDTH, BLOKE_HEIGHT_FROM_GROUND, BLOKE_MIN_SPAWNZ + CordZ * BLOKE_HEIGHT);
		bool canSpawn = true;
		Transform[] childObjects = BlokeGroup.GetComponentsInChildren<Transform> ();
		foreach (Transform temp in childObjects) {
			if (temp.gameObject.activeInHierarchy && temp.position == pos) {
				canSpawn = false;
			}
		}
		if (canSpawn) {
			GameObject obj = ObjectPool.Instance.GetObject ();
			obj.transform.position = pos;
			obj.GetComponent<Block> ().SetBlock (CordType);
		} else {
			BlokeSpawner ();
		}
	}
	void LevelMaker(int levelNo)
	{
		string level = PlayerPrefs.GetString("level" + levelNo);
		foreach (string blokeName in level.Split('*'))
		{
			string[] blokeValues = blokeName.Split('-');
			int type, i, j;
			int.TryParse(blokeValues[0], out type);
			int.TryParse(blokeValues[1], out i);
			int.TryParse(blokeValues[2], out j);

			//BlokeGroup.Find("Bloke" + type + "-" + i.ToString() + "X-" + j.ToString() + "Z").gameObject.SetActive(true);
			GameObject obj = ObjectPool.Instance.GetObject();
			obj.transform.position = new Vector3 (BLOKE_MIN_SPAWNX+i*BLOKE_WIDTH, BLOKE_HEIGHT_FROM_GROUND ,BLOKE_MIN_SPAWNZ+j*BLOKE_HEIGHT);
			obj.GetComponent<Block> ().SetBlock (type-1);
		}
	}

    private void ExtraFeatures()
    {
        ballList = PowerUp.Instance.ballList;
        if (currentPlayingLevel % 10 == 0)
        {
            foreach (GameObject ball in ballList)
            {
                ball.transform.GetChild(3).gameObject.SetActive(true);
                ball.transform.GetChild(7).gameObject.SetActive(true);
            }
            specials.SetActive(true);
            specials.transform.GetChild(0).gameObject.SetActive(true);
        }
    }


    // make coins at the bloke places
    public void makeCoin(GameObject tempBloke){
	    //Random.Range (0f, 1f);

		if (Random.Range(0, 100) < COIN_PROB*100 ) {
			Instantiate (coin, tempBloke.transform.position, Quaternion.Euler (0, 0, 90));
		}
	}
    // power up created at bloke places	
	public void makePowerUp(GameObject tempBloke,bool turn){
		if (youdidthistoher.Instance.MCDActive == 1) {
			return;
		}
		float chance = Random.Range (0f, 1f);
		if (chance < POWER_UP_PROB) {

			int powerChoice = Random.Range (0, 33);
			//powerChoice = 7;
			var pu=new GameObject();
            if (powerChoice < 4) pu = Instantiate(padLong, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 8) pu = Instantiate(padShort, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 12) pu = Instantiate(bigBall, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 16) pu = Instantiate(speedUp, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 20) pu = Instantiate(speedDown, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 23) pu = Instantiate(flareBall, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 26) pu = Instantiate(multiBall, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 29) pu = Instantiate(VIPBall, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 31) pu = Instantiate(gunPad, tempBloke.transform.position, Quaternion.identity);
            else if (powerChoice < 33) pu = Instantiate(magnetPad, tempBloke.transform.position, Quaternion.identity);
            else return;
			pu.GetComponent<rotator>().turn = turn;

        }

    }

    
	public void Blast(GameObject blokeTemp){
		Transform[] childObjects = BlokeGroup.GetComponentsInChildren<Transform> ();
		Vector3 pos = blokeTemp.transform.position;
		foreach (Transform temp in childObjects) {
			if (temp == BlokeGroup)
				continue;

			for (int i = (int)pos.x - 1; i <= (int)pos.x + 1; i++) {
				for (int j = (int)pos.z - 1; j <= (int)pos.z + 1; j++) {
					if (i == (int)pos.x && j == (int)pos.z) {
						continue;			
					}
					if (temp.position.x == i && temp.position.z == j) {
						if (!BlastList.Contains (temp.gameObject)) {
							BlastList.Add (temp.gameObject);
						}
					}
				}
			}
		}

	}

	private void EmptyBlastList(){
		if (BlastList.Count > 0) {
			GameObject temp = BlastList [0];
			if(BlastList.Remove (BlastList [0])){
				if (temp.GetComponent<Block>().blockType == BlockTypes.Blast)
					Blast (temp);
			}
		}
	}

	public void gogoScreen(int state)
	{	/////
		if(ShowInterAd){
			ShowInterAd = false;
			/////
			//		AdManager.Instance.ShowInterstitial();
			///// 
		}

		/////
		//	AdManager.Instance.ShowBanner();
		/////
		ObjectPool.Instance.Reset();

		coinsEarned.gameObject.transform.parent.gameObject.SetActive(false);
		gameoverAnimationButton.GetComponent<UIAnimController>().PanelActive();
		gameCanvas.GetComponent<GameUI> ().gameOver(state, GameManager.Instance.coinCount);
		for (int i = 0; i < powerup.GetComponent<PowerUp> ().ballList.Length; i++)
			powerup.GetComponent<PowerUp> ().ballList [i].SetActive (false);
	}

	private int BlokeRemaining(){
		int count = 0;
		foreach (Transform t in BlokeGroup.transform) {
			if (t.gameObject.activeSelf) {
				Block temp = t.GetComponent<Block> ();
				if(temp!=null && ((int)temp.blockType<3||temp.blockType==BlockTypes.Blast))
					count++;
			}
		}
		return count;
	}
	public void BlokePoint(bool turn){
		if (turn) {
			player_BlokePoint++;	
		}
		else {
			AI_BlokePoint++;
		}
	}
	public void BlastAnimation(Vector3 pos){
		GameObject temp = Instantiate (BlastAnim, pos, Quaternion.identity);
		Destroy (temp, 3f);
	}
}
