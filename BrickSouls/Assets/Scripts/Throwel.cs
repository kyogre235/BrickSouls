
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwel : MonoBehaviour
{
 [Header("Configuración del Contraataque")]
    [Tooltip("Probabilidad de atrapar la bola (0.2 = 20%)")]
    public float catchChance = 0.2f; 
    public GameObject enemyBallPrefab; 
    public Transform throwPoint;       
    public float throwForce = 10f;     

    private GameObject caughtBall;
    private Animator anim;
    private Transform player;

    void Start()
    {
        anim = GetComponent<Animator>();
        
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    
    void OnTriggerEnter(Collider collision)
    {
        
        if (collision.CompareTag("Ball"))
        {   
            Debug.Log("golpeado");
            
            if (Random.value <= catchChance)
            {
              
                caughtBall = collision.gameObject;
                
                
                caughtBall.SetActive(false);
                
                
                CatchAndThrow();
            }
            else
            {
                
                GameManager.instance.EnemyDestroy(); 
            }
        } else if (collision.CompareTag("BallClone"))
        {
            
            Destroy(collision.gameObject);
            GameManager.instance.EnemyDestroy(); 
        }
    }

    void CatchAndThrow()
    {
       
        if (anim != null) anim.SetTrigger("Throw");

        
        StartCoroutine(ThrowRoutine());
    }

    IEnumerator ThrowRoutine()
    {
        
        yield return new WaitForSeconds(0.4f);

        if (player != null)
        {
            caughtBall.transform.position = throwPoint.position;
            caughtBall.SetActive(true);

            
            Vector3 direction = (player.position - throwPoint.position).normalized;

            
            Rigidbody rb = caughtBall.GetComponent<Rigidbody>();
            if (rb != null)
            {   
                rb.velocity = Vector3.zero;
                rb.velocity = direction * throwForce;
            }
            caughtBall = null;
        }
    }
}
