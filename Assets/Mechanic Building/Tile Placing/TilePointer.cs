using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePointer : MonoBehaviour
{
    [SerializeField] Tilemap offsetTilemap;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        var tilePos = offsetTilemap.WorldToCell(worldPos);

        transform.position = tilePos + new Vector3(0.5f,0.5f,0);
    }
}
