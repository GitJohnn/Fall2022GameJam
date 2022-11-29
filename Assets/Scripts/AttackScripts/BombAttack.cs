using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;
public class BombAttack : Attack
{
    public ButtonCoolDownHandler attackButton;
    public GameObject bombPrefab;

    public bool bombAttackUnlocked = true;
    public bool usePlayerStats = false;
    [SerializeField, ShowIf("usePlayerStats")] PlayerStats stats;
    [SerializeField] private float totalCooldown;

    [SerializeField] SFXEvent _sfxBombPlace;

    public void Awake()
    {
        if (bombAttackUnlocked)
            BombUnlocked();
        else
            BombLocked();
    }

    private void Update()
	{
		if (!bombAttackUnlocked) return;
        attackButton.CurrentAbilityCooldown = currentAttackCooldown;

		currentAttackCooldown += Time.deltaTime;
		if(currentAttackCooldown >= totalCooldown) canAttack = true;
    }

    public void BombUnlocked()
	{
		bombAttackUnlocked = true;
        canAttack = true;
        attackButton.SetTotalcooldown(totalCooldown);
        currentAttackCooldown = totalCooldown;
    }

    private void BombLocked()
	{
		bombAttackUnlocked = false;
        canAttack = false;
    }

	public float currentAttackCooldown; /*{ get; set; }*/

    bool canAttack;

    public override void HandlePrimaryAttack()
    {
        if (!canAttack) return;
		//StartCoroutine(RangeAttack());
		SpawnBomb();
        _onPrimaryAttack?.Invoke();
    }

    IEnumerator RangeAttack()
    {
        canAttack = false;
        _sfxBombPlace.Play();
        currentAttackCooldown = 0;

        GameObject bombrb = Instantiate(bombPrefab, _attackPosition.position, Quaternion.identity);
        //projectile.AddForce(_attackPosition.right * projectileSpeed, ForceMode2D.Impulse);

        while (currentAttackCooldown <= totalCooldown)
        {
            currentAttackCooldown += Time.deltaTime;
            yield return null;
        }

        canAttack = true;
    }

	private void SpawnBomb() {
		_sfxBombPlace.Play();
		canAttack = false;
		currentAttackCooldown = 0;
		GameObject bomb = Instantiate(bombPrefab, _attackPosition.position, Quaternion.identity);
        bomb.GetComponent<BombScript>().damageAmount = Mathf.RoundToInt(stats.AttackPower);
	}
}
