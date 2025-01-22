using UnityEngine;
using UnityEngine.InputSystem;

public class MoveToRight : MonoBehaviour
{
    private float moveSpeed = 1.0f;
    public enum FishState
    {
        Moving,
        Static
    }

    private FishState currentState = FishState.Moving;

    private void Update()
    {
        switch (currentState)
        {
            case FishState.Moving:
                MoveFish();
                break;
            case FishState.Static:
                break;
        }

    }


    void MoveFish()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

}
