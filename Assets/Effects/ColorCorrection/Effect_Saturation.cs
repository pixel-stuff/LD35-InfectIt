using UnityEngine;
using System.Collections;

public class Effect_Saturation : MonoBehaviour {
    private UnityStandardAssets.ImageEffects.ColorCorrection m_colorCorrection;

	// Use this for initialization
	void Start () {
        m_colorCorrection = GetComponent<UnityStandardAssets.ImageEffects.ColorCorrection>();
    }

    public void startSaturation(float duration) {
        if(m_colorCorrection!= null)
        m_colorCorrection.startSaturation(duration);
    }

    public void stopSaturation() {
        if (m_colorCorrection != null)
            m_colorCorrection.stopSaturation();
    }

    // Update is called once per frame
    void Update () {
        if (m_colorCorrection != null) {
            m_colorCorrection.update(Time.time);
        }

    }
}
