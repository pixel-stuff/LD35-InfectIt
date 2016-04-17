using UnityEngine;
using System.Collections;

public class Effect_DesaturationOverTime : Effect_Animation {
    public Effect_DesaturationOverTime() : base() {
        m_shaderTimeStartUniName = "_TimeStart";
        m_shaderTimeCurrentUniName = "_TimeCurrent";
        m_shaderTimeEndUniName = "_TimeEnd";
    }
}