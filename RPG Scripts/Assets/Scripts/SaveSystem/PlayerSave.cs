using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSave
{
    public int experience;
    public int level;
    public int potions;
    public int xBuffs;
    public int xDebuffs;
    public int smokebomb;
    public int requiredXP;
    public float baseHealth;
    public float baseAttack;
    public float baseDefence;
    public float health;

    public PlayerSave(PlayerCombat player, DataMemory playerstats)
    {
        experience = player.experience;
        level = player.level;
        potions = player.potions;
        xBuffs = player.xBuff;
        xDebuffs = player.xDebuff;
        smokebomb = player.smokeBomb;
        requiredXP = player.requiredXP;
        baseHealth = playerstats.baseHealth;
        baseAttack = playerstats.baseAttack;
        baseDefence = playerstats.baseDefence;
        health = playerstats.health;
    }
}
