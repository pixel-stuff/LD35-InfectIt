using UnityEngine;
using System.Collections;

public class followGameObject : MonoBehaviour {

	public GameObject followGO;

	public float force;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (followGO.activeSelf) {

			Vector2 moveVector = new Vector2 (followGO.transform.position.x, followGO.transform.position.y) - new Vector2 (this.transform.position.x, this.transform.position.y);
			moveVector.Normalize ();
			this.gameObject.GetComponent<Rigidbody2D> ().AddRelativeForce (moveVector * force);
			//this.transform.Rotate( Vector3.forward * Mathf.Acos (moveVector.x));
		} else {
			this.gameObject.SetActive (false);
		}
	}
}
