using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private AgentMover agentMover;
    private AgentAnimations agentAnimations;

    private EnemyAI enemyAI;

    private Vector2 pointerInput, movementInput;

    public Vector2 PointerInput
    {
        get;
        set;
    }

    public Vector2 MovementInput
    {
        get;
        set;
    }
    
    void Awake()
    {
        agentMover = GetComponent<AgentMover>();
        agentAnimations = GetComponent<AgentAnimations>();
        enemyAI = GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = Vector2.zero;

        pointerInput = enemyAI.WeaponParent.PointerPosition;
        agentMover.MovementInput = MovementInput;
        AnimateCharacters();
    }

    private void AnimateCharacters()
    {
        Vector2 lookDirection = (pointerInput - (Vector2)transform.position);
        agentAnimations.RotateToPointer(lookDirection, enemyAI.WeaponParent.characterRenderer);
        //agentAnimations.PlayAnimation(MovementInput);
    }
}
