using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager _instance;
    public static ResourceManager Instance { get { return _instance; } }

    public Character characterPrefab;
    public Item itemPrefab;
    public CharacterInfoScriptableObject[] playerSideCharacters;
    public CharacterInfoScriptableObject[] enemySideCharacters;
    public ItemInfoScriptableObject[] items;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public Character GeneratePlayerSnake(bool isHead)
    {
        Character newCharacter = GameObject.Instantiate(characterPrefab);
        newCharacter.Setup(playerSideCharacters[Random.Range(0, playerSideCharacters.Length)], CharacterStatus.Player);

        //Add PlayerSnake Component
        PlayerSnake playerSnakeComponent = newCharacter.gameObject.AddComponent<PlayerSnake>();
        playerSnakeComponent.Setup(isHead, null);
        return newCharacter;
    }

    public Character GenerateEnemy()
    {
        Character newCharacter = GameObject.Instantiate(characterPrefab);
        newCharacter.Setup(enemySideCharacters[Random.Range(0, enemySideCharacters.Length)], CharacterStatus.Enemy);
        return newCharacter;
    }

    public Character GenerateHero()
    {
        Character newCharacter = GameObject.Instantiate(characterPrefab);
        newCharacter.Setup(playerSideCharacters[Random.Range(0, playerSideCharacters.Length)], CharacterStatus.Hero);
        return newCharacter;
    }

    public Item GenerateItem()
    {
        Item newItem = GameObject.Instantiate(itemPrefab);
        newItem.Setup(items[Random.Range(0, items.Length)]);
        return newItem;
    }
}
