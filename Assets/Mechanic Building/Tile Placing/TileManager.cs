using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using TMPro;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;

    [SerializeField] Tilemap ground;
    [SerializeField] GameObject newGroundPrefabStatic, newGroundPrefabFalling;
    GameObject selectedGround;
    [SerializeField] float tileDestroyDelay = 5;
    [SerializeField] int staticBlockLimit, fallingBlockLimit, storedBlockLimit,
        staticBlockCount, fallingBlockCount, storedBlockCount;
    [SerializeField] TMP_Text staticBlockCountText, fallingBlockCountText, storedBlockCountText;
    Vector3 worldPos;
    public List<Vector3Int> posOccupied = new List<Vector3Int>{};
    List<Vector3Int> groundTiles = new List<Vector3Int>{};

    //
    bool isEnabled = true;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        foreach(var position in ground.cellBounds.allPositionsWithin) {
            if (ground.HasTile(position)) {
                groundTiles.Add(position);
            }
        }
        posOccupied.AddRange(groundTiles);

        //storedBlockLimit = staticBlockLimit + fallingBlockLimit;
        selectedGround = newGroundPrefabStatic;
        
        UpdateBlockCount();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEnabled) return;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // remove ground block
        if(storedBlockCount + staticBlockCount + fallingBlockCount < storedBlockLimit){
            if(Input.GetMouseButtonDown(1)){
                var tilePos = ground.WorldToCell(worldPos);
                if (groundTiles.Contains(tilePos)){
                    ground.SetTile(tilePos, null);
                    posOccupied.Remove(tilePos);
                    groundTiles.Remove(tilePos);
                    storedBlockCount++;
                    UpdateBlockCount();
                }
            }
        }

        // place block
        if(storedBlockCount > 0 && staticBlockCount + fallingBlockCount + storedBlockCount <= storedBlockLimit){
            if(Input.GetMouseButtonDown(0)){
                if(!EventSystem.current.IsPointerOverGameObject()){
                    var tilePos = ground.WorldToCell(worldPos);
                    if(!posOccupied.Contains(tilePos)){
                        if(selectedGround == newGroundPrefabStatic && staticBlockCount < staticBlockLimit){
                            CreateBlock();
                            staticBlockCount++;
                            UpdateBlockCount();
                        } else
                        if(selectedGround == newGroundPrefabFalling && fallingBlockCount < fallingBlockLimit){
                            CreateBlock();
                            fallingBlockCount++;
                            UpdateBlockCount();
                        }
                        void CreateBlock(){
                            var newGroundObj = GameObject.Instantiate(selectedGround,
                                    tilePos + new Vector3(0.5f,0.5f,0), Quaternion.identity);
                            posOccupied.Add(tilePos);
                            storedBlockCount--;
                            UpdateBlockCount();
                            StartCoroutine(RemoveTileDelay(newGroundObj, tilePos, selectedGround));
                        }
                    }
                }
            }
        }
    }

    IEnumerator RemoveTileDelay(GameObject tile, Vector3Int pos, GameObject type){
        float t = 0;
        while (t < tileDestroyDelay){
            t += Time.deltaTime;
            tile.transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetFloat("_Arc1", t/tileDestroyDelay*360);
            yield return null;
        }
        posOccupied.Remove(pos);
        Destroy(tile);
        //storedBlockCount++;

        if(type == newGroundPrefabStatic){
            staticBlockCount--;
            UpdateBlockCount();
        } else {
            fallingBlockCount--;
            UpdateBlockCount();
        }
    }

    void UpdateBlockCount(){
        staticBlockCountText.text = $"{staticBlockCount}/{staticBlockLimit}";
        fallingBlockCountText.text = $"{fallingBlockCount}/{fallingBlockLimit}";
        storedBlockCountText.text = $"Block stored: {storedBlockCount}/{storedBlockLimit}";
    }

    public void ChangeSelectedGround(int selected){
        if(selected == 0){
            selectedGround = newGroundPrefabStatic;
        } else selectedGround = newGroundPrefabFalling;
    }

    public void SetActive(bool active)
    {
        isEnabled = active;
    }
}