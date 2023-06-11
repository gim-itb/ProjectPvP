using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{   //catatan: tag dengan nama "Untagged" harus diubah kalo seandainya block mau dikasih tag "Block"
    #region Attributes
    public Vector3 PressurePlateStartPos,DoorStartPos;
    public Vector3 PressurePlateEndPos, DoorEndPos;
    public GameObject door;
    bool back = false;
    public float PressurePlateSpeed = 0.005f;
    public float DoorSpeed = 0.015f;
    #endregion
    #region Get door and pressure plate original position
    private void Start()
    {
        PressurePlateStartPos = transform.position;
        DoorStartPos = door.transform.position;
    }
    #endregion

    #region Saat benda dan pemain tetap di atas pressure plate
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player" || collision.collider.tag == "Untagged" || collision.collider.tag == "Entity")
        {
            transform.Translate(0, -PressurePlateSpeed, 0);
            door.transform.Translate(-DoorSpeed, 0, 0);//sorry pintunya diputer -90 derajat ke arah sumbu z di scene PressurePlateTest, ubah nilainya dan pindahin ke nilai y nanti
            back = false;
        }
    }
    #endregion

    #region Saat pressure platenya dipencet
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" || collision.collider.tag == "Untagged" || collision.collider.tag == "Entity")
        {
            collision.transform.parent = transform;
        }
    }
    #endregion

    #region Saat pressure platenya dilepas
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" || collision.collider.tag == "Untagged" || collision.collider.tag == "Entity")
        {
            collision.transform.parent = null;
            back = true;
        }
    }
    #endregion
    #region Balik ke posisi semula
    private void Update()
    {
        if (back)
        {
            if (transform.position.y < PressurePlateStartPos.y)
            {
                transform.Translate(0, PressurePlateSpeed, 0);
                door.transform.Translate(DoorSpeed, 0, 0); //sorry pintunya diputer -90 derajat ke arah sumbu z di scene PressurePlateTest, ubah nilainya dan pindahin ke nilai y nanti
            }
            else
            {
                back = false;
            }
        }
    }
    #endregion
}
