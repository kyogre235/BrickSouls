
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloater : MonoBehaviour
{
 private Animator anim;

    [Header("Configuración del Disparo")]
    public GameObject prefabBolaCaida; 
    public float velocidadCaida = 8f; 

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ball")|| collision.gameObject.CompareTag("BallClone"))
        {   
            Debug.Log("Bloater golpeado por una bola, activando poder de bola descendente");
            
            if (anim != null)
            {
                anim.SetTrigger("Golpe");
            }

            
            CrearBolaDescendente();
            GameManager.instance.EnemyDestroy(); 
        }
    }

    void CrearBolaDescendente()
    {
        
        GameObject nuevaBola = Instantiate(prefabBolaCaida, transform.position, Quaternion.identity);

        
        Rigidbody rbBola = nuevaBola.GetComponent<Rigidbody>();

        if (rbBola != null)
        {
            
            rbBola.velocity = Vector3.down * velocidadCaida;
        }
        else
        {
            Debug.LogWarning("¡Al prefabBolaCaida le falta un Rigidbody para poder caer!");
        }

        // Ya terminó el poder, ahora sí destruimos este power-up
        Destroy(gameObject);
    }
}
