using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct StepFight{
	public string m_name;
	public Sprite m_sprite;
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


	public void InitFight(Cell cell){
		

		m_isInit = true;
	}


	// Use this for initialization
	void Start () {
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
