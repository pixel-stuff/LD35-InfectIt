using UnityEngine;
using System.Collections;
using System;

public class PlayerManager : MonoBehaviour {

	#region Singleton
	public static PlayerManager m_instance;
	void Awake(){
		if(m_instance == null){
			//If I am the first instance, make me the Singleton
			m_instance = this;
		}else{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}
	#endregion Singleton

	// Use this for initialization

	public int nbDestroyCellForWin =7;
	public int nbDestroyCell;

	public int fadeDuration;

	private bool fadeBlanc;
	private bool fadeClear;
	private bool isFighting;
	private bool isWinning;
	private int m_fadeTime = 0;
	public SpriteRenderer m_fadeRenderer = null;


	void Start () {
		GameStateManager.onChangeStateEvent += handleChangeGameState;
		try{
			FindObjectOfType<CompteurManager> ().SetNumberAtStart (nbDestroyCellForWin);
		}catch(Exception e){};
	}
	
	// Update is called once per frame
	void Update () {
		if (m_fadeRenderer == null) {
			m_fadeRenderer = GameObject.FindGameObjectWithTag ("Fade").GetComponent<SpriteRenderer> ();
		} 
		if (m_fadeRenderer && (fadeBlanc || fadeClear)) {
			float step = 1f / fadeDuration;
			float actualStep = m_fadeTime++ * step;
		//	Debug.Log (actualStep);
			actualStep = (actualStep > 1f) ? 1f : actualStep;
			float alphaValue;
			if (fadeBlanc) {
				if (m_fadeTime > fadeDuration) {
					fadeBlanc = false;
					fadeBlancOver ();
					m_fadeTime = 0;
				}
				alphaValue = actualStep;
			} else {
				if (m_fadeTime > fadeDuration) {
					fadeClear = false;
					fadeClearOver ();
					m_fadeTime = 0;
				}
				alphaValue = 1f - actualStep;
			}

			Color temp = m_fadeRenderer.color;// = actualStep;
			temp.a = alphaValue;
			m_fadeRenderer.color = temp;
		}
	}


	public void addDestroyCell() {
		nbDestroyCell++;
		try{
			FindObjectOfType<CompteurManager> ().SetNumberAtStart (nbDestroyCell);
		}catch(Exception e){};
		if (nbDestroyCell == nbDestroyCellForWin) {
			GameStateManager.m_instance.setGameState (GameState.GameOver);
		}
	}
	void handleChangeGameState(GameState newState){
		Debug.Log ("PLAYER SEE THE NEW STATE : " + newState);
        if(GameStateManager.getGameState()==GameState.GameOver) {
         //   Camera.main.GetComponent<Effect_Saturation>().startSaturation(4.0f);
        }
	}

	public void FightOver(bool win) {
		isFighting = false;
		fadeBlanc = true;
		isWinning = win;
		//hide canvas fight
		//animation transparent 
		freezeGame(false);
		//If win - NOMNOM
		//if !win EJECT
	}
	void fadeClearOver() {
		if (isFighting) {
		} else {
			freezeGame (false);
			if (isWinning) {
				GameObject.FindGameObjectWithTag ("Virus").GetComponent<Virus> ().ConsumeCell ();
			} else {
				GameObject.FindGameObjectWithTag ("Virus").GetComponent<Virus> ().ejectVirus ();
			}
		}
	}

	public void startFight() {
		Debug.Log("StartFight");
		isFighting = true; 
		fadeBlanc = true;
		//animation -> blanc

	}

	void fadeBlancOver() {
		if (isFighting) {
			//Initiliase Fight
			freezeGame (true);//is kinetic GO
			//change state fight
			//appear fight sceen
			fadeClear = true;

			//DEBUG 
			FightOver(true);
		} else {
			//hide fight sceen
			fadeClear = true;
		}
	}

	void freezeGame(bool freeze) {
		GameObject[] cells = GameObject.FindGameObjectsWithTag("CellRenderer");
		for (int i = 0; i < cells.Length; i++) {
			Debug.Log ("freezze  "+ freeze);
			cells [i].GetComponent<Rigidbody2D> ().isKinematic = freeze;
		}

		GameObject[] indicators = GameObject.FindGameObjectsWithTag("Indicator");
		for (int i = 0; i < indicators.Length; i++) {
			Debug.Log ("freezze  "+ freeze);
			indicators [i].GetComponent<Rigidbody2D> ().isKinematic = freeze;
		}
	}

	#region Intéraction
	public static void UP(){
		try{
			GameObject.FindGameObjectWithTag ("Virus").GetComponent<Virus> ().up ();
		}catch(Exception e){}
	}

	public static void DOWN(){
		try{
			GameObject.FindGameObjectWithTag ("Virus").GetComponent<Virus> ().down ();
		}catch(Exception e){}
	}

	public static void LEFT(){
		try{
			GameObject.FindGameObjectWithTag ("Virus").GetComponent<Virus> ().left ();
		}catch(Exception e){}
	}

	public static void RIGHT(){
		try{
			GameObject.FindGameObjectWithTag ("Virus").GetComponent<Virus> ().right ();
		}catch(Exception e){}
	}

	public static void UpFight(){
		try{
			FindObjectOfType<FightManager> ().UpInput ();
		}catch(Exception e){}
	}

	public static void LeftFight(){
		try{
			FindObjectOfType<FightManager> ().LeftInput ();
		}catch(Exception e){}
	}

	public static void RightFight(){
		try{
			FindObjectOfType<FightManager> ().RightInput ();
		}catch(Exception e){}
	}

	public static void DownFight(){
		try{
			FindObjectOfType<FightManager> ().DownInput ();
		}catch(Exception e){}
	}
	#endregion Intéraction

	void OnDestroy(){
		GameStateManager.onChangeStateEvent -= handleChangeGameState;
	}
}
