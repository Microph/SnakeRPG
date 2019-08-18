using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    public CharacterInfoScriptableObject characterScriptableObject;

    //Called by ResourceManager
    public virtual void Setup(CharacterInfoScriptableObject characterInfoScriptableObject)
    {
        this.characterScriptableObject = characterInfoScriptableObject;
        spriteRenderer.sprite = characterInfoScriptableObject.sprite;
        //Stats ....
    }
}
