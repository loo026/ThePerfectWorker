using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition;
    public float spawnInterval = 2f;
    private Queue<FishController> fishQueue = new Queue<FishController>(); // arrange fish order
    private FishController currentCutFish = null; // fish wait to cut


    private void OnEnable()
    {
        FishController.OnFishArriveAtCutPosition += HandleFishArriveAtCutPos;
        FishController.OnFishCut += HandledFishWasCut;
    }

    private void OnDisable()
    {
        FishController.OnFishArriveAtCutPosition -= HandleFishArriveAtCutPos;
        FishController.OnFishCut -= HandledFishWasCut;
    }


    void Start()
    {
        InvokeRepeating("SpawnFish", 1f, spawnInterval);
    }


    void SpawnFish()
    {
        GameObject fishObj = ObjectPool.SharedInstance.GetPooledObject();
        if (fishObj != null)
        {
            fishObj.transform.position = spawnPosition;

            fishObj.SetActive(true);
            FishController fish = fishObj.GetComponent<FishController>();
            fishQueue.Enqueue(fish); 

            // if there is no fish wait to cut, set 1st fish as it
            if (currentCutFish == null)
            {
                currentCutFish = fish;
            }
        }
    }

    public void RecycleFish(GameObject fishObj)
    {
        //fish.SetActive(false);
        FishController fishController = fishObj.GetComponent<FishController>();
        fishObj.SetActive(false);

        if (fishQueue.Contains(fishController))
        {
            fishQueue.Dequeue();
        }

        fishController.ResetChildTransform();


        Debug.Log("Fish is now inactive.");
    }

    public bool IsCurrentCutFish(FishController fish)
    {
        //check if it is the first order
        return currentCutFish == fish;
    }

    private void HandleFishArriveAtCutPos(FishController fish)
    {
        foreach (var f in fishQueue)
        {
            f.SetStatic();
        }
    }


    private void HandledFishWasCut(FishController fish)
    {
        if (fish == currentCutFish)
        {
            fishQueue.Dequeue();
            currentCutFish = fishQueue.Count > 0 ? fishQueue.Peek() : null;


            foreach (var f in fishQueue)
            {
                f.SetMoving();
            }
        }

    }
}
