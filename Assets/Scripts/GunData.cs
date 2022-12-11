using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData")]
public class GunData : ScriptableObject
{
    [Header("Gameplay parameters")]
    public int damage;
    public float fireRate;
    public int ammoBeforeReload;

    [Header("Sounds")]
    public AudioClip fireSoundEffect;
    public AudioClip reloadSoundEffect;

    [Header("Art")]
    public Sprite gunImage;
    public Sprite ammoImage;
    




}    
