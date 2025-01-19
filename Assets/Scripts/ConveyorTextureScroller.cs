using UnityEngine;

public class ConveyorTextureScroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    public Renderer conveyorRenderer; 

    private void Update()
    {
        float offset = Time.time * scrollSpeed;
        conveyorRenderer.material.mainTextureOffset = new Vector2(offset, 0); 
    }

}
