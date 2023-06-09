using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    // Start is called before the first frame update

    #region Attributes
    public float OpenSpeed = 5f; //kecepatan buka pintu
    public float fadeSpeed = 10f; //kecepatan koin hilang
    public Transform door;
    public Vector2 StartPos = new Vector3(10.407f, -0.86f,0); //posisi awal pintu
    public Vector2 EndPos = new Vector3(10.407f, 3.55f,0); //posisi akhir pintu
    private bool open = false; //flag pintu
    SpriteRenderer keySprite; //dapetin sprite pintu
    #endregion

    #region Start
    private void Start()
    {
        keySprite = GetComponent<SpriteRenderer>();
    }
    #endregion

    #region "Animations"
    private void Update()
    {
        if (open)
        {
            door.position = Vector3.Lerp(door.position, EndPos, OpenSpeed * Time.deltaTime); //animasi pintu gerak ke atas
            keySprite.color = Color.Lerp(keySprite.color, new Color(keySprite.color.r,keySprite.color.g,keySprite.color.b,0),fadeSpeed*Time.deltaTime); //animasi fade trigger
        }
    }
    #endregion

    #region Trigger Script
    //Trigger kunci biar bisa buka pintu
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player") //jika tabrakan sama objek yang tagnya player
        {
            //Debug.Log("kunci diambil");
            //pintu hilang instan
            //door.SetActive(false);
            open = true; //tandai flag open jadi true
            //pintu animasi gerak ke atas

            //matikan trigger
            //this.gameObject.SetActive(false);
        }
    }
    #endregion
}
