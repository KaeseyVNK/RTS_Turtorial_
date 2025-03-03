using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerManager : MonoBehaviour
{
    public static PowerManager Instance{get; set;}

    public int totalPower; //total power produced
    public int powerUsage; //total power consumed

    public AudioClip powerInsufficientClip;
    public AudioClip powerAddedClip;

    private AudioSource powerAudioSource;

    [SerializeField] private Image sliderFill;
    [SerializeField] private Slider powerSlider;
    [SerializeField] private TextMeshProUGUI powerText;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
        Destroy(gameObject);
        }else
        {
            Instance = this;
        }

        powerAudioSource = gameObject.AddComponent<AudioSource>();
    }

    public void AddPower(int amount)
    {
        PlayPowerAddedSound();
        totalPower += amount;
        UpdatePowerUI();
    }

    public void ConsumePower(int amount)
    {
        powerUsage += amount;
        UpdatePowerUI();
        if(isInsufficientPower())
        {
            PlayPowerInsufficientSound();
        }
    }
    
    public void RemovePower(int amount)
    {
        totalPower -= amount;
        UpdatePowerUI();
        if(isInsufficientPower())
        {
            PlayPowerInsufficientSound();
        }
    }

    public void ReleasePower(int amount)
    {
        powerUsage -= amount;
        UpdatePowerUI();
       
    }

    private bool isInsufficientPower(){
        return totalPower - powerUsage <= 0;
    }


    private void UpdatePowerUI()
    {
        int availablePower = totalPower - powerUsage;
        if(availablePower > 0)
        {
            sliderFill.gameObject.SetActive(true);
        }
        else
        {
            sliderFill.gameObject.SetActive(false);
        }

        if(powerSlider != null)
        {
            powerSlider.maxValue = totalPower;
            powerSlider.value = totalPower - powerUsage;
        }

        if(powerText != null)
        {
            powerText.text = $"{totalPower - powerUsage}/{totalPower}";
        }
        
    }
    
    public int CaculateAvailablePower(){
        return totalPower - powerUsage;
    }
    
    public void PlayPowerAddedSound()
    {
        powerAudioSource.PlayOneShot(powerAddedClip);
    }

    public void PlayPowerInsufficientSound()
    {
        Debug.Log("PlayPowerInsufficientSound");
        powerAudioSource.PlayOneShot(powerInsufficientClip);
    }
}
