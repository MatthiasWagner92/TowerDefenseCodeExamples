using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameState GameState;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        changeState(GameState.GenerateGrid);
    }

    public void changeState(GameState newState)
    {
        GameState = newState;
        Debug.Log("GameManager change state" + newState);
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.HeroTime:
                UIManager.Instance.showInfoWindow();
                UIManager.Instance.OnBuildState();
                break;
            case GameState.SpawnEnemies:
                UIManager.Instance.showInfoWindow();
                GridManager.Instance.clearAllPaths();
                UnitManager.Instance.SpawnEnemys();
                UIManager.Instance.OnFightState();
                break;
            case GameState.HeroStatistics:
                break;
            case GameState.TowerFactory:
                UIManager.Instance.OnTowerFactoryState();
                FactoryManager.Instance.startFactory();
                break;
            case GameState.MunitionFactory:
                
                UIManager.Instance.OnMunitionFactoryState();
                
                //Alt_MiniGameManager.Instance.
                    
                break;
            case GameState.EnemiesTurn:
                break;

        }
    }

    public void GoSpawnTime()
    {
        changeState(GameState.SpawnEnemies);
    }
    public void GoHeroTime()
    {
        changeState(GameState.HeroTime);
    }

}
public enum GameState
{
    GenerateGrid = 0,
    HeroTime = 1,
    SpawnEnemies = 2,
    HeroStatistics = 3,
    EnemiesTurn = 4,
    TowerFactory = 5,
    MunitionFactory = 6
}