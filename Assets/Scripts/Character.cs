using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : Entity
{
    //Stats views
    public Text currentHPText;
    public Text currentShieldText;
    public Text currentATKText;
    public Text currentCharacterSideText;

    //Current stats
    private CharacterSide _currentCharacterSide;
    private CharacterType _currentType;
    private int _currentHP;
    private int _currentShield;
    private int _currentATK;

    private CharacterInfoScriptableObject characterInfoScriptableObject;

    public CharacterSide CurrentCharacterSide
    {
        get => _currentCharacterSide;
        set
        {
            _currentCharacterSide = value;
            currentCharacterSideText.text = _currentCharacterSide == CharacterSide.Enemy ? "E" : _currentCharacterSide == CharacterSide.PlayerSnake ? "P" : "H";
        }
    }

    public CharacterType CurrentType
    {
        get => _currentType;
        set
        {
            _currentType = value;
            currentATKText.color = _currentType == CharacterType.Blue ? Color.blue : _currentType == CharacterType.Red ? Color.red : Color.green;
        }
    }

    public int CurrentHP
    {
        get => _currentHP;
        set
        {
            if(value < 0)
            {
                value = 0;
            }
            _currentHP = value;
            currentHPText.text = _currentHP.ToString();
        }
    }

    public int CurrentShield
    {
        get => _currentShield;
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            _currentShield = value;
            currentShieldText.text = _currentShield.ToString();
        }
    }

    public int CurrentATK
    {
        get => _currentATK;
        set
        {
            if (value < 1)
            {
                value = 1;
            }
            _currentATK = value;
            currentATKText.text = _currentATK.ToString();
        }
    }

    //Called by ResourceManager
    public void Setup(CharacterInfoScriptableObject characterInfoScriptableObject, CharacterSide characterSide)
    {
        //All entity subclass must call this first
        base.Setup(characterInfoScriptableObject);

        this.characterInfoScriptableObject = characterInfoScriptableObject;
        this.CurrentCharacterSide = characterSide;
        this.CurrentType = characterInfoScriptableObject.type;
        this.CurrentHP = characterInfoScriptableObject.hp;
        this.CurrentShield = characterInfoScriptableObject.shield;
        this.CurrentATK = characterInfoScriptableObject.atk;
    }
}

public enum CharacterSide
{
    PlayerSnake,
    Hero,
    Enemy
}