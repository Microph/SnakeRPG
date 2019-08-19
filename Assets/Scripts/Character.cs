using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    public CharacterInfoScriptableObject characterInfoScriptableObject;
    public CharacterSide characterSide;

    //Called by ResourceManager
    public void Setup(CharacterInfoScriptableObject characterInfoScriptableObject, CharacterSide characterSide)
    {
        //All entity subclass must call this first
        base.Setup(characterInfoScriptableObject);

        this.characterInfoScriptableObject = characterInfoScriptableObject;
        this.characterSide = characterSide;
        //Stats, etc...
    }
}

public enum CharacterSide
{
    PlayerSnake,
    Hero,
    Enemy
}