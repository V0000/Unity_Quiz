using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerHandler : MonoBehaviour
{
    public void OnButtonClick()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Right");
    }
}
