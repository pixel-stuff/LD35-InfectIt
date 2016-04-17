using UnityEngine;
using System.Collections;

public class Effect_DesaturationOverTime : Effect_Animation {
    public Effect_DesaturationOverTime() : base() {
        m_shaderTimeStartUniName = "_TimeStart";
        m_shaderTimeCurrentUniName = "_TimeCurrent";
        m_shaderTimeEndUniName = "_TimeEnd";
    }

    public override void setupChildren(float duration) {
        SpriteRenderer[] mats = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in mats) {
            if (sr.sortingOrder == 0) {
                Material ml = sr.material = m_referenceMaterial;
                if (ml != null) {
                    Debug.Log("Hello");
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
    }

    public override void updateTimeInChildren(float t) {
        SpriteRenderer[] mats = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in mats) {
            if (sr.sortingOrder == 0 && (m_shaderTimeStartUniName == null && m_shaderTimeEndUniName == null) || (m_time <= m_endTime)) {
                sr.material.SetFloat(m_shaderTimeCurrentUniName, t);
            }
        }
    }
}