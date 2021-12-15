using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviour
{
    private PlayerCombat player;
    private DataMemory playerstats;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<PlayerCombat>();
        playerstats = gameObject.GetComponent<DataMemory>();
    }

    public void Sceneswap()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(TelePlayer());

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
        player.scene = data.scene;
        print("Stats have been loaded");
    }

    public void NewGame()
    {   
        player.experience = 0;
        player.level = 1;
        player.potions = 1;
        player.xBuff = 1;
        player.xDebuff = 1;
        player.smokeBomb = 1;
        player.requiredXP = 20;
        playerstats.baseAttack = 1.5f;
        playerstats.baseDefence = 1.5f;
        playerstats.baseHealth = 100;
        playerstats.health = 100;
        StartCoroutine(TelePlayer());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadGame()
    { 
        player.ClearConsole();
        PlayerSave data = SaveSystem.LoadPlayer();
        if (data.scene > 0)
        {
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
            player.scene = data.scene;
            SceneManager.LoadScene(player.scene);
            StartCoroutine(TelePlayer());
            print("Stats have been loaded");
        }
        else
            print("No save data found");
    }

    IEnumerator TelePlayer()
    {
        gameObject.GetComponent<SimplePlayerController>().enabled = false;
        player.enabled = false;
        yield return new WaitForSeconds(1f);
        gameObject.transform.position = new Vector3(0, 3, 0);
        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<SimplePlayerController>().enabled = true;
        player.enabled = true;
    }
}
