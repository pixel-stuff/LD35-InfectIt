using UnityEngine;
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
	public StepFight[] m_listStep; 	
	[Space(10)]
	public GameObject[] m_listGameObjectDisplayable;

	private bool m_isInit = false;
	private int m_numberOfStep = 4;
	private List<int> m_listOfInputWaited = new List<int> ();
	private int temp = 0;

	public void InitFight(Cell cell){
		TimeManager.m_instance.ChangeState (TimeState.fight);
		m_listOfInputWaited.Clear();
		for (int i = 0; i < m_listGameObjectDisplayable.Length; i++) {
			if( i < m_numberOfStep){
				temp = UnityEngine.Random.Range (0, 4);
				m_listOfInputWaited.Add(temp);
				m_listGameObjectDisplayable [i].GetComponent<Image> ().sprite = m_listStep[temp].m_spriteController;
			}else{
				m_listGameObjectDisplayable [i].SetActive (false);
			}
		}

		for (int i = 0; i < m_listOfInputWaited.Count; i++) {
			Debug.Log ("id : " + m_listOfInputWaited[i]);
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
		Debug.Log ("BEAT FIGHT");
		m_lastBeatInvoke = Time.time;
	}

	// Use this for initialization
	void Start () {
		InitFight (null);
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}

	public void UpInput(){
		Debug.Log ("FIGHT UP");
	}

	public void DownInput(){
		Debug.Log ("FIGHT DOWN");
	}

	public void RightInput(){
		Debug.Log ("FIGHT RIGHT");
	}

	public void LeftInput(){
		Debug.Log ("FIGHT LEFT");
	}
}
