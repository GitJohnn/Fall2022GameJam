using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DashBossScript : MonoBehaviour
{
    public EnemyHealth tailHealth;
    public EnemyHealth headHealth;
    public int headAttackDamage;
    public GameObject headObjParent;
    public Slider bossHealthSlider;
    public bool startOnAwake = true;

    public static bool isBossDead = false;

    [SerializeField] UnityEvent onStopBossMovement;
    [SerializeField] UnityEvent onContinueBossMovement;
    [SerializeField] UnityEvent onBossDeath;
    [SerializeField] UnityEvent onScreenFadeAfterDeath;

    public float stopBossCooldown = 4f;
    private float currentBossCooldown = 0;
    private bool stopBossMovement = true;
    private bool runOnce = false;
    //private DashBossAttack dashBossAttack;
    private DashBossMovement dashBossMovement;

    private void Awake()
    {
        //Get components
        //dashBossAttack = GetComponent<DashBossAttack>();
        dashBossMovement = GetComponent<DashBossMovement>();

        if(startOnAwake)
            StartBossFight();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isBossDead);

        bossHealthSlider.value = tailHealth.GetHealthPercentage();

        if (isBossDead && !runOnce)
        {
            runOnce = true;
            onBossDeath?.Invoke();
            OnScreenFadeEventSubscription();
            if (MusicManager.Instance) MusicManager.Instance.SwapTrack();
            StartCoroutine(AfterFadeUnsubscribe());
            return;
        }
    }

    public void StartBossFight()
    {
        isBossDead = false;
        dashBossMovement.IsStopped = false;
        //dashBossAttack.CanAttack = false;
    }

    public void TailHealthDies()
    {
        //If tail is destroyed then boss dies
        isBossDead = true;
        if (MusicManager.Instance) MusicManager.Instance.SwapTrack();
        Debug.Log("Boss dead");
    }

    public void HeadAttack()
    {
        tailHealth.GetHit(headAttackDamage, gameObject);
        Debug.Log("Hurt boss head");
    }

    public void HeadHealthDies()
    {
        //If head is destroyed then add another health component and make the boss stop moving.   
        //isBossDead = true;
        Debug.Log("Boss movement stopped");
        stopBossMovement = true;
        onStopBossMovement?.Invoke();
        StartCoroutine(BossHeadHurtCoroutine());
    }

    private IEnumerator BossHeadHurtCoroutine()
    {
        while(currentBossCooldown < stopBossCooldown)
        {
            currentBossCooldown += Time.deltaTime;
            yield return null;
        }
        stopBossMovement = false;
        onContinueBossMovement?.Invoke();
        headHealth.ResetHealth();
        Debug.Log("Boss movement continue");
    }

    public void OnScreenFadeEventSubscription()
    {
        FadeAnimationScript.OnFaded += (ScreenFadeEvent);
    }

    private void ScreenFadeEvent()
    {
        onScreenFadeAfterDeath?.Invoke();
    }

    IEnumerator AfterFadeUnsubscribe()
    {
        yield return new WaitForSeconds(5f);
        FadeAnimationScript.OnFaded -= (ScreenFadeEvent);
        Debug.Log("Unsubscribing from event");
        gameObject.SetActive(false);
    }

}
