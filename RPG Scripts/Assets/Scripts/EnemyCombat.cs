using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public GameObject player;
    private PlayerCombat playerCombat;
    private DataMemory playerStats;
    private DataMemory enemyStats;
    private int experience;
    private bool hasRun;
    public float extraIncrease;
    public float extraDecrease;
    public int extraExperience;
    public int extraDamage;
    public bool boss;
    void Start()
    {
        enemyStats = gameObject.GetComponent<DataMemory>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (player != null)
        {
            playerCombat = player.GetComponent<PlayerCombat>();
            playerStats = player.GetComponent<DataMemory>();
            

            healthCheck();

            if (!playerCombat.playerTurn)
            {
                StartCoroutine("RollMove");
            }
        }    
        
    }

    public void healthCheck()
    {
        if (enemyStats.health <= 1 && !hasRun)
        {
            experience = Random.Range(20 + extraExperience, 40 + extraExperience);
            print("The enemy has ran out of health, battle won!");
            print("You gained " + experience + " experience");
            print("Some of your health has been restored");
            playerStats.health += 50;
            playerCombat.experience += experience;
            hasRun = true;
            playerCombat.combat = false;
            ItemDrop();
            Destroy(gameObject);
        }
    }
    public void Attack()
    {
        print("");
        float damage = Random.Range(10 + extraDamage, 25 + extraDamage);
        damage = (damage * enemyStats.attack / (playerStats.defence + 1)) + 1;
        playerStats.health -= (int) damage;

        print("The enemy attacked and dealt " + (int) damage + " damage!");
        print("You have " + (int) playerStats.health + " health remaining");
        playerCombat.playerTurn = true;
    }

    public void RaiseStat()
    {
        print("");
        print("The enemy focuses its own energy");
        float increase;
        int baseIncrease = Random.Range(1, 11);
        if (baseIncrease >= 1 && baseIncrease <= 4)
            increase = 1;
        else if (baseIncrease >= 5 && baseIncrease <= 7)
            increase = 2;
        else if (baseIncrease >= 8 && baseIncrease <= 9)
            increase = 3;
        else if (baseIncrease == 10)
            increase = 4;
        else
            return;

        increase /= 10;
        increase += extraIncrease;

        switch (Random.Range(0, 2))
        {
            case 0:
                enemyStats.attack += increase;
                print("Enemy attack has been increased by " + increase);
                break;
            case 1:
                enemyStats.defence += increase;
                print("Enemy defence has been increased by " + increase);
                break;

        }
        playerCombat.playerTurn = true;
    
    }

    public void ReduceStat()
    {
        print("");
        print("The enemy lowers your stats");
        float decrease;
        int baseDecrease = Random.Range(1, 11);
        if (baseDecrease >= 1 && baseDecrease <= 5)
            decrease = 2;
        else if (baseDecrease >= 6 && baseDecrease <= 9)
            decrease = 3;
        else if (baseDecrease == 10)
            decrease = 4;
        else
            return;

        decrease /= 10;
        decrease += extraDecrease;

        switch (Random.Range(0, 2))
        {
            case 0:
                playerStats.attack -= decrease;
                print("Your attack decreased by " + decrease);
                break;
            case 1:
                playerStats.defence -= decrease;
                print("Your defence decreased by " + decrease);
                break;

        }
        playerCombat.playerTurn = true;
    }

    public void SpecialAttack()
    {
        print("");
        float damage = Random.Range(25 + extraDamage, 40 + extraDamage);
        damage = (damage * enemyStats.attack / (playerStats.defence + 1)) + 1;
        playerStats.health -= (int) damage;

        print("The enemy did a special attack!");
        print("The enemy dealt " + (int)damage + " damage!");
        print("You have " + (int)playerStats.health + " health remaining");
        playerCombat.playerTurn = true;
    }

    IEnumerator RollMove()
    {
        int move = Random.Range(1, 11);
        yield return new WaitForSeconds(0.5f);
        if (move >= 1 && move <= 5 && !hasRun)
            Attack();
        if (move >= 6 && move <= 7 && !hasRun)
            RaiseStat();
        if (move >= 8 && move <= 9 && !hasRun)
            ReduceStat();
        if (move == 10 && !hasRun)
            SpecialAttack();
        StopAllCoroutines();
    }

    public void ItemDrop()
    {
        if (!boss)
        {
                switch (Random.Range(1,6))
            {
                case 1:
                    print("The enemy dropped a potion");
                    playerCombat.potions += 1;
                    break;
                case 2:
                    print("The enemy dropped an XBuff");
                    playerCombat.xBuff += 1;
                    break;
                case 3:
                    print("The enemy dropped an XDebuff");
                    playerCombat.xDebuff += 1;
                    break;
                case 4:
                    print("The enemy dropped a Smokebomb");
                    playerCombat.smokeBomb += 1;
                    break;
                case 5:
                    print("No item drop");
                    break;
            }
        }

        if(boss)
        {
            for (int i = 0; i < 3; i++)
            {
                switch (Random.Range(1, 5))
                {
                    case 1:
                        print("The enemy dropped a potion");
                        playerCombat.potions += 1;
                        break;
                    case 2:
                        print("The enemy dropped an XBuff");
                        playerCombat.xBuff += 1;
                        break;
                    case 3:
                        print("The enemy dropped an XDebuff");
                        playerCombat.xDebuff += 1;
                        break;
                    case 4:
                        print("The enemy dropped a Smokebomb");
                        playerCombat.smokeBomb += 1;
                        break;
                }
            }
        }
    }
}
