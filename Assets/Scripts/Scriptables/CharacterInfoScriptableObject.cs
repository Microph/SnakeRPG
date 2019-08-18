using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Red,
    Green,
    Blue
}

[CreateAssetMenu(fileName = "New Character", menuName = "Character Info Scriptable Object")]
public class CharacterInfoScriptableObject : EntityScriptableObject
{
    public int hp;
    public int shield;
    public int atk;
    public CharacterType type;
}