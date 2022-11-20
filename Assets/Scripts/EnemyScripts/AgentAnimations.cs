using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentAnimations : MonoBehaviour
{
    private Animator animator;

    public float animationScaleMultipler = 1.25f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void RotateToPointer(Vector2 lookDirection, SpriteRenderer characterImg)
    {
        //Vector3 scale = transform.localScale;
        if (lookDirection.x > 0)
        {
            characterImg.flipX = false; //scale.x = 1 * animationScaleMultipler;
        }
        else if (lookDirection.x < 0)
        {
            characterImg.flipX = true;//scale.x = -1 * animationScaleMultipler;
        }
        //transform.localScale = scale;
    }

    public void PlayAnimation(Vector2 movementInput)
    {
        animator.SetBool("Running", movementInput.magnitude > 0);

    }
}
