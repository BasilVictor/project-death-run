﻿/*
WallState::
0000 -> NO WALLS
1111 -> LEFT, RIGHT, UP, DOWN

MAZE::
[[(0,0), (1,0), (2,0)]
 [(0,1), (1,1), (2,1)]]
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Flags]
public enum WallState
{
    LEFT = 1, // 0001
    RIGHT = 2, // 0010
    UP = 4, // 0100
    DOWN = 8, // 1000
    VISITED = 128, // 1000 0000
}

public class Position
{
    private int X, Y;

    public Position(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }

    public Position(string position)
    {
        this.X = int.Parse(position.Split(',')[0]);
        this.Y = int.Parse(position.Split(',')[1]);
    }

    public int getX()
    {
        return X;
    }

    public int getY()
    {
        return Y;
    }

    public string toString()
    {
        return this.getX() + "," + this.getY();
    }
}

public class Neighbour
{
    private Position Pos;
    private WallState SharedWall;

    public Neighbour(Position Pos, WallState SharedWall)
    {
        this.Pos = Pos;
        this.SharedWall = SharedWall;
    }

    public Position getPosition()
    {
        return Pos;
    }

    public WallState getSharedWall()
    {
        return SharedWall;
    }
}

public class Distance
{
    public int[,] distances;

    public Distance(int height, int width, int x, int y)
    {
        distances = new int[width, height];
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                if(i==x && j==y)
                    distances[i,j] = 0;
                else
                    distances[i,j] = -1;
            }
        }
    }

    public int[,] getDistances()
    {
        return distances;
    }
}

public class CoordinateDistance : IComparable<CoordinateDistance>
{
    public int distance;
    public Position position;

    public CoordinateDistance(int distance, int x, int y)
    {
        this.distance = distance;
        this.position = new Position(x, y);
    }

    public int CompareTo(CoordinateDistance obj)
    {
        return distance.CompareTo(obj.distance);
    }
}

public class Maze
{
    private WallState[,] maze;
    private Distance[,] distanceMatrix;

    public Maze(WallState[,] maze, Distance[,] distanceMatrix)
    {
        this.maze = maze;
        this.distanceMatrix = distanceMatrix;
    }

    public WallState[,] getMaze()
    {
        return maze;
    }

    public Distance[,] getDistanceMatrix()
    {
        return distanceMatrix;
    }
}

public class MazeGenerator : MonoBehaviour
{
    private static WallState GetOppositeWall(WallState wall)
    {
        switch (wall)
        {
            case WallState.RIGHT: return WallState.LEFT;
            case WallState.LEFT: return WallState.RIGHT;
            case WallState.UP: return WallState.DOWN;
            case WallState.DOWN: return WallState.UP;
            default: return WallState.LEFT;
        }
    }

    private static Maze ApplyRecursiveBacktracker(WallState[,] maze, Distance[,] distanceMatrix, int width, int height)
    {
        System.Random random = new System.Random(/*seed*/);
        Stack<Position> positionStack = new Stack<Position>();
        Position position = new Position(random.Next(0, width), random.Next(0, height));

        positionStack.Push(position);
        maze[position.getX(), position.getY()] |= WallState.VISITED;

        while(positionStack.Count > 0)
        {
            Position current = positionStack.Pop();
            List<Neighbour> neighbours = GetUnvisitedNeighbours(current, maze, width, height);

            if(neighbours.Count > 0)
            {
                positionStack.Push(current);

                int randIndex = random.Next(0, neighbours.Count);
                Neighbour randomNeighbour = neighbours[randIndex];

                Position neighbourPosition = randomNeighbour.getPosition();
                maze[current.getX(), current.getY()] &= ~randomNeighbour.getSharedWall();
                maze[neighbourPosition.getX(), neighbourPosition.getY()] &= ~GetOppositeWall(randomNeighbour.getSharedWall());
                maze[neighbourPosition.getX(), neighbourPosition.getY()] |= WallState.VISITED;

                positionStack.Push(neighbourPosition);
                calculateDistance(distanceMatrix[neighbourPosition.getX(), neighbourPosition.getY()].getDistances(), distanceMatrix[current.getX(), current.getY()].getDistances(), width, height);
            }
            else if(positionStack.Count > 0)
            {
                Position previous = positionStack.Peek();
                calculateDistance(distanceMatrix[previous.getX(), previous.getY()].getDistances(), distanceMatrix[current.getX(), current.getY()].getDistances(), width, height);
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    for (int l = 0; l < height; l++)
                    {
                        if(distanceMatrix[i,j].getDistances()[k,l] == -1)
                        {
                            distanceMatrix[i, j].distances[k, l] = distanceMatrix[k, l].getDistances()[i, j];
                        }
                    }
                }
            }
        }

        return new Maze(maze, distanceMatrix);
    }

    private static void calculateDistance(int[,] toChange, int[,] toConst, int width, int height)
    {
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                if(toChange[i,j] == 0 || toChange[i,j] != -1)
                    continue;
                if(toConst[i,j] == -1)
                    continue;
                toChange[i,j] = toConst[i,j];
                toChange[i,j] += 1;
            }
        }
    }

    private static List<Neighbour> GetUnvisitedNeighbours(Position p, WallState[,] maze, int width, int height)
    {
        List<Neighbour> list = new List<Neighbour>();

        if (p.getY() > 0) // UP
        {
            if (!maze[p.getX(), p.getY() - 1].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour(new Position(p.getX(), p.getY() - 1), WallState.UP));
            }
        }

        if (p.getY() < height - 1) // DOWN
        {
            if (!maze[p.getX(), p.getY() + 1].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour(new Position(p.getX(), p.getY() + 1), WallState.DOWN));
            }
        }

        if (p.getX() > 0) // LEFT
        {
            if (!maze[p.getX() - 1, p.getY()].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour(new Position(p.getX() - 1, p.getY()), WallState.LEFT));
            }
        }

        if (p.getX() < width - 1) // RIGHT
        {
            if (!maze[p.getX() + 1, p.getY()].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour(new Position(p.getX() + 1, p.getY()), WallState.RIGHT));
            }
        }

        return list;
    }

    public static Maze Generate(int width, int height)
    {
        WallState[,] maze = new WallState[width, height];
        Distance[,] distanceMatrix = new Distance[width, height];
        WallState initial = WallState.RIGHT | WallState.LEFT | WallState.UP | WallState.DOWN;
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                maze[i,j] = initial;  // 1111
                distanceMatrix[i,j] = new Distance(height, width, i, j);
            }
        }
        
        return ApplyRecursiveBacktracker(maze, distanceMatrix, width, height);
        // return maze;
    }

    public static List<Position> Spawn(int width, int height, Distance[,] distanceMatrix, int players)
    {
        List<Position> spawnPoints = new List<Position>();
        Position position;
        System.Random random = new System.Random(/*seed*/);
        for (int i=1;i<=players;i++)
        {
            position = new Position(random.Next(0, width), random.Next(0, height));
            while(spawnPoints.Contains(position))
                position = new Position(random.Next(0, width), random.Next(0, height));
            spawnPoints.Add(position);
        }
        List<List<string>> coordinates = new List<List<string>>();
        for (int i=0;i<spawnPoints.Count;i++)
        {
            position = spawnPoints[i];
            List<CoordinateDistance> cd = new List<CoordinateDistance>();
            for(int j=0;j<width;j++)
            {
                for(int k=0;k<height;k++)
                {
                    cd.Add(new CoordinateDistance(distanceMatrix[position.getX(), position.getY()].getDistances()[j, k], j, k));
                }
            }
            cd.Sort();
            cd.Reverse();
            coordinates.Add(cd.Select(e => e.position.toString()).ToList());
        }
        IEnumerable<string> res = coordinates[0];
        for(int i=1;i<coordinates.Count;i++)
        {
            res = res.AsQueryable().Intersect(coordinates[i]);
        }
        List<Position> spawnPointsWithObj = new List<Position>();
        foreach(var j in spawnPoints)
        {
            spawnPointsWithObj.Add(j);
        }
        spawnPointsWithObj.Add(new Position(res.ElementAt(0)));
        return spawnPointsWithObj;
    }
}