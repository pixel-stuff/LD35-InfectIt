using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Effect_DesaturationOverTime>().setup(3.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
