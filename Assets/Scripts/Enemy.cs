using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

    internal void TakeDamge(int damageToInflict)
    {
        health -= damageToInflict;
    }
}
