using UnityEngine;

public class BeatDetector : MonoBehaviour
{
    public AudioSource audioSource;
    public int sampleSize = 128;
    public float threshold = 0.2f;
    public float minBeatInterval = 0.3f;

    public delegate void BeatEvent(float beatIntensity);
    public static event BeatEvent OnBeat;

    private float lastBeatTime = 0f;
    private float[] spectrumData;

    private void Start()
    {
        spectrumData = new float[sampleSize];
    }

    private void Update()
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        float sum = 0f;
        for (int i = 0; i < sampleSize; i++)
        {
            sum += spectrumData[i];
        }

        // 当能量超过阈值且间隔足够时触发 beat
        if (sum > threshold && Time.time - lastBeatTime > minBeatInterval)
        {
            lastBeatTime = Time.time;
            // 计算一个归一化的 beat 强度，例如将 (sum - threshold) 映射到 0~1
            float beatIntensity = Mathf.Clamp01((sum - threshold) / (threshold * 2f));
            OnBeat?.Invoke(beatIntensity);
            Debug.Log("Beat detected at time: " + Time.time + ", intensity: " + beatIntensity);
        }
    }
}
