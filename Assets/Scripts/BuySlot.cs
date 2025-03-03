using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySlot : MonoBehaviour
{
   public Sprite availableSprite;
   public Sprite unAvailableSprite;
 
    private bool isAvailable;
    public BuySystem buySystem;

    public int databaseItemID;

    private void Start() {

        ResourceManager.Instance.OnResourceChanged += HandleResourceChanged;
        //UpdateAvailbilityUI();
        HandleResourceChanged();
        
        ResourceManager.Instance.OnBuildingsChanged += HandleBuildingChanged;
        HandleBuildingChanged();
    }

    public void ClickedOnSlot() {
        if (isAvailable) {
            buySystem.placementSystem.StartPlacement(databaseItemID);
        }       
    }

    private void UpdateAvailbilityUI() {
        if (isAvailable) 
        {
            GetComponent<Image>().sprite = availableSprite;
            GetComponent<Button>().interactable = true;
        } else {
            GetComponent<Image>().sprite = unAvailableSprite;
            GetComponent<Button>().interactable = false;
        }
    }  


    private void HandleResourceChanged() {
        
        ObjectData objectData = DatabaseManager.Instance.databaseSO.objectsData[databaseItemID];

        bool requirementMet = true;

        foreach( BuildRequirement req in objectData.resourceRequirements)
        {
            if(ResourceManager.Instance.GetResourceAmount(req.resource) < req.amount)
            {
                requirementMet = false;
                break;
            }
        }

        isAvailable = requirementMet;
        UpdateAvailbilityUI();

    }


     private void HandleBuildingChanged (){
        ObjectData objectData = DatabaseManager.Instance.databaseSO.objectsData[databaseItemID];
        foreach(BuildingType dependency in objectData.buildDependency){
            // if the buiding has not dependencies
            if (dependency == BuildingType.None){
                gameObject.SetActive(true);
                return;
            }
            // if the buiding has dependencies
            if(ResourceManager.Instance.allExistingBuildings.Contains(dependency) == false){
                gameObject.SetActive(false);
                return;
            }
        }
        // if the buiding has dependencies and all dependencies are met
        gameObject.SetActive(true);
    }

}
