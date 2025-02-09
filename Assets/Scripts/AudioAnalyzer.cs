using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AudioAnalyzer", menuName = "Audio/Analyzer")]
public class AudioAnalyzer : ScriptableObject
{
    public AudioClip clip;       // 要分析的音频
    public PeakData peakData;    // 存储峰值数据
    public float threshold = 0.1f;     // 触发峰值的振幅阈值
    public float minInterval = 0.1f;   // 峰值之间的最小间隔（秒）

    [ContextMenu("Analyze Audio")]  
    public void Analyze()
    {
        if (clip == null || peakData == null)
        {
            Debug.LogError("Please assign an AudioClip and a PeakData ScriptableObject!");
            return;
        }

        int totalSamples = clip.samples * clip.channels;
        float[] samples = new float[totalSamples];
        clip.GetData(samples, 0);

        float sampleRate = clip.frequency;
        List<float> foundPeaks = new List<float>();
        float lastPeakTime = -999f;

        for (int i = 0; i < totalSamples; i++)
        {
            float amplitude = Mathf.Abs(samples[i]);
            float time = i / (sampleRate * clip.channels);

            if (amplitude > threshold && (time - lastPeakTime) >= minInterval)
            {
                foundPeaks.Add(time);
                lastPeakTime = time;
            }
        }

        peakData.peaks = foundPeaks.ToArray();
        EditorUtility.SetDirty(peakData); // 标记为修改

        Debug.Log($"Analysis complete. Detected {peakData.peaks.Length} peaks.");
    }
}
