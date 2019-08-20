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
    public CharacterInfoScriptableObject[] heroCharacters;
    public CharacterInfoScriptableObject[] enemyCharacters;
    public ItemInfoScriptableObject[] items;

    private void Awake()
    {
        _instance = this;
    }

    public Character GeneratePlayerSnake(bool isHead, Tuple<int, int> atIndex)
    {
        Character newCharacter = GameObject.Instantiate(characterPrefab);
        newCharacter.Setup(heroCharacters[Random.Range(0, heroCharacters.Length)], CharacterSide.PlayerSnake);

        //Add PlayerSnake Component
        PlayerSnakeComponent playerSnakeComponent = newCharacter.gameObject.AddComponent<PlayerSnakeComponent>();
        playerSnakeComponent.Setup(isHead, atIndex);
        return newCharacter;
    }

    public Character GenerateEnemy()
    {
        Character newCharacter = GameObject.Instantiate(characterPrefab);
        newCharacter.Setup(enemyCharacters[Random.Range(0, enemyCharacters.Length)], CharacterSide.Enemy);
        return newCharacter;
    }

    public Character GenerateHero()
    {
        Character newCharacter = GameObject.Instantiate(characterPrefab);
        newCharacter.Setup(heroCharacters[Random.Range(0, heroCharacters.Length)], CharacterSide.Hero);
        return newCharacter;
    }

    public Item GenerateItem()
    {
        Item newItem = GameObject.Instantiate(itemPrefab);
        newItem.Setup(items[Random.Range(0, items.Length)]);
        return newItem;
    }
}
