using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CompteurManager : MonoBehaviour {
	private int m_numberAtStart = 3;

	public void SetCurrentCellDestroy(int number){
		this.GetComponent<Text> ().text = "" + number.ToString () + " / " + m_numberAtStart.ToString ();
	}

	public void SetNumberAtStart(){
		this.GetComponent<Text> ().text = "0 / " + m_numberAtStart.ToString();
	}
}
