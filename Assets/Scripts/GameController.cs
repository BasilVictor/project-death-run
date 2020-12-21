using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Transform wallPrefab = null; //v

    [SerializeField]
    private Transform wallPrefab2 = null; //h

    [SerializeField]
    private Transform floorPrefab = null;

    void Start()
    {
        var maze = MazeGenerator.Generate(width, height);
        // for (int i = 0; i < 1; ++i)
        // {
        //     for (int j = 0; j < 1; ++j)
        //     {
        //         var position = new Vector3(-width / 2 + i, 0, -height / 2 + j);
        //         var topWall = Instantiate(wallPrefab, transform) as Transform;
        //         topWall.position = position + new Vector3(0, 0, size);
        //         var leftWall = Instantiate(wallPrefab2, transform) as Transform;
        //         leftWall.position = position + new Vector3(-size / 2, 0, 0);
        //         var rightWall = Instantiate(wallPrefab2, transform) as Transform;
        //         rightWall.position = position + new Vector3(+size / 2, 0, 0);
        //         var bottomWall = Instantiate(wallPrefab, transform) as Transform;
        //         bottomWall.position = position + new Vector3(0, 0, 0);
        //     }
        // }
        Draw(maze);
    }

    private void Draw(WallState[,] maze)
    {

        // var floor = Instantiate(floorPrefab, transform);
        // floor.localScale = new Vector3(width, 1, height);

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                var cell = maze[i, j];
                var position = new Vector3(-width / 2 + i, 0, -height / 2 + j);

                // var floor = Instantiate(floorPrefab, transform) as Transform;
                // floor.position = position;
                // floor.GetComponent<SpriteRenderer>().sortingLayerName="Background";

                if (cell.HasFlag(WallState.UP))
                {
                    var topWall = Instantiate(wallPrefab, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size);
                    // topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                }

                if (cell.HasFlag(WallState.LEFT))
                {
                    var leftWall = Instantiate(wallPrefab2, transform) as Transform;
                    leftWall.position = position + new Vector3(-size / 2, 0, 0);
                    // leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    // leftWall.eulerAngles = new Vector3(0, 0, 90);
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.RIGHT))
                    {
                        var rightWall = Instantiate(wallPrefab2, transform) as Transform;
                        rightWall.position = position + new Vector3(+size / 2, 0, 0);
                        // rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        // rightWall.eulerAngles = new Vector3(0, 0, 90);
                    }
                }

                if (j == 0)
                {
                    if (cell.HasFlag(WallState.DOWN))
                    {
                        var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                        bottomWall.position = position + new Vector3(0, 0, 0);
                        // bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                    }
                }
            }

        }

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
