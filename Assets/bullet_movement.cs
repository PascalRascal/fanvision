using UnityEngine;
using System.Collections;

public class bullet_movement : MonoBehaviour
{
    private float speed = 5f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
