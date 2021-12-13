using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviour
{
    private PlayerCombat player;
    private DataMemory playerstats;
    public bool swap;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<PlayerCombat>();
        playerstats = gameObject.GetComponent<DataMemory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (swap)
            Sceneswap();
    }
    public void Sceneswap()
    {
        swap = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        gameObject.transform.position = new Vector3(0, 0, 0);
        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        
    }

    public void saveCheckpoint()
    {
        player.ClearConsole();
        SaveSystem.SavePlayer(player, playerstats);
        print("Stats have been saved");
    }

    public void loadCheckpoint()
    {
        player.ClearConsole();
        PlayerSave data = SaveSystem.LoadPlayer();
        player.experience = data.experience;
        player.level = data.level;
        player.potions = data.potions;
        player.xBuff = data.xBuffs;
        player.xDebuff = data.xDebuffs;
        player.smokeBomb = data.smokebomb;
        player.requiredXP = data.requiredXP;
        playerstats.baseAttack = data.baseAttack;
        playerstats.baseDefence = data.baseDefence;
        playerstats.baseHealth = data.baseHealth;
        playerstats.health = data.health;
        print("Stats have been loaded");
    }
}
