using UnityEngine;
using System.Collections;

public class Effect_Animation : MonoBehaviour {

    protected bool m_isEnabled;
    protected Material m_material;
    protected float m_time;
    protected float m_endTime;

    public string m_shaderTimeStartUniName;
    public string m_shaderTimeCurrentUniName;
    public string m_shaderTimeEndUniName;

    public Material m_referenceMaterial;

    public Effect_Animation() {
        m_isEnabled = false;
    }

    public virtual void setupChildren(float duration) {
        SpriteRenderer[] mats = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sr in mats) {
            Material ml = sr.material = m_referenceMaterial;
            if (ml != null) {
                m_endTime = m_time + duration;
                if (m_shaderTimeStartUniName != null)
                    ml.SetFloat(m_shaderTimeStartUniName, m_time);
                ml.SetFloat(m_shaderTimeCurrentUniName, m_time);
                if (m_shaderTimeEndUniName != null)
                    ml.SetFloat(m_shaderTimeEndUniName, m_endTime);
                m_isEnabled = true;
            }
        }
    }

    public void setupSelf(float duration) {
        m_material = GetComponent<SpriteRenderer>().material = m_referenceMaterial;
        if (m_material != null) {
            m_endTime = m_time + duration;
            if (m_shaderTimeStartUniName != null)
                m_material.SetFloat(m_shaderTimeStartUniName, m_time);
            m_material.SetFloat(m_shaderTimeCurrentUniName, m_time);
            if (m_shaderTimeEndUniName != null)
                m_material.SetFloat(m_shaderTimeEndUniName, m_endTime);
            m_isEnabled = true;
        }
    }

    public virtual void setup(float duration) {
        if (GetComponent<SpriteRenderer>() != null) {
            setupSelf(duration);
        } else if(GetComponentsInChildren<SpriteRenderer>() != null) {
            setupChildren(duration);
        }
    }

    public virtual void updateTimeInChildren(float t) {
        if ((m_shaderTimeStartUniName == null && m_shaderTimeEndUniName == null) || (m_material != null && m_time <= m_endTime)) {
            SpriteRenderer[] mats = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in mats) {
                sr.material.SetFloat(m_shaderTimeCurrentUniName, t);
            }
        }
    }

    public void updateTime(float t) {
        if ((m_shaderTimeStartUniName == null && m_shaderTimeEndUniName == null) || (m_material != null && m_time <= m_endTime)) {
            m_material.SetFloat(m_shaderTimeCurrentUniName, t);
        }
    }

    void Update() {
        m_time = Time.time;
        if (m_isEnabled) {
            if (GetComponent<SpriteRenderer>() != null)
                updateTime(m_time);
            else if (GetComponentsInChildren<SpriteRenderer>() != null)
                updateTimeInChildren(m_time);
        }
    }
}
