using UnityEngine;

public class BeatColorController : MonoBehaviour
{
    public Renderer targetRenderer;
    private Material mat;

    public float normalIntensity = 0f;  
    public float beatIntensity = 1f;    
    public float fadeSpeed = 2f;        

    private float currentIntensity;

    private void OnEnable()
    {
        BeatDetector.OnBeat += OnBeatDetected;
    }

    private void OnDisable()
    {
        BeatDetector.OnBeat -= OnBeatDetected;
    }

    private void Start()
    {
        mat = targetRenderer.material;
        // 起始时设置为正常亮度
        currentIntensity = normalIntensity;
        mat.SetFloat("_GlowIntensity", currentIntensity);
    }

    private void Update()
    {
        // 如果当前亮度高于 normalIntensity，就让它平滑衰减回 normalIntensity
        if (currentIntensity > normalIntensity)
        {
            currentIntensity = Mathf.Lerp(currentIntensity, normalIntensity, Time.deltaTime * fadeSpeed);
            mat.SetFloat("_GlowIntensity", currentIntensity);
        }
    }

    private void OnBeatDetected(float intensity)
    {
        // 当检测到节拍时，瞬间把当前亮度加到一个较高值
        // 比如 normalIntensity + intensity * beatIntensity
        float newValue = normalIntensity + intensity * beatIntensity;

        // 更新 currentIntensity 并传给材质
        currentIntensity = newValue;
        mat.SetFloat("_GlowIntensity", currentIntensity);
    }
}
