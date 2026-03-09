
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloater : MonoBehaviour
{
 private Animator anim;

    [Header("Configuración del Disparo")]
    public GameObject prefabBolaCaida; // Arrastra aquí el prefab de la bola que va a caer
    public float velocidadCaida = 10f; // Qué tan rápido bajará la bola

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Revisamos si el objeto que nos golpeó tiene la etiqueta "Pelota"
        if (collision.gameObject.CompareTag("Ball")|| collision.gameObject.CompareTag("BallClone"))
        {   
            Debug.Log("Bloater golpeado por una bola, activando poder de bola descendente");
            // 1. Reproducimos la animación de daño (como lo configuramos antes)
            if (anim != null)
            {
                anim.SetTrigger("Golpe");
            }

            // 2. Ejecutamos la función para crear la bola descendente
            CrearBolaDescendente();
            GameManager.instance.EnemyDestroy(); // Notificamos al GameManager que un enemigo fue destruido
        }
    }

    void CrearBolaDescendente()
    {
        // Instanciamos la bola exactamente en la posición de este enemigo (Bloater)
        GameObject nuevaBola = Instantiate(prefabBolaCaida, transform.position, Quaternion.identity);

        // Buscamos el Rigidbody (3D) de la nueva bola para poder moverla con las físicas
        Rigidbody rbBola = nuevaBola.GetComponent<Rigidbody>();

        if (rbBola != null)
        {
            // Limpiamos cualquier velocidad extraña y la forzamos a ir en línea recta hacia abajo
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
