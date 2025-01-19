using UnityEngine;
using UnityEngine.InputSystem;

public class FishManager : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private Vector3 recyclePosition;
    public float spawnInterval = 2f;


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
        Debug.Log("Fish is now inactive.");
    }

}
