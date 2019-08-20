using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : Entity
{
    //Stats views
    public Text currentHPText;
    public Text currentShieldText;
    public Text currentATKText;
    
    public ItemInfoScriptableObject itemInfoScriptableObject;

    //Called by ResourceManager
    public void Setup(ItemInfoScriptableObject itemInfoScriptableObject)
    {
        //All entity subclass must call this first
        base.Setup(itemInfoScriptableObject);

        this.itemInfoScriptableObject = itemInfoScriptableObject;
        currentHPText.text = itemInfoScriptableObject.hpModifier.ToString();
        currentShieldText.text = itemInfoScriptableObject.shieldModifier.ToString();
        currentATKText.text = itemInfoScriptableObject.atkModifier.ToString();
    }
}
