using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;

public class FishController : MonoBehaviour
{
/*    public delegate void CutEventHandler(FishController fish);
    public static event CutEventHandler OnFishCut;*/

    [SerializeField] private Rigidbody fishHeadRb;
    [SerializeField] private Rigidbody fishBodyRb;
    [SerializeField] private Vector3 cutPosition;
    [SerializeField] private Vector3 recyclePosition;

    private Vector3 headInitialPosition;
    private Quaternion headInitialRotation;
    private Vector3 bodyInitialPosition;
    private Quaternion bodyInitialRotation;

    private bool isCut = false;
    private FishManager fishManager;



    public void Start()
    {
        fishHeadRb.isKinematic = true;
        fishBodyRb.isKinematic = true;

        headInitialPosition = fishHeadRb.transform.localPosition;
        headInitialRotation = fishHeadRb.transform.localRotation;

        bodyInitialPosition = fishBodyRb.transform.localPosition;
        bodyInitialRotation = fishBodyRb.transform.localRotation;

        fishManager = FindFirstObjectByType<FishManager>();

    }

    private void Update()
    {
        CheckRecycle();
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


    private void CheckRecycle()
    {
        if (transform.position.x >= recyclePosition.x)
        {
            fishManager.RecycleFish(gameObject); 
            Debug.Log("Fish recycled.");
        }
    }

    public void ResetChildTransform()
    {

        fishHeadRb.transform.localPosition = headInitialPosition;
        fishHeadRb.transform.localRotation = headInitialRotation;

        fishBodyRb.transform.localPosition = bodyInitialPosition;
        fishBodyRb.transform.localRotation = bodyInitialRotation;


        fishHeadRb.isKinematic = true;
        fishBodyRb.isKinematic = true;

        isCut = false; 
        Debug.Log("Fish has been reset.");
    }
}
