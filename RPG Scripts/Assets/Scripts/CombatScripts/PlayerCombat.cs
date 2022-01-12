using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    // Enemy detection and stats
    public GameObject combatEnemy;
    private GameObject enemy;
    private DataMemory enemyStats;
    public GameObject mainCamera;
    private EnemyCombat enemyCombat;

    // Player stats
    private DataMemory playerStats;
    public int experience;
    public int requiredXP = 20;
    public int level = 1;

    // UI booleans and tools
    private GameObject movesUI;
    private GameObject itemsUI;
    public bool playerTurn;
    private bool combat;
    private bool combatUI;
    private bool inventory;
    public int scene;

    // Inventory ints
    public int potions;
    public int xDebuff;
    public int xBuff;
    public int smokeBomb;
    private Vector3 cameraPos;
    private Vector3 cameraRot;
    public bool resetCamera;



    // Start is called before the first frame update
    void Awake()
    {
        playerStats = gameObject.GetComponent<DataMemory>();

        movesUI = GameObject.FindGameObjectWithTag("Moves");
        itemsUI = GameObject.FindGameObjectWithTag("Items");

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        cameraPos = mainCamera.transform.localPosition;
        cameraRot = mainCamera.transform.localEulerAngles;
    }


    private void FixedUpdate()
    {

        
        if (combat)
            gameObject.GetComponent<SimplePlayerController>().enabled = false;
        else if (!combat)
            gameObject.GetComponent<SimplePlayerController>().enabled = true;

        CheckXP();
        registerUI();

        if (combatEnemy != null && enemy != null && !resetCamera)
        {
            enemyStats = enemy.GetComponent<DataMemory>();
            enemyCombat = enemy.GetComponent<EnemyCombat>();
            combat = true;
            ProcessCamera();
        }
        else
        {
            combat = false;
            StopAllCoroutines();
        }

        scene = SceneManager.GetActiveScene().buildIndex;

        if (resetCamera)
            ReProcessCamera();
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
            resetCamera = true;
        }
        else
            print("Not enough Smokebombs!");
    }

    public void ProcessCamera()
    {
        this.gameObject.transform.position = enemyStats.gameObject.transform.position + new Vector3(0 - enemyCombat.extraXDistance, 0.85f - enemyCombat.extraYDistance, -10 - enemyCombat.extraZDistance);
        this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);

        cameraAdjust(2.2f, 0.5f, -2.2f, 0, -20, 0);
    }

    private void ReProcessCamera()
    {
        mainCamera.transform.localPosition = cameraPos;
        mainCamera.transform.localEulerAngles = cameraRot;
        resetCamera = false;
    }

    public void cameraAdjust(float posX,float posY, float posZ, float rotX, float rotY, float rotZ)
    {
        mainCamera.transform.localPosition = new Vector3(posX, posY, posZ);
        mainCamera.transform.localEulerAngles = new Vector3(rotX, rotY, rotZ);
    }
}