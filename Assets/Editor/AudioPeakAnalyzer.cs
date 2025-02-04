using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AudioAnalyzerEditor : EditorWindow
{
    private AudioClip clip;
    private PeakData peakData; // ScriptableObject
    private float threshold = 0.1f;
    private float minInterval = 0.1f;

    [MenuItem("Tools/Audio Analyzer")]
    static void ShowWindow()
    {
        GetWindow<AudioAnalyzerEditor>("Audio Analyzer");
    }

    private void OnGUI()
    {
        clip = EditorGUILayout.ObjectField("Audio Clip:", clip, typeof(AudioClip), false) as AudioClip;
        peakData = EditorGUILayout.ObjectField("Peak Data SO:", peakData, typeof(PeakData), false) as PeakData;

        threshold = EditorGUILayout.FloatField("Threshold:", threshold);
        minInterval = EditorGUILayout.FloatField("Min Interval:", minInterval);

        if (GUILayout.Button("Analyze"))
        {
            Analyze();
        }
    }

    private void Analyze()
    {
        if (clip == null || peakData == null)
        {
            Debug.LogError("Please assign an AudioClip and a PeakData ScriptableObject!");
            return;
        }

        // 获取波形数据
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

        // 把结果写到 PeakData 的数组里
        peakData.peaks = foundPeaks.ToArray();
        EditorUtility.SetDirty(peakData);
        // 标记该资源已修改，Unity 会在你保存项目时自动更新 .asset 文件

        Debug.Log($"Analysis complete. Detected {peakData.peaks.Length} peaks. Saved to {peakData.name}.");
    }
}
