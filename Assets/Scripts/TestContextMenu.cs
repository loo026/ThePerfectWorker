using UnityEngine;
using UnityEngine.Scripting;

public class TestContextMenu : MonoBehaviour
{
    [ContextMenu("Test Function")]
    private void TestFunction()
    {
        Debug.Log("Test Function Executed!");
    }
}
