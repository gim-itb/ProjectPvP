using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PressurePlateSingleScript : MonoBehaviour
{   
    //catatan: tag dengan nama "Untagged" harus diubah kalo seandainya block mau dikasih tag "Block"
    // Start is called before the first frame update

    public Vector3 PressurePlateStartPosition; //Posisi awal pressure plate
    public Vector3 PressurePlateEndPosition; //Posisi akhir pressure plate
    public float PressurePlateSpeed; //kecepatan pressure plate
    public GameObject Door;
    public Vector3 DoorStartPosition; //Posisi awal pressure plate
    public Vector3 DoorEndPosition; //Posisi akhir pressure plate
    public float DoorSpeed; //kecepatan pintu
    private bool trigger = false;
    void Start()
    {
        PressurePlateStartPosition = transform.position;
        DoorStartPosition = Door.transform.position;
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag == "Player" || coll.tag == "Block")
        {
            trigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player" || coll.tag == "Block")
        {
            trigger= false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {
            transform.position = V3Damp(transform.position, PressurePlateEndPosition, PressurePlateSpeed, Time.deltaTime);
            Door.transform.position = V3Damp(Door.transform.position, DoorEndPosition, DoorSpeed, Time.deltaTime);
        }
        else
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