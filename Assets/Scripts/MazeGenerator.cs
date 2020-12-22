/*
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

    public int getX()
    {
        return X;
    }

    public int getY()
    {
        return Y;
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

    private static WallState[,] ApplyRecursiveBacktracker(WallState[,] maze, int width, int height)
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
            }
        }

        return maze;
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

    public static WallState[,] Generate(int width, int height)
    {
        WallState[,] maze = new WallState[width, height];
        WallState initial = WallState.RIGHT | WallState.LEFT | WallState.UP | WallState.DOWN;
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                maze[i, j] = initial;  // 1111
            }
        }
        
        return ApplyRecursiveBacktracker(maze, width, height);
        // return maze;
    }
}