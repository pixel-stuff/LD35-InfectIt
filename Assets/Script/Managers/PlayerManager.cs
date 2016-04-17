﻿using UnityEngine;
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



	void Start () {
		GameStateManager.onChangeStateEvent += handleChangeGameState;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void handleChangeGameState(GameState newState){
		Debug.Log ("PLAYER SEE THE NEW STATE : " + newState);
	}

	#region Intéraction
	public static void UP(){
		Debug.Log("UP ! ");
		try{
			GameObject.FindGameObjectWithTag ("Virus").GetComponent<Virus> ().up ();
		}catch(Exception e){}
	}

	public static void DOWN(){
		Debug.Log("DOWN ! ");
		try{
			GameObject.FindGameObjectWithTag ("Virus").GetComponent<Virus> ().down ();
		}catch(Exception e){}
	}

	public static void LEFT(){
		Debug.Log("LEFT ! ");
		try{
			GameObject.FindGameObjectWithTag ("Virus").GetComponent<Virus> ().left ();
		}catch(Exception e){}
	}

	public static void RIGHT(){
		Debug.Log("RIGHT ! ");
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
