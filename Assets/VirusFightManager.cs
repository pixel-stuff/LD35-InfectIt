using UnityEngine;
using System.Collections;

public class VirusFightManager : MonoBehaviour {

	private Animation m_animation;

	void Awake(){
		m_animation = this.GetComponent<Animation> ();
	}
	// Use this for initialization
	void Start () {
		m_animation.Play ("IdleToSquare");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
