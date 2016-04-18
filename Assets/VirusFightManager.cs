using UnityEngine;
using System.Collections;

public class VirusFightManager : MonoBehaviour {

	private Animator m_animator;

	void Awake(){
		m_animator = this.GetComponent<Animator> ();
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TriangleAnim(){
		m_animator.SetTrigger ("Triangle");
	}
	public void SquareAnim(){
		m_animator.SetTrigger ("Square");
	}
	public void DonutsAnim(){
		m_animator.SetTrigger ("Donuts");
	}
	public void CrossAnim(){
		m_animator.SetTrigger ("Cross");
	}
	public void BeatAnim(){
		this.GetComponent<Animation> ().Play ();
	}
}
