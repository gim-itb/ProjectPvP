using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PressurePlateScript : MonoBehaviour
{   //THIS CODE IS ARCHIVED, MIGHT BE USED LATER WHEN NEEDED
    //catatan: tag dengan nama "Untagged" harus diubah kalo seandainya block mau dikasih tag "Block"
    // Start is called before the first frame update

    public Vector3 PressurePlateStartPosition; //Posisi awal pressure plate
    public Vector3 PressurePlateEndPosition; //Posisi akhir pressure plate
    public float PressurePlateSpeed; //kecepatan pressure plate
    public GameObject Door; 
    public Vector3 DoorStartPosition; //Posisi awal pressure plate
    public Vector3 DoorEndPosition; //Posisi akhir pressure plate
    public float DoorSpeed; //kecepatan pintu
    public int MinimumMass;
    public List<GameObject> CollisionList;
    private bool trigger = false;
    void Start()
    {
        PressurePlateStartPosition = transform.position;
        DoorStartPosition = Door.transform.position;
        GetComponent<Transform>().localScale = new Vector3(MinimumMass,1, 1);
        GetComponent<BoxCollider2D>().size = new Vector2(1, MinimumMass);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Player" || coll.tag == "Block")
        {
            CollisionList.Add(coll.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player" || coll.tag == "Block")
        {
            CollisionList.Remove(coll.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (CollisionList.Count >= MinimumMass)
        {
            transform.position = V3Damp(transform.position, PressurePlateEndPosition, PressurePlateSpeed, Time.deltaTime);
            Door.transform.position = V3Damp(Door.transform.position, DoorEndPosition, DoorSpeed, Time.deltaTime);
        } else
        {
            transform.position = V3Damp(transform.position, PressurePlateStartPosition, PressurePlateSpeed, Time.deltaTime);
            Door.transform.position = V3Damp(Door.transform.position, DoorStartPosition, DoorSpeed, Time.deltaTime);
        }
    }

    public static Vector3 V3Damp(Vector3 a, Vector3 b, float lambda, float dt)
    {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
