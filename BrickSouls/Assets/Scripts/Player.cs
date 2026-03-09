using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 7f;

    public Rigidbody rb;

    public float input;

    private bool canMoveLeft = true;
    private bool canMoveRight = true;

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
        if ((input < 0 && !canMoveLeft) || (input > 0 && !canMoveRight))
        {
            input = 0;
        }

        Vector3 newPosition = rb.position + Vector3.right * input * speed * Time.fixedDeltaTime;
        //newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        rb.MovePosition(newPosition); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Izquierda"))
        {
            canMoveLeft = false;
            Debug.Log("Colision con izquierda");            
        }
        else if (collision.gameObject.CompareTag("Derecha"))
        {
            canMoveRight = false;
            Debug.Log("Colision con derecha");
        }   

        if (collision.gameObject.CompareTag("BallEnemy"))
        {
            Debug.Log("Colision con pelota");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Izquierda"))
        {
            canMoveLeft = true;
        }
        else if (collision.gameObject.CompareTag("Derecha"))
        {
            canMoveRight = true;
        }
    }
}
