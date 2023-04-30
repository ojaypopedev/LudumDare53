using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HotDogCannon/CharacterColor", fileName = "New Character Color")]
public class CharacteracterColor : ScriptableObject
{
    public Color[] Colors;

    public Color GetRandom()=> Colors[Random.Range(0,Colors.Length)];   
}
