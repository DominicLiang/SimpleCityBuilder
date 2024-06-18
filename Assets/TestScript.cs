using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var y = Input.GetAxis("Vertical");
        transform.position += new Vector3(0, y * 0.05f, 0);
    }
}
