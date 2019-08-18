using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item Info Scriptable Object")]
public class ItemInfoScriptableObject : EntityScriptableObject
{
    public int hpModifier;
    public int shieldModifier;
    public int atkModifier;
}
