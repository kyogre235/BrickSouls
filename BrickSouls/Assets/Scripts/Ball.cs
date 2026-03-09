using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float launchSpeed = 8f;

    public bool launched = false;

    private Rigidbody rb;

    [Header("Referencia al pad")]
    public Transform padtransform;
    public Vector3 offsetFromPad = new Vector3(0f,0.65f, 0f);

    void Awake()
    {   
        rb = GetComponent<Rigidbody>();
        padtransform = GameObject.Find("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (launched)
        {
            rb.velocity = rb.velocity.normalized * launchSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("BallEnemy"))
        {
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Launch();
        }
    }

    public void Launch()
    {
        float angle = Random.Range(10f, 170f);
        float radians = angle * Mathf.Deg2Rad;

        Vector3 direccion = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f);
        rb.velocity = direccion.normalized * launchSpeed;
        launched = true;
    }

    void FollowPad()
    {
        transform.position = padtransform.position + offsetFromPad;
    }

    public void ResetBall()
    {
        launched = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        FollowPad();
        Debug.Log("Ball reset");
    }

    public void MultiplySpeed(int mult)
    {
        launchSpeed *= mult;
    }

    public void DivideSpeed(int div)
    {
        launchSpeed /= div;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeathZone" && gameObject.CompareTag("Ball"))
        {
            ResetBall();
            GameManager.instance.LoseLifes();
        } else if (other.gameObject.tag == "DeathZone" && gameObject.CompareTag("BallClone"))
        {
            Destroy(gameObject);
        } else if(other.gameObject.tag == "DeathZone" && gameObject.CompareTag("BallEnemy"))
        {
            Destroy(gameObject);
            GameManager.instance.LoseLifes();
        }
    }

    void Update()
    {   
        if (!launched)
        {
            if(gameObject.CompareTag("Ball"))
            {
                FollowPad();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Launch();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && gameObject.CompareTag("Ball"))
        {
            ResetBall();
        }

    }
}
