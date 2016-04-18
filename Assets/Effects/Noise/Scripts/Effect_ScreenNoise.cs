using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Custom/Noise/Noise")]
public class Effect_ScreenNoise : UnityStandardAssets.ImageEffects.PostEffectsBase {

    [Range(0.0f, 40.0f)]
    public float speed = 1.0f;

    [Range(0.0f, 100.0f)]
    public float freq = 1.0f;

    [Range(0.0f, 10.0f)]
    public float amp = 0.5f;
    [Range(0.0f, 1.0f)]
    public float noiseType = 0.5f;

    public Shader noiseShader = null;
    private Material noiseMaterial = null;
    public Texture noiseTex = null;


    public override bool CheckResources() {
        CheckSupport(false);

        noiseMaterial = CheckShaderAndCreateMaterial(noiseShader, noiseMaterial);
        if (noiseMaterial != null) {
            noiseMaterial.SetTexture("_NoiseTex", noiseTex);
        }

        if (!isSupported)
            ReportAutoDisable();
        return isSupported;
    }

    public void OnDisable() {
        if (noiseMaterial)
            DestroyImmediate(noiseMaterial);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (CheckResources() == false) {
            Graphics.Blit(source, destination);
            return;
        }

        noiseMaterial.SetVector("_Parameter", new Vector4(speed, freq, amp, noiseType));
        Graphics.Blit(source, destination, noiseMaterial);
    }
}
