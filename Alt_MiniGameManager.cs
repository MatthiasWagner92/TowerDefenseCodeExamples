using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Alt_MiniGameManager : MonoBehaviour
{
    [SerializeField] public float tickSpeed;
    Coroutine ticker = null;
    public int tickCount = 0;
    private bool isPaused = true;
    private List<GameObject> bulletList;
    private List<GameObject> mySpawner;
    private List<GameObject> myTileHolders;
    public Alt_Tile[,] tileList;

    
    public static Alt_MiniGameManager Instance;


    public List<GameObject> producedBulletsRepresentation;
    private bool tickToggle = false;

    private void Awake()
    {
        Instance = this;
        bulletList = new List<GameObject>();
        mySpawner = new List<GameObject>();
        myTileHolders = new List<GameObject>();
        
        producedBulletsRepresentation = new List<GameObject>();
    }


    private IEnumerator Tick()
    {
        WaitForSeconds wait = new WaitForSeconds(tickSpeed);

        while (true)
        {
            Debug.Log("Tick");
            tickCount++;

            OnTick();

            yield return wait;
        }
    }
    private void OnTick()
    {
        foreach (Alt_Tile tileItem in tileList)
        {
            if (tickToggle)
            { 
                tileItem.useManipulationFunction();
            }
            else {
                tileItem.useMovementFunction();       
            }
        }
        if (!tickToggle) 
        {
            foreach (Alt_Tile tileItem in tileList)
            {
                tileItem.Content = tileItem.ContentNextTurn;
                tileItem.ContentNextTurn = null;
                if(tileItem.Content !=null) tileItem.Content.currentTile = tileItem;
            }
        }
        tickToggle = !tickToggle;
    }

    public void OnPauseButton()
    {
        StopCoroutine(ticker);
        isPaused = true;

    }
    public void OnPlayButton()
    {        
        if (isPaused)
        {
            ticker = StartCoroutine(Tick());
            isPaused = false;
        }
    }
    public void OnStopButton()
    {
        if (!isPaused)
        {
            OnPauseButton();
        }

        foreach (GameObject s in mySpawner)
        {
            s.GetComponent<Spawner>().SetTickToggle(true);
            tickCount = 0;
        }
        foreach (GameObject th in myTileHolders)
        {
            th.GetComponent<TileHolder>().SetTickToggle(true);
        }

        foreach (GameObject bullet in bulletList)
        {
            Destroy(bullet.gameObject);
        }
    }

    


    public void AddSpawner(GameObject s)
    {
        mySpawner.Add(s);
    }
    public void AddTileHolder(GameObject th)
    {
        myTileHolders.Add(th);
    }


    public Alt_Tile GetTile(int x,int y)
    {
        if(x <0 || y<0||x>tileList.GetLength(0)-1|| y > tileList.GetLength(1) - 1)
        {
            Debug.Log("Wrong index in Alt_MiniGameManager Alt_Tile GetTile(int x,int y)");
            return null;
        }
        return tileList[x, y];
    }
}
