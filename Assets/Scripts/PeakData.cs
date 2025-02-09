using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "PeakData", menuName = "Audio/PeakData")]
public class PeakData : ScriptableObject
{
    public AudioClip clip;
    public float threshold = 0.1f;
    public float minInterval = 0.1f;
    public float[] peaks;
    public float[] amplitudes;

    [ContextMenu("Analyze Audio")]
    public void Analyze()
    {
        if (clip == null)
        {
            Debug.LogError("Please assign an AudioClip and a PeakData ScriptableObject!");
            return;
        }

        int totalSamples = clip.samples * clip.channels;
        float[] samples = new float[totalSamples];
        clip.GetData(samples, 0);

        float sampleRate = clip.frequency;
        List<float> foundPeaks = new List<float>();
        List<float> peakAmplitudes = new List<float>();

        float lastPeakTime = -minInterval; // 确保第一个峰值不会被跳过
        float maxAmplitude = 0f;
        float maxAmplitudeTime = -1f;
        bool peakFound = false;

        for (int i = 0; i < totalSamples; i += clip.channels) // 只取左声道
        {
            float amplitude = Mathf.Abs(samples[i]); // 直接取左声道数据
            float time = (i / clip.channels) / sampleRate; // 计算时间点

            //  只存 `minInterval` 内的最大值
            if (amplitude > threshold)
            {
                if (!peakFound || amplitude > maxAmplitude)
                {
                    maxAmplitude = amplitude;
                    maxAmplitudeTime = time;
                    peakFound = true;
                }
            }

            // 当 `minInterval` 结束时，存储峰值
            if (time - lastPeakTime >= minInterval && peakFound)
            {
                foundPeaks.Add(maxAmplitudeTime);
                peakAmplitudes.Add(maxAmplitude);
                lastPeakTime = maxAmplitudeTime;

                // 重新开始寻找新的 `minInterval` 内最大峰值
                maxAmplitude = 0f;
                peakFound = false;
            }
        }

        peaks = foundPeaks.ToArray();
        amplitudes = peakAmplitudes.ToArray();
        EditorUtility.SetDirty(this);

        Debug.Log($"✅ Peaks Detected: {peaks.Length}, Amplitudes Recorded: {amplitudes.Length}");
    }

}