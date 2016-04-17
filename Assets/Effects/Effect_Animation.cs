using UnityEngine;
using System.Collections;

public class Effect_Animation : MonoBehaviour {

    private bool m_isEnabled;
    private Material m_material;
    private float m_time;
    private float m_endTime;

    public string m_shaderTimeStartUniName;
    public string m_shaderTimeCurrentUniName;
    public string m_shaderTimeEndUniName;

    public Material m_referenceMaterial;

    public Effect_Animation() {
        m_isEnabled = false;
    }

    public void setup(float duration) {
        m_material = GetComponent<SpriteRenderer>().material = m_referenceMaterial;
        if (m_material != null) {
            m_endTime = m_time + duration;
            if(m_shaderTimeStartUniName != null)
                m_material.SetFloat(m_shaderTimeStartUniName, m_time);
            m_material.SetFloat(m_shaderTimeCurrentUniName, m_time);
            if(m_shaderTimeEndUniName != null)
                m_material.SetFloat(m_shaderTimeEndUniName, m_endTime);
            m_isEnabled = true;
        }
    }

    public void updateTime(float t) {
        if ((m_shaderTimeStartUniName == null && m_shaderTimeEndUniName == null) || (m_material != null && m_time <= m_endTime)) {
            m_material.SetFloat(m_shaderTimeCurrentUniName, t);
        }
    }

    void Update() {
        m_time = Time.time;
        if (m_isEnabled)
            updateTime(m_time);
    }
}
