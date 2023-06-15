using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{   //THIS CODE IS SCRAPPED, DO NOT USE IT
    //catatan: tag dengan nama "Untagged" harus diubah kalo seandainya block mau dikasih tag "Block"
    // Start is called before the first frame update
    public Vector3 PressurePlateEndPosition;
    public Vector3 PressurePlateStartPosition;
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
    void FixedUpdate()
    {

        if (trigger)
        {
            float complete = 1 - Mathf.Pow(speed,Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, PressurePlateEndPosition, complete);
        } else
        {
            float complete = 1 - Mathf.Pow(speed, Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, PressurePlateStartPosition, complete);
        }
    }
}
