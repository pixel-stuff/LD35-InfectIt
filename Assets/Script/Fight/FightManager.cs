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

	public StepFight[] m_listStep;
	public GameObject[] m_listGameObjectDisplayable;

	public void InitFight(GameObject cellule){

	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
