using UnityEngine;
using System.Collections;

public class Effect_ScreenRedEdge : Effect_Animation {
    public Effect_ScreenRedEdge():base() {
        m_shaderTimeStartUniName = null;
        m_shaderTimeCurrentUniName = "_TimeScript";
        m_shaderTimeEndUniName = null;
    }
}
