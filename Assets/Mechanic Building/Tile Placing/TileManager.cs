using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using TMPro;

public class TileManager : MonoBehaviour
{
    [SerializeField] Tilemap ground;
    [SerializeField] GameObject newGroundPrefabStatic, newGroundPrefabFalling;
    GameObject selectedGround;
    [SerializeField] float tileDestroyDelay = 5;
    [SerializeField] int staticBlockLimit, fallingBlockLimit, staticBlockCount, fallingBlockCount;
    [SerializeField] TMP_Text staticBlockCountText, fallingBlockCountText;
    Vector3 worldPos;
    List<Vector3Int> posOccupied = new List<Vector3Int>{};

    // Start is called before the first frame update
    void Start()
    {
        foreach(var position in ground.cellBounds.allPositionsWithin) {
            if (ground.HasTile(position)) {
                posOccupied.Add(position);
            }
        }

        selectedGround = newGroundPrefabStatic;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // remove
        if(Input.GetMouseButtonDown(1)){
            var tilePos = ground.WorldToCell(worldPos);
            ground.SetTile(tilePos, null);
            posOccupied.Remove(tilePos);
        }

        // add
        if(Input.GetMouseButtonDown(0)){
            if(!EventSystem.current.IsPointerOverGameObject()){
                var tilePos = ground.WorldToCell(worldPos);
                if(!posOccupied.Contains(tilePos)){
                    if(selectedGround == newGroundPrefabStatic && staticBlockCount < staticBlockLimit){
                        var newGroundObj = GameObject.Instantiate(selectedGround,
                                tilePos + new Vector3(0.5f,0.5f,0), Quaternion.identity);
                        posOccupied.Add(tilePos);
                        StartCoroutine(RemoveTileDelay(newGroundObj, tilePos));
                        staticBlockCount++;
                        staticBlockCountText.text = staticBlockCount.ToString();
                    } else
                    if(selectedGround == newGroundPrefabFalling && fallingBlockCount < fallingBlockLimit){
                        var newGroundObj = GameObject.Instantiate(selectedGround,
                                tilePos + new Vector3(0.5f,0.5f,0), Quaternion.identity);
                        posOccupied.Add(tilePos);
                        StartCoroutine(RemoveTileDelay(newGroundObj, tilePos));
                        fallingBlockCount++;
                        fallingBlockCountText.text = fallingBlockCount.ToString();
                    }
                }
            }
        }
    }

    IEnumerator RemoveTileDelay(GameObject tile, Vector3Int pos){
        float t = 0;
        while (t < tileDestroyDelay){
            t += Time.deltaTime;
            tile.transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", t/tileDestroyDelay*360);
            yield return null;
        }
        posOccupied.Remove(pos);
        Destroy(tile);
    }

    public void ChangeSelectedGround(int selected){
        if(selected == 0){
            selectedGround = newGroundPrefabStatic;
        } else selectedGround = newGroundPrefabFalling;
    }
}