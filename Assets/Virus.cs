using UnityEngine;
using System.Collections;

public class Virus : MonoBehaviour {

	public AnimationCurve goOnCenterAnimation;
	public float moveForce;
	public int nbFrameAnimation;

	public bool EJECT;

	private bool m_isOnCenterAnimation;
	private Vector2 m_centerCell; 
	private float m_animationTime;
	private Vector3 toto;

	private bool m_isEjected;

	private GameObject m_targetCell;
	// Use this for initialization
	void Start () {
		m_isEjected = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_isOnCenterAnimation) {
			Vector3 dif = -toto + new Vector3(m_centerCell.x,m_centerCell.y,this.transform.position.z);
			this.transform.position = toto + dif*(goOnCenterAnimation.Evaluate(m_animationTime/nbFrameAnimation));
			m_animationTime++;
			if (m_animationTime > nbFrameAnimation) {
				m_isOnCenterAnimation = false;
				PlayerManager.m_instance.startFight ();
			}
		}


		if (EJECT) {
			ejectVirus ();
			EJECT = false;
		}
	
	}

	public void up () {
		this.gameObject.GetComponent<Rigidbody2D> ().AddForce ( new Vector2(0, moveForce));
	}


	public void down() {
		this.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2(0, -moveForce));
	}


	public void left() {
		this.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2(-moveForce ,0));
	}


	public void right() {
		this.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2(moveForce, 0));
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Start Collide " + other.gameObject.layer);
		//this.GetComponent<SpriteRenderer> ().material.SetFloat ("_BorderSpeed", 15);
		if (other.gameObject.layer == LayerMask.NameToLayer("Cell")) {
			if (!m_isEjected && other.gameObject.GetComponent<virusHack> ().acceptFusion()) {
				Debug.Log ("Start Collide Cell");
				this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
				other.gameObject.GetComponent<virusHack> ().startFusion ();
				m_isOnCenterAnimation = true;
				m_centerCell = other.gameObject.transform.position;
				m_animationTime = 0;
				toto = this.transform.position;
				m_targetCell = other.gameObject;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer ("Cell")) {
			m_isEjected = false;
			this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = false;
			other.gameObject.GetComponent<virusHack> ().stopFusion ();
		}
	}

	public void ejectVirus() {
		m_isEjected = true;
		m_isOnCenterAnimation = false;
		this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = false;
		Vector2 direction = Random.insideUnitCircle;
		this.GetComponent<Rigidbody2D> ().AddRelativeForce (moveForce * direction*200);
	}

	public void ConsumeCell() {
		m_isOnCenterAnimation = false;
		this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = false;
		m_targetCell.GetComponent<virusHack> ().consume ();

	}
}
