using UnityEngine;
using System.Collections;

public class VirusFightManager : MonoBehaviour {

	private Animator m_animator;

	void Awake(){
		m_animator = this.GetComponent<Animator> ();
	}
	// Use this for initialization
	void Start () {
		m_animator.SetTrigger ("Triangle");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BeatAnim(){
		m_animator.SetTrigger ("Beat");
	}
}
