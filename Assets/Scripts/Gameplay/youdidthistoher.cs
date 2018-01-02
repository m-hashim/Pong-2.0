﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class youdidthistoher : MonoBehaviour {

	private const int NO_OF_MATERIALS = 10;
	private const int NO_OF_MATERIALS_BLOKE = 15;

	private static youdidthistoher instance;
	public static youdidthistoher Instance{get{ return instance; }}

	public Material[] materials; 
	public Material[] blokeMaterials;
	public Sprite[] pads;
	public Sprite[] blokes;
	public Sprite[] extras;
	public Sprite[] powerUps;

	public int currentSkinIndexPad=0;
	public int currentSkinIndexBloke = 0;
	public int currentGround = 0;
	public int currency=2000;
	public int p_bigBall = 5;
	public int p_flareBall = 5;
	public int p_gun = 5;
	public int p_padLong = 5;
	public int p_magnet = 5;
	public int p_VIPBall = 5;
	public int p_multiBall = 5;
	public int skinAvailabilityPad=1;
	public int skinAvailabilityBloke=1;
	public int skinAvailabilityGround=1;
	public int MCDActive=0;
	public int DrunkActive=0;
	public int skinAvailabilityMCD = 0;
	public int skinAvailabilityDrunk = 0;
	public int hasRatedGame = 0;
	public int gameOpenCount = 1;
	public int currentPlayingLevel = 1;
	public int HighScoreEndless = 0;
	public int HighScoreDark = 0;
	public int LevelLooseCount=1;
	public int backgroundMusic = 1;
	public int effectsSound =1;
	public int gameplayType = 0;

	public int padPriceDisplay = 100;
	public int blokePriceDisplay = 50;
	public int extraPriceDisplay = 150;
	public int powerUpPriceDisplay = 30;

	public int currentCameraMode=2;		//0 for dynamic, 2 for first person
	public int[] powerUpArray;
	public int campaignLevelReached=80;
	public string[] level;

	public bool startGame=false;

	void Awake () {
		instance = this;
		DontDestroyOnLoad(gameObject);
		materials = new Material[NO_OF_MATERIALS]; 
		blokeMaterials = new Material[NO_OF_MATERIALS_BLOKE];
		powerUpArray = new int[7];
//		PlayerPrefs.DeleteAll ();
		materials = Resources.LoadAll<Material> ("Material/Pad_Materials");
		blokeMaterials = Resources.LoadAll<Material> ("Material/Bloke_Materials");
		pads = Resources.LoadAll<Sprite> ("Pads");
		blokes = Resources.LoadAll<Sprite> ("Blokes");
		extras = Resources.LoadAll<Sprite> ("Extras");
		powerUps = Resources.LoadAll<Sprite> ("Power");
		TextAsset txt = (TextAsset)Resources.Load ("LevelStore", typeof(TextAsset));
		string content = txt.text;
		level = content.Split ('\n');
		if (PlayerPrefs.HasKey ("Currency")) {
			//We had a previous session
			currentSkinIndexPad = PlayerPrefs.GetInt("CurrentSkinPad");
			currentCameraMode = PlayerPrefs.GetInt ("currentCameraMode");
			currentSkinIndexBloke = PlayerPrefs.GetInt("CurrentSkinBloke");
			currency = PlayerPrefs.GetInt ("Currency");
			currentGround = PlayerPrefs.GetInt ("currentGround");
			skinAvailabilityPad = PlayerPrefs.GetInt ("SkinAvailabilityPad");
			skinAvailabilityBloke = PlayerPrefs.GetInt ("SkinAvailabilityBloke");
			skinAvailabilityGround = PlayerPrefs.GetInt ("SkinAvailabilityGround");
			skinAvailabilityMCD = PlayerPrefs.GetInt ("SkinAvailabilityMCD");
			skinAvailabilityDrunk = PlayerPrefs.GetInt ("SkinAvailabilityDrunk");
			MCDActive = PlayerPrefs.GetInt ("MCDActive");
			DrunkActive = PlayerPrefs.GetInt ("DrunkActive");
			backgroundMusic = PlayerPrefs.GetInt ("backgroundMusic");
			effectsSound = PlayerPrefs.GetInt ("effectsSound");
			hasRatedGame = PlayerPrefs.GetInt ("hasRatedGame");
			gameOpenCount = PlayerPrefs.GetInt ("gameOpenCount");
			LevelLooseCount = PlayerPrefs.GetInt ("levelLooseCount");
			powerUpArray [0] = PlayerPrefs.GetInt ("p_bigBall");
			powerUpArray [1] = PlayerPrefs.GetInt ("p_flareBall");
			powerUpArray [2] = PlayerPrefs.GetInt ("p_gun");
			powerUpArray [3] = PlayerPrefs.GetInt ("p_padLong");
			powerUpArray [4] = PlayerPrefs.GetInt ("p_magnet");
			powerUpArray [5] = PlayerPrefs.GetInt ("p_VIPBall");
			powerUpArray [6] = PlayerPrefs.GetInt ("p_multiBall");
			currentPlayingLevel = PlayerPrefs.GetInt ("currentPlayingLevel");
			campaignLevelReached = PlayerPrefs.GetInt ("campaignLevelReached");
			HighScoreEndless = PlayerPrefs.GetInt ("HighScoreEndless");
			HighScoreDark = PlayerPrefs.GetInt ("HighScoreDark");
			gameplayType = PlayerPrefs.GetInt ("gameplayType");
			gameOpenCount++;

		} else{
			//pehli baar chalega bencho
				for(int i=0;i<7;i++)
					powerUpArray[i]=7;
			Save();
			PlayerPrefs.SetInt ("currentPlayingLevel", currentPlayingLevel);
			PlayerPrefs.SetInt ("campaignLevelReached", campaignLevelReached);
			for (int i = 1; i<=level.Length; i++) {
				PlayerPrefs.SetString ("level" + i, level [i - 1]);
			}
			PlayerPrefs.SetInt ("HighScoreEndless", HighScoreEndless);
			PlayerPrefs.SetInt ("HighScoreDark", HighScoreDark);
		}

	/*	if (backgroundMusic == 1)
			AudioListener.volume = 1f;
		else
			AudioListener.volume = 0f;
*/
		if (!(PlayerPrefs.HasKey ("cameraChanging"))) 
		{
			PlayerPrefs.SetInt ("cameraChanging", 0);
			PlayerPrefs.SetInt ("currentCameraMode",2);
			currentCameraMode = 2;
		}
	}
		
	public void Save()
	{
		PlayerPrefs.SetInt ("CurrentSkinPad", currentSkinIndexPad);
		PlayerPrefs.SetInt ("CurrentSkinBloke", currentSkinIndexBloke);
		PlayerPrefs.SetInt ("Currency", currency);
		PlayerPrefs.SetInt ("currentGround", currentGround);
		PlayerPrefs.SetInt ("currentCameraMode", currentCameraMode);
		PlayerPrefs.SetInt ("SkinAvailabilityPad", skinAvailabilityPad);
		PlayerPrefs.SetInt ("SkinAvailabilityBloke", skinAvailabilityBloke);
		PlayerPrefs.SetInt ("SkinAvailabilityGround", skinAvailabilityGround);
		PlayerPrefs.SetInt ("SkinAvailabilityMCD", skinAvailabilityMCD);
		PlayerPrefs.SetInt ("SkinAvailabilityDrunk", skinAvailabilityDrunk);
		PlayerPrefs.SetInt ("p_bigBall", powerUpArray [0]);
		PlayerPrefs.SetInt ("p_flareBall", powerUpArray [1]);
		PlayerPrefs.SetInt ("p_gun", powerUpArray [2]);
		PlayerPrefs.SetInt ("p_padLong", powerUpArray [3]);
		PlayerPrefs.SetInt ("p_magnet", powerUpArray [4]);
		PlayerPrefs.SetInt ("p_VIPBall", powerUpArray [5]);
		PlayerPrefs.SetInt ("p_multiBall", powerUpArray [6]);
		PlayerPrefs.SetInt ("MCDActive", MCDActive);
		PlayerPrefs.SetInt ("DrunkActive", DrunkActive);
		PlayerPrefs.SetInt ("backgroundMusic", backgroundMusic);
		PlayerPrefs.SetInt ("effectsSound", effectsSound);
		PlayerPrefs.SetInt ("HighScoreEndless", HighScoreEndless);
		PlayerPrefs.SetInt ("HighScoreDark", HighScoreDark);
		PlayerPrefs.SetInt ("hasRatedGame", hasRatedGame);
		PlayerPrefs.SetInt ("gameOpenCount", gameOpenCount);
		PlayerPrefs.SetInt ("levelLooseCount", LevelLooseCount);
		PlayerPrefs.SetInt ("currentPlayingLevel", currentPlayingLevel);
		PlayerPrefs.SetInt ("campaignLevelReached", campaignLevelReached);
		PlayerPrefs.SetInt ("gameplayType", gameplayType);


	//	print ("currentPlayingLevel "+currentPlayingLevel);
	//	print ("campaignLevelReached "+campaignLevelReached);
	}
/*
	public void backgroundSound()
	{
		if (youdidthistoher.Instance.backgroundMusic == 1) {
			AudioListener.volume = 0f;
			backgroundMusic = 0;
		} else {
			AudioListener.volume = 1f;
			backgroundMusic = 1;
		}
		Save ();
	}
*/
}