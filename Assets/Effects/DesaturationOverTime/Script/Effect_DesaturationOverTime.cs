using UnityEngine;
using System.Collections;

public class Effect_DesaturationOverTime : MonoBehaviour {

    private bool m_isEnabled;
    private Material m_material;
    private float m_time;
    private float m_endTime;

    public Material m_desaturationOverTime;

    public Effect_DesaturationOverTime() {
        m_isEnabled = false;
    }

    public void setup(float duration) {
        m_material = GetComponent<SpriteRenderer>().material = m_desaturationOverTime;
        if (m_material != null) {
            m_endTime = m_time + duration;
            m_material.SetFloat("_TimeStart", m_time);
            m_material.SetFloat("_TimeCurrent", m_time);
            m_material.SetFloat("_TimeEnd", m_endTime);
            m_isEnabled = true;
        }
    }

    public void updateTime(float t) {
        if (m_material != null && m_time<=m_endTime) {
            m_material.SetFloat("_TimeCurrent", t);
        }
    }

    void Update() {
        m_time = Time.time;
        if(m_isEnabled)
            updateTime(m_time);
    }
}
