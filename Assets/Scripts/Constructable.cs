using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class Constructable : MonoBehaviour, IDamageable
{

    private float constHealth; 
    public float constMaxHealth; 

    public HealthTracker healthTracker;

    public bool isEnemy = false;

    public BuildingType buildingType;

    public UnityEngine.Vector3 buildingPosition;

    public bool inPreviewMode;

    private void Start(){
        constHealth = constMaxHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI(){
        healthTracker.UpdateSliderValue(constHealth, constMaxHealth);

        if ( constHealth<= 0 ){
            //Other destruction logic
            
            ResourceManager.Instance.UpdateBuildingChanged(buildingType, false, buildingPosition);

            SoundManager.Instance.PlayBuildingDestructionSound();

            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(inPreviewMode == false)
        {
            if(constHealth > 0 && buildingPosition != UnityEngine.Vector3.zero)
            {        
                ResourceManager.Instance.SellBuilding(buildingType);
            }
        }  
    }


    public void TakeDamage(float damageAmount){
        constHealth -= damageAmount;
        UpdateHealthUI();
    }

    NavMeshObstacle obstacle;

    public void ConstructableWasPlaced(UnityEngine.Vector3 position)
    {
        buildingPosition = position;
        inPreviewMode = false;

        // Make healthbar visible 
        healthTracker.gameObject.SetActive(true);

         ActivateObstacle();
        
        if (isEnemy)
        {
            gameObject.tag = "Enemy";
        }

        if (GetComponent<PowerUser>() != null)
        {   
            GetComponent<PowerUser>().PowerOn();
        }

    }

    private void ActivateObstacle() 
    {
        obstacle = GetComponentInChildren<NavMeshObstacle>();
        obstacle.enabled = true;
    }
}
