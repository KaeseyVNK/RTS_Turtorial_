using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance{get; set;}

    [Header("Units")]
    private AudioSource infantryAttackChannel; 
    public AudioClip infantryAttackClip;

    [Header("Buildings")]
    private AudioSource destructionBuildingChannel;
    private AudioSource[] constructionBuildingChannelPool;  
    private AudioSource extraBuildingChannel;

    [Header("Units")]

    public int poolSize = 5;
    private int unitCurrentPoolIndex = 0;
    private int constructionCurrentPoolIndex = 0;
    private AudioSource[] unitVoiceChannelPool;
    public AudioClip []unitSelectionSounds;
    public AudioClip []unitCommandSounds;

    [Header("Sounds")]
    public AudioClip sellingSound;
    public AudioClip buildingConstructionSound;
    public AudioClip buildingDestructionSound;


    private void Awake(){
        if(Instance != null && Instance != this ){
            Destroy(gameObject);
        }
        else 
        {
            Instance = this;
        }

        infantryAttackChannel = gameObject.AddComponent<AudioSource>();

        destructionBuildingChannel = gameObject.AddComponent<AudioSource>();

        constructionBuildingChannelPool = new AudioSource[3];

        for(int i = 0; i < poolSize; i++)
        {
            constructionBuildingChannelPool[i] = gameObject.AddComponent<AudioSource>();
        }

        extraBuildingChannel = gameObject.AddComponent<AudioSource>();

        unitVoiceChannelPool = new AudioSource[poolSize];

        // Create a pool of audio sources with a pool size
        for(int i = 0; i < poolSize; i++)
        {
            unitVoiceChannelPool[i] = gameObject.AddComponent<AudioSource>();
        }

    }

    public void PlayInfantryAttackSound()
    {
        if( infantryAttackChannel.isPlaying == false)
        {
            infantryAttackChannel.PlayOneShot(infantryAttackClip);
        }
    }


    public void PlayBuildingSellingSound()
    {
        if( extraBuildingChannel.isPlaying == false)
        {
            extraBuildingChannel.PlayOneShot(sellingSound);
        }
    }

    public void PlayBuildingConstructionSound()
    {
        constructionBuildingChannelPool[constructionCurrentPoolIndex].PlayOneShot(buildingConstructionSound);

        constructionCurrentPoolIndex = (constructionCurrentPoolIndex + 1) % poolSize;
    }

    public void PlayBuildingDestructionSound()
    {

        if( destructionBuildingChannel.isPlaying == false)
        {
            destructionBuildingChannel.PlayOneShot(buildingDestructionSound);
        }
    }

    public void PlayUnitSelectionSound()
    {
        AudioClip randomSound = unitSelectionSounds[Random.Range(0, unitSelectionSounds.Length)];

        unitVoiceChannelPool[unitCurrentPoolIndex].PlayOneShot(randomSound);

        unitCurrentPoolIndex = (unitCurrentPoolIndex + 1) % poolSize;
    }

    public void PlayUnitCommandSound()
    {

        AudioClip randomSound = unitCommandSounds[Random.Range(0, unitCommandSounds.Length)];

        unitVoiceChannelPool[unitCurrentPoolIndex].PlayOneShot(randomSound);

        unitCurrentPoolIndex = (unitCurrentPoolIndex + 1) % poolSize;
        
    }
}
