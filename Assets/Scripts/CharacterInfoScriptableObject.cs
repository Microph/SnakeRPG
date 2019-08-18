using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Alliance
{
    Player,
    Enemy
}

public enum CharacterType
{
    Red,
    Green,
    Blue
}

[CreateAssetMenu(fileName = "New Character", menuName = "Character Info Scriptable Object")]
public class CharacterInfoScriptableObject : ScriptableObject
{
    public Sprite sprite;
    public Alliance alliance;
    public int hp;
    public int shield;
    public int atk;
    public CharacterType type;
}