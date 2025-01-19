using UnityEngine;

public class MoveToRight : MonoBehaviour
{
    private float moveSpeed = 1.0f;


    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
