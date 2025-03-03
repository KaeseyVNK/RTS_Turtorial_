using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance{get; set;}

    private void Awake(){
        if(Instance != null && Instance != this ){
            Destroy(gameObject);
        }
        else 
        {
            Instance = this;
        }
    }
    
    public int credits = 300;

    public event Action OnResourceChanged;
    public event Action OnBuildingsChanged;
    public TextMeshProUGUI creditsUI;

    public List<BuildingType> allExistingBuildings;

    public PlacementSystem placementSystem;


    public enum ResourceType
    {
        Credits,
    }
    
    private void Start(){
        UpdateUI();
    }

    public void UpdateBuildingChanged(BuildingType buildingType, bool isNew, Vector3 position){
        if(isNew)
        {
            allExistingBuildings.Add(buildingType);
            SoundManager.Instance.PlayBuildingConstructionSound();
        }
        else
        {
            placementSystem.RemovePlacementData(position);
            allExistingBuildings.Remove(buildingType);
        }
        // if the building is new or removed, invoke the event
        OnBuildingsChanged?.Invoke();
    }

    public void IncreaseResource(ResourceType resourceType, int amountToIncrease){
        switch(resourceType){
            case ResourceType.Credits:
                credits += amountToIncrease;
                creditsUI.text = $"{credits}";
                break;
            default:
                Debug.Log("Invalid resource type");
                break;
        }

        OnResourceChanged?.Invoke();
    }

    public void DecreaseResource(ResourceType resourceType, int amountToDecrease){    
        switch(resourceType){
            case ResourceType.Credits:
                credits -= amountToDecrease;
                creditsUI.text = $"{credits}";
                break;
            default:
                Debug.Log("Invalid resource type");
                break;
        }

        OnResourceChanged?.Invoke();
    }

    void OnEnable(){
        OnResourceChanged += UpdateUI;
    }

    void OnDisable(){
        OnResourceChanged -= UpdateUI;
    }

    public void SellBuilding(BuildingType buildingType)
    {
        SoundManager.Instance.PlayBuildingSellingSound();
        var sellingPrice = 0;
        foreach(ObjectData objectData in DatabaseManager.Instance.databaseSO.objectsData){
            if(objectData.thisBuildingType == buildingType)
            {
                foreach(BuildRequirement req in objectData.resourceRequirements){
                    if (req.resource == ResourceType.Credits){
                        sellingPrice = req.amount;
                    }
                }              
            }
        }    

        int amountToReturn = (int)(sellingPrice * 0.75f); //75% of the selling price

        IncreaseResource(ResourceType.Credits, amountToReturn);
    }

    private void UpdateUI()
    {
        creditsUI.text = $"{credits}";
    }

    public int GetCredits(){
        return credits;
    }

    public void AddCredits(int amount){
        credits += amount;
    }

    internal int GetResourceAmount(ResourceType resourceType)
    {
        switch(resourceType){
            case ResourceType.Credits:
                return credits;
            default:
                Debug.Log("Invalid resource type");
                return 0;
        }
    }

    internal void DecreaseResourcesBasedOnRequirement(ObjectData objectData)
    {
        foreach(BuildRequirement req in objectData.resourceRequirements){
            DecreaseResource(req.resource, req.amount);
        }
    }

}
