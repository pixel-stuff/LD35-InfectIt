using UnityEngine;
using System.Collections;

public class followGameObject : MonoBehaviour {

	public GameObject followGO;

	public GameObject container;

	public float force;
	public float OldRotation = 0;


	// Use this for initialization
	void Start () {
		OldRotation = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (followGO.gameObject.GetComponent<Cell> ().isTrueCell && !followGO.gameObject.GetComponent<Cell> ().m_isAfraid) {
			if (followGO.gameObject.GetComponent<Cell> ().isOnCamera ()) {
				container.SetActive (false);
			} else {
				container.SetActive (true);
			}
				Vector2 moveVector = new Vector2 (followGO.transform.position.x, followGO.transform.position.y) - new Vector2 (this.transform.position.x, this.transform.position.y);
				moveVector.Normalize ();

				float angle = Mathf.Atan2 (moveVector.y, moveVector.x);
				angle = (-180.0f / Mathf.PI * angle) % 360.0f;
				this.gameObject.GetComponent<Rigidbody2D> ().AddRelativeForce (moveVector * force);
				container.transform.Rotate (Vector3.forward * (OldRotation - angle));
				OldRotation = angle;
		} else {
			this.gameObject.SetActive (false);
		}
	}
}
