using UnityEngine;
using UnityEngine.InputSystem;

public class FishManager : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private Vector3 recyclePosition;
    public float spawnInterval = 2f;
    private bool isAllStatic = false;

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
        InvokeRepeating("SpawnFish",1f,2f);
    }


    void SpawnFish()
    {
        GameObject fish = ObjectPool.SharedInstance.GetPooledObject();
        if (fish != null)
        {
            fish.transform.position = spawnPosition;

            fish.SetActive(true);

        }
    }

   public void RecycleFish(GameObject fish)
    {
        fish.SetActive(false);
        FishController fishController = fish.GetComponent<FishController>();
        if (fishController != null)
        {
            fishController.ResetChildTransform();
        }
        Debug.Log("Fish is now inactive.");
    }


    private void HandleFishArriveAtCutPos(FishController fish)
    {
        if (isAllStatic) return;

        isAllStatic = true;
        FishController[] allFish = FindObjectsByType<FishController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var f in allFish)
        {
            f.SetStatic();
        }
    }


    private void HandledFishWasCut(FishController fish)
    {
        if (!isAllStatic) return;

        isAllStatic = false;
        FishController[] allFish = FindObjectsByType<FishController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var f in allFish)
        {
            f.SetMoving();
        }
    }

/*    public void NotifyFishAtCutPostion(FishController fish)
    {
        if (isAllStatic) return;

        isAllStatic = true;
        FishController[] allFish = FindObjectsByType<FishController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var f in allFish)
        {
            f.SetStatic();
        }
    }

    public void NotifyFishWasCut()
    {
        if(!isAllStatic) return;

        isAllStatic = false;
        FishController[] allFish = FindObjectsByType<FishController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var f in allFish)
        {
            f.SetMoving();
        }
    }*/
}
