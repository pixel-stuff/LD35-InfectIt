using UnityEngine;
using System.Collections;

public class VirusFightManager : MonoBehaviour {

	private Animator m_animator;

	void Awake(){
		m_animator = this.GetComponent<Animation> ();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
