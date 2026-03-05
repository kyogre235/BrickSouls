using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 7f;

    public float minX = -2.8f;

    public float maxX = 2.1f;

    public Rigidbody rb;

    public float input;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Update()
    {
        input = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        Vector3 newPosition = rb.position + Vector3.right * input * speed * Time.fixedDeltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        rb.MovePosition(newPosition); 
    }
}
