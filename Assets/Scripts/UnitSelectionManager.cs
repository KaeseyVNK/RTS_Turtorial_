using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }

    public List<GameObject> allUnitList = new List<GameObject>();
    public List<GameObject> unitSelected = new List<GameObject>();

    public LayerMask clickable;
    public LayerMask ground;
    public LayerMask constructable;

    public LayerMask attackable;
    public bool attackCursorVisible;

    public GameObject groundMarker;

    private Camera cam;

    public bool playedDuringThisDrag = false;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //If we are hitting a clickable object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                }
      
            }
            else //if we are NOT hitting a clickable object
            {
                if(Input.GetKey(KeyCode.LeftShift) == false)
                {
                    DeselectAll();
                }               
            }
        }


        if (Input.GetMouseButtonDown(1) && unitSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //If we are hitting a clickable object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = hit.point;

                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
            }

        }

        // Atack Target
        if ( unitSelected.Count > 0 && AtleastOneOffensiveUnit(unitSelected))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackable))
            {
                Debug.Log("Enemy Hovered with mouse");

                attackCursorVisible = true;

                if (Input.GetMouseButtonDown(1))
                {
                    Transform target = hit.transform;
                    foreach ( GameObject unit in unitSelected)
                    {
                        if ( unit.GetComponent<AttackController>()) 
                        {
                            unit.GetComponent<AttackController>().targetToAttack = target;
                        }
                    }
                }
            }
            else
            {
                attackCursorVisible = false;
            }
        }

        CursorSeclector();
    }

    private void CursorSeclector()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Selectable);
        }
        else if( ResourceManager.Instance.placementSystem.isSellMode )
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.SellCursor);
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackable) && unitSelected.Count > 0 && AtleastOneOffensiveUnit(unitSelected)) 
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Attackable);

        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, constructable) && unitSelected.Count > 0)
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.UnAvailable);
        }
         else if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground) && unitSelected.Count > 0)
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Walkable);
        }
        else
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.None);
        }
    }

    private bool AtleastOneOffensiveUnit(List<GameObject> unitSelected)
    {
        foreach (GameObject unit in unitSelected)
        {
            if (unit != null && unit.GetComponent<AttackController>())
            {
                return true;
            }
        }
        return false;
    }

    private void MultiSelect(GameObject unit)
    {
        if (unitSelected.Contains(unit) == false)
        {
            unitSelected.Add(unit);
            SelectUnit(unit, true);
        }
        else
        {
            SelectUnit(unit, false);
            unitSelected.Remove(unit);
        }
    }

    public void DeselectAll()
    {
        foreach(var unit in unitSelected)
        {
            SelectUnit(unit, false);
        }
        groundMarker.SetActive(false);
        unitSelected.Clear();
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();

        unitSelected.Add(unit);

        SelectUnit(unit, true);
    }

    private void SelectUnit(GameObject unit, bool isSelected)
    {
        TriggerSelectionIndicator(unit, isSelected);

        EnableUnitMovement(unit, isSelected);
    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
        unit.GetComponent<UnitMovement>().enabled = shouldMove;
    }

    private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
    {

        GameObject indicator = unit.transform.Find("Indicator").gameObject;

        if(!indicator.activeInHierarchy && playedDuringThisDrag == false)
        {
            SoundManager.Instance.PlayUnitSelectionSound();
            playedDuringThisDrag = true;
        }
        
        unit.transform.Find("Indicator").gameObject.SetActive(isVisible);
    }

    internal void DragSelect(GameObject unit)
    {
        if (unitSelected.Contains(unit) == false)
        {
            unitSelected.Add(unit);
            SelectUnit(unit, true);
        }
    }
}
