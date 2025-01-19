using UnityEngine;
using UnityEngine.InputSystem;

public class KnifeController : MonoBehaviour
{
    [SerializeField] private Animator knifeAnimator;
    [SerializeField] private AudioSource cuttingSound;
    [SerializeField] private AudioClip[] cuttingClips;


    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TriggerCut();
            PlayRandomCuttingSound();
        }
    }

    private void TriggerCut()
    {
        knifeAnimator.SetTrigger("Cut");        
    }


    private void PlayRandomCuttingSound()
    {
        if (cuttingClips.Length > 0)
        {
            int randomIndext = Random.Range(0, cuttingClips.Length);
            cuttingSound.clip = cuttingClips[randomIndext];
            cuttingSound.volume = Random.Range(0.8f, 1.0f);
            cuttingSound.Play();
        }
    }
}
