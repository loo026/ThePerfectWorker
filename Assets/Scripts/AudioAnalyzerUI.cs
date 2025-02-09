using UnityEngine;
using UnityEngine.UI;

public class AudioAnalyzerUI : MonoBehaviour
{
    public PeakData peakData;
    public RectTransform graphContainer;
    public GameObject pointPrefab; // 预制体，代表峰值点

    public float graphWidth = 400f; // UI 宽度
    public float graphHeight = 200f; // UI 高度

    [ContextMenu("ShowPeakDataGraph")]
    void ShowUI()
    {
        if (peakData == null || peakData.peaks == null || peakData.peaks.Length == 0)
            return;

        ClearGraph(); // ✅ 先清除已有的点，防止重复

        // 计算振幅的最大值，以便归一化到 UI 高度
        float maxAmplitude = 0f;
        foreach (float amp in peakData.amplitudes)
            if (amp > maxAmplitude) maxAmplitude = amp;

        // 遍历峰值数据，生成 UI 点
        for (int i = 0; i < peakData.peaks.Length; i++)
        {
            float time = peakData.peaks[i];
            float amplitude = peakData.amplitudes[i];

            float x = (time / peakData.peaks[peakData.peaks.Length - 1]) * graphWidth;
            float y = (amplitude / maxAmplitude) * graphHeight; // 归一化振幅到 UI 高度

            GameObject point = Instantiate(pointPrefab, graphContainer);
            point.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        }
    }

    // 清除所有子对象
    [ContextMenu("ClearGraph")]
    void ClearGraph()
    {
        for (int i = graphContainer.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(graphContainer.GetChild(i).gameObject); // 立即删除
        }
    }

}
