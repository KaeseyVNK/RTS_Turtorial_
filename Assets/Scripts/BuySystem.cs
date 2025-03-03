using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySystem : MonoBehaviour
{
    public GameObject buildingPanel; 
    public GameObject unitsPanel;
    
    public Button buildingButton;

    public Button unitsButton;

    public PlacementSystem placementSystem;

    private void Start() {
        unitsButton.onClick.AddListener(UnitsCategorySelected);   
        buildingButton.onClick.AddListener(BuildingCategorySelected); 

        unitsPanel.SetActive(false);
        buildingPanel.SetActive(true);


    }
    
    public void UnitsCategorySelected()
    {
        unitsPanel.SetActive(true);
        buildingPanel.SetActive(false);
    }

    public void BuildingCategorySelected()
    {
        unitsPanel.SetActive(false);
        buildingPanel.SetActive(true);
    }
}
