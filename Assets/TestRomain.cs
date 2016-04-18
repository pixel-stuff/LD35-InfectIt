using UnityEngine;
using System.Collections;

public class TestRomain : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FindObjectOfType<FightManager>().InitFight (null);
		GameStateManager.m_instance.setGameState (GameState.Playing);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
