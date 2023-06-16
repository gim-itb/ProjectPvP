using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{   //THIS CODE IS ARCHIVED, MIGHT BE USED LATER WHEN NEEDED
    //catatan: tag dengan nama "Untagged" harus diubah kalo seandainya block mau dikasih tag "Block"
    // Start is called before the first frame update
    public Vector3 PressurePlateEndPosition; //Posisi akhir pressure plate
    public Vector3 PressurePlateStartPosition; //Posisi awal pressure plate
    public float speed;
    private float elapsed;
    private bool trigger = false;
    void Start()
    {
        PressurePlateStartPosition = transform.position;
        PressurePlateEndPosition = new Vector3(transform.position.x, transform.position.y - 1, 0);
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.tag == "Player" || coll.tag == "Untagged" || coll.tag == "Entity")
        {
            trigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player" || coll.tag == "Untagged" || coll.tag == "Entity")
        {
            trigger = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {
            transform.position = V3Damp(transform.position, PressurePlateEndPosition, speed, Time.deltaTime);
        } else
        {
            transform.position = V3Damp(transform.position, PressurePlateStartPosition, speed, Time.deltaTime);
        }
    }

    public static Vector3 V3Damp(Vector3 a, Vector3 b, float lambda, float dt)
    {
        return Vector3.Lerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
