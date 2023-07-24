using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    List<Tile3D> openList;
    List<Tile3D> closedList;
    public List<Tile3D> resultPath;
    public List<Tile3D> knownPath;

    private bool saveCondition = true;
    public static Pathfinding Instance;
    private void Awake()
    {
        Instance = this;
    }

    private List<Tile3D> AppendTile3D(List<Tile3D> basis,List<Tile3D> tail)
    {
        foreach (var elem in tail)
        {
            basis.Add(elem);
        }
        return basis;
    }
    public List<Tile3D> startSearchPath(Tile3D startNode, Tile3D targetNode)
    {
        //checks if there is already a known path to the target node stored in the startNode.pathToTarget list. If such a path exists, it is returned immediately, avoiding unnecessary calculations.
        if (startNode.pathToTarget.Count > 0) return startNode.pathToTarget;

        openList = new List<Tile3D>();
        openList.Add(startNode);
        closedList = new List<Tile3D>();
        Tile3D current = targetNode;
        //The main loop runs as long as there are nodes in the openList to be evaluated.
        while (openList.Count > 0)
        {
            
            current = GetLowestFcost(openList);
            openList.Remove(current);
            closedList.Add(current);
            
            if(current.pathToTarget.Count>0)
            {
                saveCondition = false;
                knownPath = current.pathToTarget;
                return AppendTile3D(RetracePath(startNode, current), knownPath);

            }

            if (current == targetNode)
            {

                saveCondition = true;
                return RetracePath(startNode, targetNode);
            }
            //The method iterates through the neighbors of the current node and calculates their G and H costs. If a neighbor is already in the closedList, is blocked by a wall, or is unwalkable, it is skipped.
            foreach (Tile3D neighbour in current.getNeighbours())
            {
                if (closedList.Contains(neighbour) || (neighbour.GetWall() != null)||!neighbour._isWalkable)
                {

                    continue;
                }

                int newMovementToNeighbour = current.getG_Costs() + pathLength(current, neighbour);
                if (newMovementToNeighbour < current.getG_Costs() || !openList.Contains(neighbour))
                    {
                    neighbour.setG_Costs(newMovementToNeighbour);
                    neighbour.setH_Costs(pathLength(neighbour, targetNode));
                    neighbour.setParent(current);
                    
                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
            
        }
        saveCondition = true;
        return AppendTile3D(RetracePath(startNode, targetNode) ,knownPath);
    }

    //returns the Result
    private List<Tile3D> RetracePath(Tile3D startTile, Tile3D endTile)
    {
        List<Tile3D> path = new List<Tile3D>();
        Tile3D current = endTile;
        
        while(current!= startTile)
        {

            path.Add(current);

            
            current = current.getParent();
            
            
            
        }
        
        path.Reverse();
        if(saveCondition) StoreSuccessors(path);

        return path;
    }

    //Optimization. Gets the Path from any Enemy that also walks on that Tile.
    public void StoreSuccessors(List<Tile3D> tileList)
    {

        int endIndex = tileList.Count - 1;

        for (int i = 0; i < endIndex; i++)
        {
            Tile3D currentTile = tileList[i];

            for (int j = i + 1; j <= endIndex; j++)
            {
                currentTile.pathToTarget.Add(tileList[j]);
            }
        }
    }

    //(At the beginning of each iteration) 
    //the node with the lowest F-cost (combination of G-cost and H-cost) from the openList is selected as the current node.
    public Tile3D GetLowestFcost(List<Tile3D> searchList)
    {
        
        int oldf_Costs = searchList[0].getF_Costs();
        Tile3D winner = searchList[0];
        foreach (Tile3D elem in searchList)
        {

            int newf_Costs = elem.getF_Costs();
            if(newf_Costs < oldf_Costs)
            {
                oldf_Costs = newf_Costs;
                winner = elem;
            }
        }

        return winner;
    }

    private int pathLength(Tile3D from, Tile3D to)
    {

        int result = 0;
        int xDiff = Mathf.Abs(from.GetxValueOfName() - to.GetxValueOfName()); 
        int yDiff = Mathf.Abs(from.GetyValueOfName() - to.GetyValueOfName());
        if ( xDiff > yDiff)
        {
            result = yDiff * 14 + (xDiff - yDiff) * 10;
        }
        else 
        {
            result = xDiff * 14 + (yDiff - xDiff) * 10;
        }

        return result;
    }



}
