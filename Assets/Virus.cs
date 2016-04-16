using UnityEngine;
using System.Collections;

public class Virus : MonoBehaviour {


	public float moveForce;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
		if (other.gameObject.layer == LayerMask.NameToLayer("Cell")) {
			Debug.Log ("Start Collide Cell");
			this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
		}
	}
}
