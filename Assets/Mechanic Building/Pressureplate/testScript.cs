using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 endPosition = new Vector3(5, 2, 0);
    private Vector3 startPosition;
    private float duration = 3f;
    private float elapsed;
    void Start()
    {
        startPosition= transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        float complete = elapsed / duration;
        transform.position = Vector3.Lerp(startPosition, endPosition,complete);
    }
}
