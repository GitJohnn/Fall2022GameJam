using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private AgentMover agentMover;


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
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = Vector2.zero;

        agentMover.MovementInput = MovementInput;
    }
}
