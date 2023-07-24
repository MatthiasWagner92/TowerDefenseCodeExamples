using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alt_Functions:MonoBehaviour
{
    [SerializeField] private static bool showVisual =true;

    public delegate void MaipulationFunction(Alt_Tile container);
    public delegate void MovementFunction(Alt_Tile container);

    static private MaipulationFunction _manipulationMethod;
    static private MovementFunction _movementMethod;
    public static MaipulationFunction GetManipulation(TileType t)
    {
        
        switch (t)
        {
            case TileType.AddAir:
                _manipulationMethod = AddAir;
                break;
            case TileType.AddFire:
                _manipulationMethod = AddFire;
                break;
            case TileType.AddStone:
                _manipulationMethod = AddStone;
                break;
            case TileType.AddWater:
                _manipulationMethod = AddWater;
                break;
            case TileType.MoveByAir:
                _manipulationMethod = MoveByAir;
                break;
            case TileType.MoveByFire:
                _manipulationMethod = MoveByFire;
                break;
            case TileType.MoveByStone:
                _manipulationMethod = MoveByStone;
                break;
            case TileType.MoveByWater:
                _manipulationMethod = MoveByWater;
                break;
            case TileType.MoveSecondBullet:
                _manipulationMethod = MoveSecondBullet;
                break;
            case TileType.AddDamageUpper:
                _manipulationMethod = AddDamageUpper;
                break;
            case TileType.AddDamageLower:
                _manipulationMethod = AddDamageLower;
                break;
            case TileType.StackEnd:
                _manipulationMethod = StackEnd;
                break;
            case TileType.Spawner:
                _manipulationMethod = Spawn;
                break;
            default:
                _manipulationMethod = DoNothing;
                break;
        }
        return _manipulationMethod;

    }
    public static MovementFunction GetMovement()
    {
        return basicMovement;

    }



    #region ManipulationFunctions
    static void AddAir(Alt_Tile container)
    {


        container.Content?.changeBulletType(BulletType.AIR);
    }
    static void AddFire(Alt_Tile container)
    {
        container.Content?.changeBulletType(BulletType.FIRE);
    }
    static void AddWater(Alt_Tile container)
    {
        container.Content?.changeBulletType(BulletType.WATER);
    }
    static void AddStone(Alt_Tile container)
    {
        container.Content?.changeBulletType(BulletType.STONE);
    }

    static void AddDamageUpper(Alt_Tile container)
    {
        if (container.Content == null) return;
        container.Content.upperDamage+=1;
    }
    static void AddDamageLower(Alt_Tile container)
    {
        if (container.Content == null) return;
        container.Content.lowerDamage+=1;
    }

    static void MoveByStone(Alt_Tile container)
    {
        if (container.Content == null ||!container.Content.IsType(BulletType.STONE)) return;
        
        Alt_MiniGameManager.Instance.GetTile(container.getXValue(), container.getYValue()+1).Content = container.Content;
        container.Content.currentTile = Alt_MiniGameManager.Instance.GetTile(container.getXValue(), container.getYValue()+1);
        container.Content = null;

    }

    static void MoveByAir(Alt_Tile container)
    {
        if (container.Content == null || !container.Content.IsType(BulletType.AIR)) return;

        Alt_MiniGameManager.Instance.GetTile(container.getXValue(), container.getYValue() + 1).Content = container.Content;
        container.Content.currentTile = Alt_MiniGameManager.Instance.GetTile(container.getXValue(), container.getYValue() + 1);
        container.Content = null;
    }

    static void MoveByWater(Alt_Tile container)
    {
        if (container.Content == null || !container.Content.IsType(BulletType.WATER)) return;

        Alt_MiniGameManager.Instance.GetTile(container.getXValue(), container.getYValue() + 1).Content = container.Content;
        container.Content.currentTile = Alt_MiniGameManager.Instance.GetTile(container.getXValue(), container.getYValue() + 1);
        container.Content = null;
    }

    static void MoveByFire(Alt_Tile container)
    {
        if (container.Content == null || !container.Content.IsType(BulletType.FIRE)) return;

        Alt_MiniGameManager.Instance.GetTile(container.getXValue(), container.getYValue() + 1).Content = container.Content;
        container.Content.currentTile = Alt_MiniGameManager.Instance.GetTile(container.getXValue(), container.getYValue() + 1);
        container.Content = null;
    }

    static void MoveSecondBullet(Alt_Tile container)
    {
        if (container.Content == null) return;
        if (container.moveSecondBool)
        {
            Alt_MiniGameManager.Instance.GetTile(container.getXValue(), container.getYValue() + 1).Content = container.Content;
            container.Content.currentTile = Alt_MiniGameManager.Instance.GetTile(container.getXValue(), container.getYValue() + 1);
            container.Content = null;
        }
        container.moveSecondBool = !container.moveSecondBool;
    }

    static void StackEnd(Alt_Tile container)
    {
        if (container.Content == null) return;
        Debug.Log("Add " + container.Content + " to BulletList");
        PlayerInformations.Instance.AddBullet(container.Content);
        
        container.Content.currentTile.Content = null;

        
    }

    static void Spawn(Alt_Tile container)
    {

        GameObject bullet = Resources.Load<GameObject>("BulletPrefab");
        
        if (bullet != null)
        {
            Alt_Bullet spawnedBullet = new Alt_Bullet();
            spawnedBullet.currentTile = container;
            container.Content = spawnedBullet;

            if (GameManager.Instance.GameState == GameState.MunitionFactory) 
            {
                GameObject spawnedVisual = Instantiate(bullet, container.myRepresentation.transform.position, Quaternion.identity);
                spawnedVisual.AddComponent<Alt_BulletVisual>().bulletData = spawnedBullet;
                Alt_MiniGameManager.Instance.producedBulletsRepresentation.Add(spawnedVisual);
            }
            
        }
        else
        {
            Debug.LogError("Failed to load Bullet prefab from Resources folder.");
        }
    }

    static void DoNothing(Alt_Tile container)
    {
        Debug.Log("DoNothing");
    }

    #endregion



    #region Movement
    static void basicMovement(Alt_Tile container)
    {
        if (container.Content == null) return;
        if (container.GetNextTile() == null) container.Content = null;
        else container.GetNextTile().ContentNextTurn = container.Content;
    }

    #endregion
}
