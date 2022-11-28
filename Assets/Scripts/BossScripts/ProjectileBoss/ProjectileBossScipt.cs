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

    private bool runOnce = false;

    private void Awake()
    {
        projAttackScript = GetComponent<ProjectileBossAttack>();
        projMoveScript = GetComponent<ProjectileBossMovement>();

        if (startOnAwake)
            StartBossFight();
    }

    private void OnDisable()
    {
        //FadeAnimationScript.OnFaded -= OnScreenFadeSubscription;
    }

    private void Update()
    {
        healthSlider.value = bossCapsuleHealth.GetHealthPercentage();

        if (IsBossDead && !runOnce)
        {

            runOnce = true;
            OnBossDeath?.Invoke();
            Debug.Log("Subscribing to event");
            FadeAnimationScript.OnFaded += (ScreenFadeEvent);
            StartCoroutine(AfterFadeUnsubscribe());
        }
    }

    public void StartBossFight()
    {
        IsBossDead = false;
        projAttackScript.StopAttacking = false;
        projMoveScript.StopMovement = false;
        projMoveScript.StopRotate = false;
    }

    private void ScreenFadeEvent()
    {
        OnScreenFadeAfterDeath?.Invoke();
    }

    IEnumerator AfterFadeUnsubscribe()
    {
        yield return new WaitForSeconds(5f);
        FadeAnimationScript.OnFaded -= (ScreenFadeEvent);
        Debug.Log("Unsubscribing from event");
        gameObject.SetActive(false);
    }

}
