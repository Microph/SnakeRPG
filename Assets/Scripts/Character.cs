using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    public CharacterInfoScriptableObject characterInfoScriptableObject;
    public CharacterStatus characterStatus;

    //Called by ResourceManager
    public void Setup(CharacterInfoScriptableObject characterInfoScriptableObject, CharacterStatus characterStatus)
    {
        //All entity subclass must call this first
        base.Setup(characterInfoScriptableObject);

        this.characterInfoScriptableObject = characterInfoScriptableObject;
        this.characterStatus = characterStatus;
        //Stats, etc...
    }
}

public enum CharacterStatus
{
    Player,
    Hero,
    Enemy
}