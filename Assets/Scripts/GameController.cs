﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[RequireComponent(typeof(MazeGenerator))]

public class GameController : MonoBehaviour
{

    private MazeGenerator mazeGenerator;

    [SerializeField]
    [Range(1, 50)]
    private int width = 10;

    [SerializeField]
    [Range(1, 50)]
    private int height = 10;

    [SerializeField]
    private float size = 1f;

    [SerializeField]
    private int noOfPlayers = 2;

    [SerializeField]
    private Transform horizontalWall = null; 

    [SerializeField]
    private Transform verticalWall = null; 

    [SerializeField]
    private Transform floorPrefab = null;

    [SerializeField]
    private Transform playerPrefab = null;

    [SerializeField]
    private Transform objPrefab = null;

    void Start()
    {
        var maze = MazeGenerator.Generate(width, height);
        //Printing distances
        /*var distmat = maze.getDistanceMatrix();
        string yolo = "";
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                yolo += "("+i+","+j+")\n";
                for(int k=0;k<width;k++)
                {
                    for(int l=0;l<height;l++)
                    {
                        yolo += distmat[i,j].getDistances()[k,l] +"\t";
                    }
                    yolo += "\n";
                }
                Debug.Log(yolo);
                yolo = "";
            }
        }*/
        Draw(maze.getMaze());
        List<Position> spawn = MazeGenerator.Spawn(width, height, maze.getDistanceMatrix(), noOfPlayers);
        SpawnPlayers(spawn);
        SpawnObjective(spawn);
    }

    private void Draw(WallState[,] maze)
    {

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                var cell = maze[i,j];
                var position = new Vector3(-width / 2 + i, 0, height / 2 - j);

                var floor = Instantiate(floorPrefab, transform) as Transform;
                floor.position = position + new Vector3(0, 0, size/2);
                floor.GetComponent<SpriteRenderer>().sortingLayerName="Background";

                if (cell.HasFlag(WallState.UP))
                {
                    var topWall = Instantiate(horizontalWall, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size);
                    topWall.name = "TOP: " + i + ", " + j;
                }

                if (cell.HasFlag(WallState.LEFT))
                {
                    var leftWall = Instantiate(verticalWall, transform) as Transform;
                    leftWall.position = position + new Vector3(-size / 2, 0, 0);
                    leftWall.name = "LEFT: " + i + ", " + j;
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.RIGHT))
                    {
                        var rightWall = Instantiate(verticalWall, transform) as Transform;
                        rightWall.position = position + new Vector3(+size / 2, 0, 0);
                        rightWall.name = "RIGHT: " + i + ", " + j;
                    }
                }

                if (j == height - 1)
                {
                    if (cell.HasFlag(WallState.DOWN))
                    {
                        var bottomWall = Instantiate(horizontalWall, transform) as Transform;
                        bottomWall.position = position + new Vector3(0, 0, 0);
                        bottomWall.name = "BOTTOM: " + i + ", " + j;
                    }
                }
            }

        }

    }

    private void SpawnPlayers(List<Position> points)
    {
        var position = new Vector3(-width / 2, 0, height / 2);
        Position pos;
        for (int i = 0; i < points.Count - 1; i++)
        {
            pos = points[i];
            var spaw = Instantiate(playerPrefab, transform) as Transform;
            spaw.position = position + new Vector3(pos.getX(), 0, -pos.getY() + size / 2);
            spaw.name = pos.getX() + "," + pos.getY();
        }
    }

    private void SpawnObjective(List<Position> points)
    {
        var position = new Vector3(-width / 2, 0, height / 2);
        Position pos = points[points.Count - 1];
        var spawObj = Instantiate(objPrefab, transform) as Transform;
        spawObj.position = position + new Vector3(pos.getX(), 0, -pos.getY() + size / 2);
        spawObj.name = pos.getX() + "," + pos.getY();
    }

    void Update()
    {
        
    }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [RequireComponent(typeof(MazeGenerator))]

// public class GameController : MonoBehaviour
// {

//     private MazeGenerator mazeGenerator;

//     [SerializeField]
//     [Range(1, 50)]
//     private int width = 10;

//     [SerializeField]
//     [Range(1, 50)]
//     private int height = 10;

//     [SerializeField]
//     private float size = 1f;

//     [SerializeField]
//     private Transform wallPrefab = null;

//     [SerializeField]
//     private Transform floorPrefab = null;

//     void Start()
//     {
//         var maze = MazeGenerator.Generate(width, height);
//         Draw(maze);
//     }

//     private void Draw(WallState[,] maze)
//     {

//         // var floor = Instantiate(floorPrefab, transform);
//         // floor.localScale = new Vector3(width, 1, height);

//         for (int i = 0; i < width; ++i)
//         {
//             for (int j = 0; j < height; ++j)
//             {
//                 var cell = maze[i, j];
//                 var position = new Vector3(-width / 2 + i, -height / 2 + j, 0);

//                 var floor = Instantiate(floorPrefab, transform) as Transform;
//                 floor.position = position;
//                 floor.GetComponent<SpriteRenderer>().sortingLayerName="Background";

//                 if (cell.HasFlag(WallState.UP))
//                 {
//                     var topWall = Instantiate(wallPrefab, transform) as Transform;
//                     topWall.position = position + new Vector3(0, size / 2, 0);
//                     // topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
//                 }

//                 if (cell.HasFlag(WallState.LEFT))
//                 {
//                     var leftWall = Instantiate(wallPrefab, transform) as Transform;
//                     leftWall.position = position + new Vector3(-size / 2, 0, 0);
//                     // leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
//                     leftWall.eulerAngles = new Vector3(0, 0, 90);
//                 }

//                 if (i == width - 1)
//                 {
//                     if (cell.HasFlag(WallState.RIGHT))
//                     {
//                         var rightWall = Instantiate(wallPrefab, transform) as Transform;
//                         rightWall.position = position + new Vector3(+size / 2, 0, 0);
//                         // rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
//                         rightWall.eulerAngles = new Vector3(0, 0, 90);
//                     }
//                 }

//                 if (j == 0)
//                 {
//                     if (cell.HasFlag(WallState.DOWN))
//                     {
//                         var bottomWall = Instantiate(wallPrefab, transform) as Transform;
//                         bottomWall.position = position + new Vector3(0, -size / 2, 0);
//                         // bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
//                     }
//                 }
//             }

//         }

//     }

//     void Update()
//     {
        
//     }
// }
