using UnityEngine;
using System.Collections;

public class DCCWindowTransition : MonoBehaviour
{
    public Animator wtAnimator;
    public bool isPlaying;

	public void PlayTransition ()
    {
        //Debug.Log("Playing transition animation....");
        wtAnimator.SetTrigger("doTransition");
    }
}
