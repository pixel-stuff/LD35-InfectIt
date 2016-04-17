using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {

	public float speedThreshold;
	public float accelerationSpeedThreshold;
	public int minForce;
	public int maxForce;
	public int nbMoveBeforeStopRun;


	private bool m_run;
	public bool m_isAfraid;
	private int m_nbTimeRun;
	private Vector2 m_endFusionPosition;

	private float m_speed;
	private Vector3 m_OldPosition;
	private bool m_accelerationPhase;
	// Use this for initialization
	void Start () {
		m_speed = 0;
		m_OldPosition = this.transform.position;
		m_accelerationPhase = false;
		m_run = false;

		//moveRandomDirection ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 speedVector = this.transform.position - m_OldPosition;
		m_speed = Mathf.Abs (speedVector.x) + Mathf.Abs (speedVector.y);
		m_OldPosition = this.transform.position;

		/*if (!m_accelerationPhase) {
			moveRandomDirection ();
		}*/
		if (m_speed < speedThreshold && !m_accelerationPhase) {
			if (m_run) {
				if (m_nbTimeRun < nbMoveBeforeStopRun) {
					run ();
					m_nbTimeRun++;
				} else {
					m_run = false;
					m_nbTimeRun = 0;
				}
			} else {
				moveRandomDirection ();
			}
		}
		if (m_speed > accelerationSpeedThreshold || m_speed == 0) {
			m_accelerationPhase = false;
		}
	
	}

	void moveRandomDirection() {
		Vector2 direction = Random.insideUnitCircle;
		int force = Random.Range (minForce,maxForce);
		//Vector3 forceVector = new Vector3(direction.x*force,direction.y*force,0);

		this.GetComponent<Rigidbody2D> ().AddRelativeForce (force * direction);
		m_accelerationPhase = true;
	}

	void run() {
		Vector2 direction = -(m_endFusionPosition - new Vector2(this.transform.position.x, this.transform.position.y));
		direction.Normalize();
		int force = Random.Range (minForce,maxForce);
		this.GetComponent<Rigidbody2D> ().AddRelativeForce (force * direction);
		m_accelerationPhase = true;
	}

	public void startFusion(){
		Debug.Log ("StartFusion");
		if (m_isAfraid) {
			m_run = true;
			m_endFusionPosition = this.transform.position;
			//TODO animation afarid here

			//this.gameObject.SetActive(false);
		} else {
			this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
			this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		}
	}

	public void stopFusion() {
		m_endFusionPosition = this.transform.position;
		m_isAfraid = true;
		m_run = true;
		this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = false;
		this.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
	}

	public void consume() {
		Debug.Log("CellNOMNOMNOM");
		this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
		this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		PlayerManager.m_instance.addDestroyCell ();
		this.gameObject.SetActive (false);
		//this.GetComponent<Animation> ().Play ("DeathANimation");
	}
}
