using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{
    public ItemInfoScriptableObject itemInfoScriptableObject;

    //Called by ResourceManager
    public void Setup(ItemInfoScriptableObject itemInfoScriptableObject)
    {
        //All entity subclass must call this first
        base.Setup(itemInfoScriptableObject);

        this.itemInfoScriptableObject = itemInfoScriptableObject;
        //Stats, etc...
    }
}
