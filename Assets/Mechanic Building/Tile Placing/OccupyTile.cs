using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OccupyTile : MonoBehaviour
{
    Tilemap offsetTilemap;
    [SerializeField] Vector3 offset;
    TileManager tileManager;
    Vector3Int lastTilePos;

    void Start()
    {
        offsetTilemap = GameObject.Find("Grid").transform.GetChild(0).GetComponent<Tilemap>();
        tileManager = TileManager.instance;
        lastTilePos = offsetTilemap.WorldToCell(transform.position + offset);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var tilePos = offsetTilemap.WorldToCell(transform.position + offset);

        if (tilePos != lastTilePos)
        {
            tileManager.posOccupied.Add(tilePos);
            tileManager.posOccupied.Remove(lastTilePos);
            lastTilePos = tilePos;
        }
    }
}
