using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    public AIBehavior aiBehavior = AIBehavior.ChaseAndAttack;

    [SerializeField] private LayerMask playerLayerMask, obstacleLayerMask;

    [SerializeField]
    public AttackSettings attackSettings;
    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 1f;

    public bool showGizmo = false;

    [SerializeField]
    private float attackDistance = 0.5f;
    [SerializeField]
    private float runAwayRadius = 0.25f;

    //Inputs sent from the Enemy AI to the Enemy controller
    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    [SerializeField]
    private Vector2 movementInput;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    [SerializeField]
    private WeaponParent weaponParent;

    public WeaponParent WeaponParent
    {
        get {
            return weaponParent;
        }
    }

    bool following = false;
    bool runningAway = false;

    private void Start()
    {
        //Detecting Player and Obstacles around
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }

        //float[] danger = new float[8];
        //float[] interest = new float[8];

        //foreach(SteeringBehaviour behavior in steeringBehaviours)
        //{
        //    (danger, interest) = behavior.GetSteering(danger, interest, aiData);
        //}
    }

    private void Update()
    {
        //Enemy AI movement based on Target availability
        if (aiData.currentTarget != null)
        {
            //Looking at the Target
            OnPointerInput?.Invoke(aiData.currentTarget.position);
            if (!following)
            {
                following = true;
                CallAIBehavior(aiBehavior);
                //StartCoroutine(ChaseAndAttack());
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            //Target acquisition logic
            aiData.currentTarget = aiData.targets[0];
        }
        //Moving the Agent
        OnMovementInput?.Invoke(movementInput);
    }

    private void CallAIBehavior(AIBehavior value)
    {
        switch (value)
        {
            case AIBehavior.ChaseAndAttack:
                StartCoroutine(ChaseAndAttack());
                break;
            case AIBehavior.RunAwayAndAttack:
                StartCoroutine(RunAwayAndAttack());
                break;
        }
    }

    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            //Stopping Logic
	        //Debug.Log("Stopping");
            movementInput = Vector2.zero;
            following = false;
            yield break;
        }
        else
        {
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

            if (distance <= attackDistance)
            {
                //Attack logic
                movementInput = Vector2.zero;
                OnAttackPressed?.Invoke();
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                //Chase logic
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
            }

        }

    }

    private IEnumerator RunAwayAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            //Stopping Logic
            Debug.Log("Stopping");
            movementInput = Vector2.zero;
            //following = false;
            yield break;
        }
        else
        {
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

            if (distance <= attackDistance)
            {
                //Attack logic
                movementInput = Vector2.zero;

                if (distance <= runAwayRadius)
                {
                    Debug.Log("Run Away");
                    movementInput = (transform.position - aiData.currentTarget.position).normalized;
                    //yield return null;
                }

                OnAttackPressed?.Invoke();
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(RunAwayAndAttack());

            }

            else
            {
                //Chase logic
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(RunAwayAndAttack());
            }

        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmo == false)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

    }
}

public enum AIBehavior
{
    ChaseAndAttack,
    RunAwayAndAttack
}
