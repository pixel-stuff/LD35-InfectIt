using UnityEngine;
using System.Collections;

public class Virus : MonoBehaviour {

	public AnimationCurve goOnCenterAnimation;
	public float moveForce;
	public int nbFrameAnimation;


	private bool m_isOnCenterAnimation;
	private Vector2 m_centerCell; 
	private float m_animationTime;
	private Vector3 toto;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (m_isOnCenterAnimation) {
			Vector3 dif = -toto + new Vector3(m_centerCell.x,m_centerCell.y,this.transform.position.z);
			this.transform.position = toto + dif*(goOnCenterAnimation.Evaluate(m_animationTime/nbFrameAnimation));
			m_animationTime++;
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
		if (other.gameObject.layer == LayerMask.NameToLayer("Cell")) {
			Debug.Log ("Start Collide Cell");
			this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
			m_isOnCenterAnimation = true;
			m_centerCell = other.gameObject.transform.position;
			m_animationTime = 0;
			toto = this.transform.position;
		}
	}
}
