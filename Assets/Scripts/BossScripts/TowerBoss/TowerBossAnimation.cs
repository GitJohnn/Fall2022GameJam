using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBossAnimation : MonoBehaviour
{
    public Animator SpiderAnimator;
    
    public void SetSpiderAnimation(bool value)
    {
        SpiderAnimator.SetBool("isDizzy", value);
    }

}
