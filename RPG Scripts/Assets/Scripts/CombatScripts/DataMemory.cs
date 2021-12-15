using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMemory : MonoBehaviour
{
    public float health = 100;
    public float attack = 1.5f;
    public float defence = 1.5f;
    public float baseHealth;
    public float baseAttack;
    public float baseDefence;
    // Start is called before the first frame update
    void Start()
    {
        baseHealth = health;
        baseDefence = defence;
        baseAttack = attack;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (attack <= 0.1)
            attack = 0.4f;
        if (defence <= 0.1)
            defence = 0.4f;
        if (health > baseHealth)
            health = baseHealth;
    }
}
