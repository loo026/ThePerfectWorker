using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    [Header("Audio & Peak Data")]
    public AudioSource audioSource;
    public PeakData peakData;  // 在 Inspector 中拖入你事先生成并写好峰值数据的 PeakData.asset

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnPosition;

    // 如果你想保留每条鱼的顺序管理，就维持这个队列
    private Queue<FishController> fishQueue = new Queue<FishController>();

    // 用来遍历 peakData.peaks
    private int currentPeakIndex = 0;

    private void Start()
    {
        // 开始播放音频
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource not assigned!");
        }
    }

    private void Update()
    {
        // 如果没有峰值数据，就别执行
        if (peakData == null || peakData.peaks == null || peakData.peaks.Length == 0) return;

        // 如果已经到达数组末尾，也不再生成
        if (currentPeakIndex >= peakData.peaks.Length) return;

        // 当前音乐播放到的时间（秒）
        float currentTime = audioSource.time;

        // 检查是否到达下一个峰值时间
        float nextPeakTime = peakData.peaks[currentPeakIndex];
        if (currentTime >= nextPeakTime)
        {
            // 到达峰值 -> 生成鱼
            SpawnFish();
            currentPeakIndex++;
        }
    }

    private void SpawnFish()
    {
        // 从对象池获取对象
        GameObject fishObj = ObjectPool.SharedInstance.GetPooledObject();
        if (fishObj == null)
        {
            Debug.LogWarning("No pooled object available!");
            return;
        }

        // 放到指定位置，并激活
        fishObj.transform.position = spawnPosition;
        fishObj.SetActive(true);

        // 如果你还想管理鱼队列
        FishController fishController = fishObj.GetComponent<FishController>();
        fishQueue.Enqueue(fishController);
    }

    // 这个方法用来回收鱼
    public void RecycleFish(GameObject fishObj)
    {
        // 不改变对象池逻辑
        FishController fishController = fishObj.GetComponent<FishController>();
        fishObj.SetActive(false);

        // 如果队列里存在这条鱼，则出队
        if (fishQueue.Contains(fishController))
        {
            fishQueue.Dequeue();
        }

        // 重置鱼内部状态
        fishController.ResetChildTransform();
        Debug.Log("Fish is now inactive.");
    }
}
