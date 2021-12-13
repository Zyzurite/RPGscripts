using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;

public class PlayerCombat : MonoBehaviour
{
    // Enemy detection and stats
    public GameObject combatEnemy;
    private GameObject enemy;
    private DataMemory enemyStats;

    // Player stats
    private DataMemory playerStats;
    public int experience;
    public int requiredXP = 20;
    public int level = 1;

    // UI booleans and tools
    public GameObject movesUI;
    public GameObject itemsUI;
    public bool playerTurn;
    public bool combat;
    private bool combatUI;
    private bool inventory;

    // Inventory ints
    public int potions;
    public int xDebuff;
    public int xBuff;
    public int smokeBomb;



    // Start is called before the first frame update
    void Start()
    {
        playerStats = gameObject.GetComponent<DataMemory>();
    }

    private void FixedUpdate()
    {
        if (combat)
            gameObject.GetComponent<SimplePlayerController>().enabled = false;
        else if (!combat)
            gameObject.GetComponent<SimplePlayerController>().enabled = true;

        CheckXP();
        registerUI();

        if (combatEnemy != null && enemy != null)
            enemyStats = enemy.GetComponent<DataMemory>();
    }

    public void Attack()
    {
        ClearConsole();
        float damage = Random.Range(30, 40);
        damage = (damage * playerStats.attack / (enemyStats.defence + 1)) + 1;
        enemyStats.health -= (int) damage;
            
        print((int) damage + " damage dealt");
        print("Enemy has " + (int) enemyStats.health + " remaining health");
        playerTurn = false;
    }

    public void RaiseStat()
    {
        ClearConsole();
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

        switch (Random.Range(0, 2))
        {
            case 0:
                playerStats.attack += increase;
                print("Your attack has been increased by " + increase);
                break;
            case 1:
                playerStats.defence += increase;
                print("Your defence has been increased by " + increase);
                break;
                    
        }
        print("Enemy has " + (int) enemyStats.health + " remaining health");
        playerTurn = false;
    }

    public void ReduceStat()
    {
        ClearConsole();
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

        switch (Random.Range(0, 2))
        {
            case 0:
                enemyStats.attack -= decrease;
                print("Enemy attack decreased by " + decrease);
                break;
            case 1:
                enemyStats.defence -= decrease;
                print("Enemy defence decreased by " + decrease);
                break;

        }
        print("Enemy has " + (int) enemyStats.health + " remaining health");
        playerTurn = false;
    }

    public void SpecialAttack()
    {
        ClearConsole();
        bool success;
        int hitChance = Random.Range(1, 5);
        float damage = Random.Range(50, 70);

        if (hitChance >= 1 && hitChance <= 3)
            success = true;
        else if (hitChance >= 4)
            success = false;
        else
            return;
        if (success)
        {
            damage = (damage * playerStats.attack / (enemyStats.defence + 1)) + 1;
            print((int) damage + " damage dealt");
            enemyStats.health -= (int) damage;
        }
        else
            print("Attack missed!");
            
        print("Enemy has " + (int) enemyStats.health + " remaining health");
        playerTurn = false;
    }

    public void CheckStats()
    {
        ClearConsole();
        print("Your health is " + (int)playerStats.health);
        print("Your attack is " + playerStats.attack.ToString("0.0"));
        print("Your defence is " + playerStats.defence.ToString("0.0"));
        print("Enemy health is " + (int)enemyStats.health);
        print("Enemy attack is " + enemyStats.attack.ToString("0.0"));
        print("Enemy defence is " + enemyStats.defence.ToString("0.0"));
    }


    public void ClearConsole()
    {
        var assembly = Assembly.GetAssembly(typeof(SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    public void CheckXP()
    {
        if (experience >= requiredXP)
        {
            
            level += 1;
            print("Level up! You are now level " + level);
            print("Your base stats have been raised");
            playerStats.baseHealth += 10;
            playerStats.baseAttack += 0.2f;
            playerStats.baseDefence += 0.2f;
            playerStats.health += 10;
            experience -= requiredXP;
            requiredXP += 15;
            
        }
    }

    public void BattleStart()
    {
        playerTurn = true;
        ClearConsole();
        playerStats.attack = playerStats.baseAttack;
        playerStats.defence = playerStats.baseDefence;
        enemy = combatEnemy;
        enemy.GetComponent<EnemyCombat>().player = gameObject;
        combat = true;
        combatUI = true;
        inventory = false;
    }

    public void registerUI()
    {
        if (combatUI && playerTurn)
        {
            movesUI.SetActive(true);
            itemsUI.SetActive(false);
        }
        else if (inventory && playerTurn)
        {
            itemsUI.SetActive(true);
            movesUI.SetActive(false);
        }
        else if (!playerTurn)
        {
            movesUI.SetActive(false);
            itemsUI.SetActive(false);
        }
    }

    public void swapUI()
    {
        if (combatUI)
        {
            inventory = true;
            combatUI = false;
        }
        else if(inventory)
        {
            combatUI = true;
            inventory = false;
        }
    }

    public void Potion()
    {
        ClearConsole();
        if (potions >= 1)
        {
            playerStats.health += 50;
            print("Some health has been restored");
            potions -= 1;
            playerTurn = false;
        }
        else
            print("Not enough potions!");
    }
    public void XBuff()
    {
        ClearConsole();
        if (xBuff >= 1)
        {
            playerStats.defence += 0.8f;
            playerStats.attack += 0.8f;
            print("Your stats have been dramatically raised!");
            xBuff -= 1;
            playerTurn = false;
        }
        else
            print("Not enough XBuffs!");
    }
    public void XDebuff()
    {
        ClearConsole();
        if (xDebuff >= 1)
        {
            enemyStats.defence -= 0.5f;
            enemyStats.attack -= 0.5f;
            print("Enemy stats have been dramatically decreased!");
            xDebuff -= 1;
            playerTurn = false;
        }
        else
            print("Not enough XDebuffs!");
    }

    public void Smokebomb()
    {
        ClearConsole();
        if (smokeBomb >= 1)
        {
            print("You flee from the battle!");
            enemy.GetComponent<EnemyCombat>().player = null;
            enemy = null;
            combatEnemy = null;
            combat = false;
            playerTurn = false;
            combatUI = true;
            smokeBomb -= 1;
        }
        else
            print("Not enough Smokebombs!");
    }
}
