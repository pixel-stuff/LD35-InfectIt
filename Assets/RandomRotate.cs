using UnityEngine;
using System.Collections;

public class RandomRotate : MonoBehaviour {

	public float maxRotate;
	public float minRotate;
	// Use this for initialization
	void Start () {
		this.transform.RotateAround(this.transform.position,Vector3.forward,Random.Range(minRotate,maxRotate)); 
	}
}
