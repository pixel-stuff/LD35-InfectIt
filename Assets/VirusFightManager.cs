using UnityEngine;
using System.Collections;

public class VirusFightManager : MonoBehaviour {

	public int NbParticules;
	private Animator m_animator;

	public GameObject m_triangleParticule;
	public GameObject m_squareParticule;
	public GameObject m_donutsParticule;
	public GameObject m_crossParticule;

	void Awake(){
		m_animator = this.GetComponent<Animator> ();
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartTriangleParticles(){
		m_triangleParticule.GetComponent<ParticleSystem>().Emit(NbParticules);//.SetActive (true);
		//	Invoke ("StopParticles",0.3f);
	}//

	public void StartSquareParticles(){
		m_squareParticule.GetComponent<ParticleSystem>().Emit(NbParticules);
		//Invoke ("StopParticles",0.3f);
	}

	public void StartDonutsParticles(){
		m_donutsParticule.GetComponent<ParticleSystem>().Emit(NbParticules);
		//Invoke ("StopParticles",0.3f);
	}

	public void StartCrossParticles(){
		m_crossParticule.GetComponent<ParticleSystem>().Emit(NbParticules);
		//Invoke ("StopParticles",0.3f);
	}

	public void StopParticles(){
		m_triangleParticule.SetActive (false);
		m_squareParticule.SetActive (false);
		m_donutsParticule.SetActive (false);
		m_crossParticule.SetActive (false);
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
