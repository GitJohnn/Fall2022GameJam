using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Potion")]
public class PotionBase : ScriptableObject {

    [SerializeField] private PlayerStat statToBuff;
    [SerializeField] private float buffPercentage;
    [SerializeField] private PlayerStat statToDebuff;
    [SerializeField] private float debuffPercentage;
    [SerializeField] private float toxicity;

    //TODO: set up how to display this information to the player before they take (strings, images, etc)
    //          (this could be done on another manager, but idk)


    //TODO: play around with this and figure out how to pass in the stats most efficiently
    public void ApplyPotion(PlayerStats player) {
        AdjustStat(player, statToBuff, buffPercentage);
        AdjustStat(player, statToDebuff, -debuffPercentage);
        player.ChangeToxicity(toxicity);
    }

    private void AdjustStat(PlayerStats player, PlayerStat stat, float percentage) {
        switch(stat) {
            case PlayerStat.MaxHealth:
                player.ChangeMaxHealth(percentage);
                break;
            case PlayerStat.AttackPower:
                player.ChangeAttackPower(percentage);
                break;
            case PlayerStat.Defense:
                player.ChangeDefense(percentage);
                break;
            case PlayerStat.Speed:
                player.ChangeSpeed(percentage);
                break;
            case PlayerStat.AttackSpeed:
                player.ChangeAttackSpeed(percentage);
                break;
        }
    }
}