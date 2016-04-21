using UnityEngine;
using System.Collections;

public class whiteCell : MonoBehaviour {
	public GameObject m_target;
	public aggroZone m_aggroZone;

	public float speedThreshold;
	public float accelerationSpeedThreshold;
	public int m_minForce;
	public int m_maxForce;

	private float m_speed;
	private Vector3 m_OldPosition;
	private bool m_accelerationPhase;



	private Vector2 m_oldDirection;// = Random.insideUnitCircle;

	void Awake() {
		m_target = null;
		m_oldDirection = Random.insideUnitCircle;
	}
	// Use this for initialization
	void Start () {
		m_speed = 0;
		m_OldPosition = this.transform.position;
		m_accelerationPhase = false;
	
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
			if (m_target) {
				moveOnTarget ();
			} else {
				randomMove ();
			}
		}
		if (m_speed > accelerationSpeedThreshold || m_speed == 0) {
			m_accelerationPhase = false;
		}
			

	
	}

	void moveOnTarget() {
		Vector3 directionV3 = m_target.transform.position - this.transform.position;
		Vector2 direction = new Vector2 (directionV3.x, directionV3.y);
		direction.Normalize ();
		int force = Random.Range (m_minForce,m_maxForce);
		//Vector3 forceVector = new Vector3(direction.x*force,direction.y*force,0);

		this.GetComponent<Rigidbody2D> ().AddRelativeForce (force * direction);
		m_accelerationPhase = true;

	}

	void randomMove() {

		Vector2 direction = Random.insideUnitCircle;
		/*if(direction)
		 * TODO vecteur dans les 180° autour
*/
		int force = Random.Range (m_minForce,m_maxForce);
		//Vector3 forceVector = new Vector3(direction.x*force,direction.y*force,0);

		this.GetComponent<Rigidbody2D> ().AddRelativeForce (force * direction);
		m_accelerationPhase = true;
	}

	public void alerte () {
		m_aggroZone.setMaxRadius ();
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Virus")) {
			GameStateManager.m_instance.setGameState (GameState.GameOver);
		}
	}
}
