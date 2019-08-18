using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IEntity
{
    public CharacterInfoScriptableObject characterScriptableObject;

    //Dynamic
    public SpriteRenderer spriteRenderer;
    public FacingDirection facingDirection;

    //Called by ResourceManager
    public virtual void Setup(CharacterInfoScriptableObject characterInfoScriptableObject)
    {
        this.characterScriptableObject = characterInfoScriptableObject;
        spriteRenderer.sprite = characterInfoScriptableObject.sprite;
        //Stats ....
    }
}
