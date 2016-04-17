﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

[Serializable]
public struct StepFight{
	public int m_id;	//0 -> triangle/up
						//1 -> carre/left
						//2 -> croix/down
						//3 -> rond/right
	public string m_nameController;
	public Sprite m_spriteController;
	public string m_nameKeyBoard;
	public Sprite m_spriteKeyBoard;
	public AudioClip m_sonReussi;
	public AudioClip m_sonFailed;
}


public class FightManager : MonoBehaviour {
	#region Singleton
	private static FightManager m_instance;
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
	[Space(10)]
	public StepFight[] m_listOfInput; 	
	[Space(10)]
	public GameObject[] m_listGameObjectDisplayable;

	private bool m_isInit = false;
	private int m_numberOfStep = 4;
	private List<int> m_listOfIDInputWaited = new List<int> ();
	private float m_timeBetweenBeat = 0.60f;
	private float m_percentErrorAceptable = 40.0f;

	private int m_currentStepInputWaited = 0;



	public void InitFight(Cell cell){
		TimeManager.m_instance.ChangeState (TimeState.fight);
		m_listOfIDInputWaited.Clear();
		m_currentStepInputWaited = 0;
		for (int i = 0; i < m_listGameObjectDisplayable.Length; i++) {
			if( i < m_numberOfStep){
				int temp = UnityEngine.Random.Range (0, 4);
				m_listOfIDInputWaited.Add(temp);
				m_listGameObjectDisplayable [i].GetComponent<Image> ().sprite = m_listOfInput[temp].m_spriteController;
			}else{
				m_listGameObjectDisplayable [i].SetActive (false);
			}
		}
		for (int i = 0; i < m_listOfIDInputWaited.Count; i++) {
			Debug.Log ("init : " + m_listOfInput[m_listOfIDInputWaited[i]].m_nameKeyBoard);
		}

		FindObjectOfType<AudioManager> ().m_beatEvent += BeatInvokeHandle;
		FindObjectOfType<AudioManager> ().PlayFightMusic ();
		m_lastBeatInvoke = Time.time;

		m_isInit = true;
	}


	private int m_numberOfBeatDoneInvoke = 0;
	private float m_lastBeatInvoke = 0;
	public void BeatInvokeHandle(){
		m_numberOfBeatDoneInvoke++;
		m_lastBeatInvoke = Time.time;
		for (int i = 0; i < m_listGameObjectDisplayable.Length; i++) {
			if( i < m_numberOfStep){
				if (!m_listGameObjectDisplayable [i].GetComponent<Animation> ().isPlaying) {
					m_listGameObjectDisplayable [i].GetComponent<Animation> ().Play ("ScaleInputBeatFight");
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		m_timeBetweenBeat = FindObjectOfType<AudioManager> ().m_timeBetweenBeat;
		InitFight (null);
		GameStateManager.setGameState (GameState.Playing);
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log ("Up : " + IsTimingOK());
		Debug.Log("step : " + m_currentStepInputWaited + ", ID waited " + m_listOfIDInputWaited[m_currentStepInputWaited] + ", name = " + m_listOfInput[m_listOfIDInputWaited[m_currentStepInputWaited]].m_nameKeyBoard);
	}

	#region Input
	public void UpInput(){
		if (m_listOfIDInputWaited [m_currentStepInputWaited] != 0) {
			ErrorInput ();
			return;
		}
		if (!IsTimingOK ()) {
			ErrorInput ();
			return;
		}
		SuccessInput ();
	}

	public void DownInput(){
		if (m_listOfIDInputWaited [m_currentStepInputWaited] != 2) {
			ErrorInput ();
			return;
		}
		if (!IsTimingOK ()) {
			ErrorInput ();
			return;
		}
		SuccessInput ();
	}

	public void RightInput(){
		if (m_listOfIDInputWaited [m_currentStepInputWaited] != 3) {
			ErrorInput ();
			return;
		}
		if (!IsTimingOK ()) {
			ErrorInput ();
			return;
		}
		SuccessInput ();
	}

	public void LeftInput(){
		if (m_listOfIDInputWaited [m_currentStepInputWaited] != 1) {
			ErrorInput ();
			return;
		}
		if (!IsTimingOK ()) {
			ErrorInput ();
			return;
		}
		SuccessInput ();
	}

	public bool IsTimingOK(){
		bool isOK = false;
		if (Mathf.Abs (Time.time - m_lastBeatInvoke) <= m_timeBetweenBeat * m_percentErrorAceptable / 100f) {
			isOK = true;
		}
		if (Mathf.Abs (Time.time + m_timeBetweenBeat - m_lastBeatInvoke) <= m_timeBetweenBeat * m_percentErrorAceptable / 100f) {
			isOK = true;
		}
		return isOK;
	}

	public void ErrorInput(){
		this.GetComponent<AudioSource> ().clip = m_listOfInput [m_listOfIDInputWaited[m_currentStepInputWaited]].m_sonFailed;
		this.GetComponent<AudioSource> ().Play ();
		m_currentStepInputWaited = 0;
	}

	public void SuccessInput(){
		this.GetComponent<AudioSource> ().clip = m_listOfInput [m_listOfIDInputWaited[m_currentStepInputWaited]].m_sonReussi;
		this.GetComponent<AudioSource> ().Play ();
		m_listGameObjectDisplayable [m_currentStepInputWaited].GetComponent<Animation> ().Play ("ScaleInputEnterFight");
		m_currentStepInputWaited++;
	}
	#endregion Input

}
