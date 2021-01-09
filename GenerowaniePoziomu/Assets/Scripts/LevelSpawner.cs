using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public int maxWidth = 8;
    public int maxHeight = 8;
    public int roomsToSpawn = 12;
    public Vector2Int spawnPos = new Vector2Int( 3, 3 );

    LayoutGen layoutGen;
    LayoutGen.Room[,] grid;

    public GameObject spawnRoom;
    public GameObject encounterRoom;
    public GameObject bossRoom;
    
    public GameObject openWall;
    public GameObject fullWall;

    public Vector2 roomSize;
    
    void Start()
    {
        layoutGen = new LayoutGen();
        grid = layoutGen.GenerateLayout( maxWidth, maxHeight, roomsToSpawn, spawnPos );
        layoutGen.DebugDisplay();

        SpawnRooms();
    }

    void SpawnRooms()
    {
        for( int y = 0; y < maxHeight; y++ )
        {
            for( int x = 0; x < maxWidth; x++ )
            {
                SpawnRoom( grid[x, y], new Vector2Int( x, y ) );
            }
        }
    }

    void SpawnRoom( LayoutGen.Room roomType, Vector2Int gridPos )
    {
        switch( roomType )
        {
            case LayoutGen.Room.empty:
                break;
            
            case LayoutGen.Room.spawn:
                InstantiateRoom( spawnRoom, gridPos );
                break;
            
            case LayoutGen.Room.encounter:
                InstantiateRoom( encounterRoom, gridPos );
                break;
            
            case LayoutGen.Room.boss:
                InstantiateRoom( bossRoom, gridPos );
                break;
            
            default:
                throw new ArgumentOutOfRangeException( nameof(roomType), roomType, null );
        }
    }

    void InstantiateRoom( GameObject roomPrefab, Vector2Int gridPos )
    {
        Vector3 pos = transform.position + new Vector3( gridPos.x * roomSize.x, 0, gridPos.y * roomSize.y );

        GameObject room = Instantiate( roomPrefab, pos, Quaternion.identity );
        room.transform.SetParent( transform );

        SpawnRoomWalls( gridPos, room.transform );
    }

    void SpawnRoomWalls( Vector2Int gridPos, Transform roomTransform )
    {
        SpawnCorrectWall( gridPos, Vector2Int.up, roomTransform );
        SpawnCorrectWall( gridPos, Vector2Int.down, roomTransform );
        SpawnCorrectWall( gridPos, Vector2Int.left, roomTransform );
        SpawnCorrectWall( gridPos, Vector2Int.right, roomTransform );
    }

    void SpawnCorrectWall( Vector2Int gridPos, Vector2Int dir, Transform roomTransform )
    {
        Vector3 roomPos = transform.position + new Vector3( gridPos.x * roomSize.x, 0, gridPos.y * roomSize.y );
        
        if( IsPosValid( gridPos + dir ) )
        {
            bool open = grid[ gridPos.x + dir.x, gridPos.y + dir.y ] != LayoutGen.Room.empty;
            GameObject wall = SpawnWall( roomPos, dir, open );
            wall.transform.SetParent( roomTransform );
        }
        else
        {
            GameObject wall = SpawnWall( roomPos, dir, false );
            wall.transform.SetParent( roomTransform );
        }
    }

    bool IsPosValid( Vector2Int pos ) => pos.x >= 0 && pos.y >= 0 && pos.x < maxWidth && pos.y < maxHeight;

    GameObject SpawnWall( Vector3 roomPos, Vector2Int dir, bool open )
    {
        Vector3 wallPos = roomPos;
        Quaternion wallRot = Quaternion.identity;

        if( dir == new Vector2Int( 0, 1 ) )
        {
            wallPos += Vector3.forward * ( roomSize.y / 2 );
            wallRot = Quaternion.Euler( 0, 90, 0 );
        }
        else if( dir == new Vector2Int( 0, -1 ) )
        {
            wallPos += Vector3.back * ( roomSize.y / 2 );
            wallRot = Quaternion.Euler( 0, 270, 0 );
        }
        else if( dir == new Vector2Int( -1, 0 ) )
        {
            wallPos += Vector3.left * ( roomSize.x / 2 );
            wallRot = Quaternion.Euler( 0, 0, 0 );
        }
        else if( dir == new Vector2Int( 1, 0 ) )
        {
            wallPos += Vector3.right * ( roomSize.x / 2 );
            wallRot = Quaternion.Euler( 0, 180, 0 );
        }

        GameObject prefab = open ? openWall : fullWall;

        return Instantiate( prefab, wallPos, wallRot );
    }
}
