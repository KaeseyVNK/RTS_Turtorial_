using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private float unitHealth;
    public float unitMaxHealth;

    public HealthTracker healthTracker;
    
    private void Start()
    {
        UnitSelectionManager.Instance.allUnitList.Add(gameObject);

        unitHealth = unitMaxHealth;

        UpdateHealthUI();
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnitList.Remove(gameObject);
    }

    private void UpdateHealthUI()
    {
        healthTracker.UpdateSliderValue( unitHealth, unitMaxHealth);

        if ( unitHealth <= 0 )
        {
            //Dying Logic

            //Destruction or Dying Animation

            //Dying Sound Effect
            Destroy(gameObject);
        }
    }

    internal void TakeDamge(int damageToInflict)
    {
        unitHealth -= damageToInflict; 
        UpdateHealthUI();
    }
}
