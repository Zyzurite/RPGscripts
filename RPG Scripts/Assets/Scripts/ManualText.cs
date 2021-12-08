using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualText : MonoBehaviour
{
    public GameObject player;
    private int stock;
    public string text;
    public bool potion;
    public bool xDebuff;
    public bool xBuff;
    public bool smokebomb;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (potion)
            stock = player.GetComponent<PlayerCombat>().potions;
        else if (xDebuff)
            stock = player.GetComponent<PlayerCombat>().xDebuff;
        else if (xBuff)
            stock = player.GetComponent<PlayerCombat>().xBuff;
        else if (smokebomb)
            stock = player.GetComponent<PlayerCombat>().smokeBomb;
        else
            gameObject.GetComponent<Text>().text = text;
        
        if(stock >= 0)
            gameObject.GetComponent<Text>().text = text + stock;
        
    }
}
