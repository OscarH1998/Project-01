using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth { get; private set; } = 100;
    public int currentHealth { get; private set; } = 100;

    //[SerializeField] AudioSource PlayerDamaged = null;

    public event Action<int> HealthChanged = delegate { };

    public void TakeDamage(int damage)
    {
       // PlayerDamaged.Play();
        currentHealth -= damage;
        HealthChanged.Invoke(currentHealth);
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HealthPickup")
        {
            if (currentHealth < maxHealth)
            {
                currentHealth = maxHealth;
                HealthChanged.Invoke(currentHealth);
                other.gameObject.SetActive(false);
            }
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)){
            TakeDamage(20);
            Debug.Log("Current Health: " + currentHealth);
        }
    }
}
