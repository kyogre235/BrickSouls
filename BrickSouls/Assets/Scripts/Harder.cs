using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harder : MonoBehaviour
{
  private Animator anim;
    private Collider col;

    
    public float tiempoDestruccion = 0.5f; 

    public int health = 2; 

    void Start()
    {
        
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball")|| collision.gameObject.CompareTag("BallClone"))
        {
            
            anim.SetTrigger("Golpe");
            Debug.Log("Colision con pelota, animación de destrucción activada");
            health--;
            if (health <= 0)
            {
                GameManager.instance.EnemyDestroy(); 
                Destroy(gameObject, tiempoDestruccion);
            }
        }
    }
}
