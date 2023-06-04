using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject door;//dapetin object door

    //Trigger kunci biar bisa buka pintu
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("kunci diambil");
            //pintu hilang instan
            door.SetActive(false);
            //pintu animasi gerak ke atas

            this.gameObject.SetActive(false);
        }
    }
}
