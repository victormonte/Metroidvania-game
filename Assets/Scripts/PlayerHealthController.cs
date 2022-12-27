using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // Singleton
    public static PlayerHealthController instance;

    private void Awake()
    {
        instance = this;
    }

    //[HideInInspector] // make it hidden but public for other components to read it
    public int currentHealth;
    public int maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            gameObject.SetActive(false);
        }

        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }
}
