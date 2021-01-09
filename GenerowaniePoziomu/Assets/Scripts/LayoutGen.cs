using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayoutGen
{
    int maxHeight;
    int maxWidth;
    
    int roomsToSpawn;
    Vector2Int spawnPos;
    Vector2Int bossPos;

    public enum Room
    {
        empty,
        spawn,
        encounter,
        boss
    }

    Room[,] grid;

    int currentRoomCount;

    public Room[,] GenerateLayout( int width, int height, int nrOfRooms, Vector2Int spawn )
    {
        maxHeight = width;
        maxWidth = height;
        roomsToSpawn = nrOfRooms;
        spawnPos = spawn;

        grid = new Room[maxWidth, maxHeight];

        GenerateLevel();

        return grid;
    }

    void InitGrid()
    {
        currentRoomCount = 0;
        
        for( int y = 0; y < maxHeight; y++ )
        {
            for( int x = 0; x < maxWidth; x++ )
            {
                grid[ x, y ] = Room.empty;
            }
        }
    }

    void GenerateLevel()
    {
        InitGrid();

        grid[ spawnPos.x, spawnPos.y ] = Room.spawn;

        Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();
        roomQueue.Enqueue( spawnPos );

        while( roomQueue.Count > 0 )
        {
            List <Vector2Int> neighbours = GetNeighbours( roomQueue.Dequeue() );

            foreach( Vector2Int neighbour in neighbours )
            {
                if( currentRoomCount == roomsToSpawn ) continue;
                
                if( grid[ neighbour.x, neighbour.y ] != Room.empty ) continue;
                
                if( GetNeighbours( neighbour ).FindAll( n => grid[ n.x, n.y ] != Room.empty ).Count > 1 ) continue;
                
                if( Random.Range( 0f, 1f ) < 0.5f ) continue;

                currentRoomCount++;
                if( currentRoomCount == roomsToSpawn )
                {
                    grid[ neighbour.x, neighbour.y ] = Room.boss;
                    bossPos = neighbour;
                }
                else
                {
                    grid[ neighbour.x, neighbour.y ] = Room.encounter;
                }
                
                roomQueue.Enqueue( neighbour );
            }
        }

        if( !IsLevelCorrect() )
        {
            GenerateLevel();
        }
    }

    List<Vector2Int> GetNeighbours( Vector2Int pos )
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        
        if( pos.x > 0 ) neighbours.Add( pos + Vector2Int.left );
        if( pos.y > 0 ) neighbours.Add( pos + Vector2Int.down );
        if( pos.x < maxWidth - 1 ) neighbours.Add( pos + Vector2Int.right );
        if( pos.y < maxHeight - 1 ) neighbours.Add( pos + Vector2Int.up );

        return neighbours;
    }

    bool IsLevelCorrect()
    {
        if( currentRoomCount != roomsToSpawn ) return false;

        if( GetNeighbours( bossPos ).Any( n => grid[ n.x, n.y ] == Room.spawn ) ) return false;

        return true;
    }

    public void DebugDisplay()
    {
        string lvl = "";

        for( int y = 0; y < maxHeight; y++ )
        {
            for( int x = 0; x < maxWidth; x++ )
            {
                switch( grid[x,y] )
                {
                    case Room.empty:
                        lvl += ".";
                        break;
                    case Room.encounter:
                        lvl += "#";
                        break;
                    case Room.spawn:
                        lvl += "S";
                        break;
                    case Room.boss:
                        lvl += "B";
                        break;
                }
            }

            lvl += "\n";
        }

        Debug.Log( lvl );
    }
}
