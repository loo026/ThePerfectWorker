using UnityEngine;
using UnityEngine.Events;

public class FishController : MonoBehaviour
{
    public delegate void CutEventHandler(FishController fish);
    public static event CutEventHandler OnFishCut;

    [SerializeField] private Rigidbody fishHeadRb;
    [SerializeField] private Rigidbody fishBodyRb;


    private bool isCut = false;


    public void Start()
    {
        fishHeadRb.isKinematic = true;
        fishBodyRb.isKinematic = true;
    }


    public void Cut()
    {

        if (isCut)
        {
            return;
        }
        else
        {
            isCut = true;
            fishHeadRb.isKinematic = false;
            fishBodyRb.isKinematic = false;
            ApplyForce();
            Debug.Log("Fish is cut!");
        }
    }

    private void ApplyForce()
    {
        
        Vector3 headForceDirection = new Vector3(Random.Range(0.5f,1f),Random.Range(0.5f,1f),0f);
        Vector3 bodyForceDirection = new Vector3(Random.Range(-0.5f, -1f), Random.Range(0.5f, 1f), 0f);
        float speed = Random.Range(1f, 1.5f);

        if (fishHeadRb != null) {
        
            fishHeadRb.AddForce(headForceDirection * speed, ForceMode.Impulse);           
            fishHeadRb.AddTorque(Vector3.up * 0.1f, ForceMode.Impulse);

        }

        if (fishBodyRb != null) {

            fishBodyRb.AddForce(bodyForceDirection * speed, ForceMode.Impulse);
            fishBodyRb.AddTorque(Vector3.up * 0.1f, ForceMode.Impulse);
        }


    }

}
