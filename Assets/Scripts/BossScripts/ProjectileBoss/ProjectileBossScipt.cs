using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProjectileBossScipt : MonoBehaviour
{
    public static bool IsBossDead { get; set; } = false;

    public EnemyHealth bossCapsuleHealth;
    public Slider healthSlider;

    public UnityEvent OnBossDeath;
    public UnityEvent OnScreenFadeAfterDeath;

    public bool startOnAwake = false;

    private ProjectileBossAttack projAttackScript;
    private ProjectileBossMovement projMoveScript;

    private void Awake()
    {
        projAttackScript = GetComponent<ProjectileBossAttack>();
        projMoveScript = GetComponent<ProjectileBossMovement>();

        if (startOnAwake)
            StartBossFight();
    }

    private void OnDisable()
    {
        FadeAnimationScript.OnFaded -= OnScreenFadeSubscription;
    }

    private void Update()
    {
        healthSlider.value = bossCapsuleHealth.GetHealthPercentage();

        if (IsBossDead)
        {
            OnBossDeath?.Invoke();
            Debug.Log("Subscribing to event");
            FadeAnimationScript.OnFaded += OnScreenFadeSubscription;
            gameObject.SetActive(false);
        }
    }

    public void StartBossFight()
    {
        projAttackScript.StopAttacking = false;
        projMoveScript.StopMovement = false;
        projMoveScript.StopRotate = false;
    }

    private void OnScreenFadeSubscription()
    {
        Debug.Log("Moving Player");
        OnScreenFadeAfterDeath?.Invoke();
    }

}
