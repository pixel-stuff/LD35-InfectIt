using UnityEngine;
using System.Collections;

public class randomMove : MonoBehaviour {

	public float speedThreshold;
	public float accelerationSpeedThreshold;
	public int minForce;
	public int maxForce;

	private float m_speed;
	private Vector3 m_OldPosition;
	private bool m_accelerationPhase;
	// Use this for initialization
	void Start () {
		m_speed = 0;
		m_OldPosition = this.transform.position;
		m_accelerationPhase = false;

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
				moveRandomDirection ();
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
}
