using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerColorFeedback : MonoBehaviour
{
    [Header("Shader / Duración")]
    public Shader multicolorShader;        // asigna "Sprites/MulticolorHue" en el inspector
    public float duration = 0.15f;         // duración del feedback (0.15s por defecto)

    [Header("Animación")]
    public float cycles = 1f;              // cuántas veces recorre todo el hue durante la duración
    public AnimationCurve intensityCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    // Internals
    private SpriteRenderer[] spriteRenderers;
    private Material[] originalMaterials;
    private Material[] effectMaterials;
    private bool isPlaying = false;

    void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        // guardar originales
        originalMaterials = new Material[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            // Copiamos el material para no alterar sharedMaterial por accidente
            originalMaterials[i] = spriteRenderers[i].material;
        }
    }

    public void PlayFeedback(float customDuration = -1f)
    {
        if (isPlaying) return;
        StartCoroutine(PlayRoutine(customDuration > 0 ? customDuration : duration));
    }

    private IEnumerator PlayRoutine(float dur)
    {
        if (multicolorShader == null)
        {
            Debug.LogWarning("Multicolor shader not assigned in PlayerColorFeedback.");
            yield break;
        }

        isPlaying = true;

        // Crear materiales temporales (instanciados) con el shader y la misma textura
        effectMaterials = new Material[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            Material mat = new Material(multicolorShader);
            // Intenta copiar la textura del material original
            if (originalMaterials[i] != null && originalMaterials[i].HasProperty("_MainTex"))
            {
                Texture tex = originalMaterials[i].GetTexture("_MainTex");
                if (tex != null) mat.SetTexture("_MainTex", tex);
            }
            mat.SetFloat("_HueOffset", 0f);
            mat.SetFloat("_Intensity", 1f);
            effectMaterials[i] = mat;
            spriteRenderers[i].material = mat;
        }

        float t = 0f;
        while (t < dur)
        {
            float normalized = t / dur;
            // Hue progression: cycles times around the hue circle
            float hue = Mathf.Repeat(normalized * cycles, 1f); // 0..1
            float intensity = intensityCurve.Evaluate(normalized); // 1 -> 0 by default curve

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                Material m = effectMaterials[i];
                if (m != null)
                {
                    m.SetFloat("_HueOffset", hue);
                    m.SetFloat("_Intensity", intensity);
                }
            }

            t += Time.deltaTime;
            yield return null;
        }

        // restore original materials
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
            {
                spriteRenderers[i].material = originalMaterials[i];
            }
            if (effectMaterials[i] != null)
            {
                Destroy(effectMaterials[i]);
            }
        }

        isPlaying = false;
    }
}
