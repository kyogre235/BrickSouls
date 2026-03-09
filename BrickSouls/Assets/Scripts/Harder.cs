using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harder : MonoBehaviour
{
  private Animator anim;
    private Collider col;

    // Ajusta esto al tiempo que dure tu animación de destrucción en segundos
    public float tiempoDestruccion = 0.5f; 

    public int health = 2; // Vida del enemigo

    void Start()
    {
        // Obtenemos las referencias automáticamente
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Asegúrate de que tu pelota tenga la etiqueta (Tag) "Ball" en el editor
        if (collision.gameObject.CompareTag("Ball")|| collision.gameObject.CompareTag("BallClone"))
        {
            // 1. Disparamos la animación
            anim.SetTrigger("Golpe");
            Debug.Log("Colision con pelota, animación de destrucción activada");
            // 2. Apagamos el collider para que la pelota no rebote dos veces
            // mientras se reproduce la animación
            //col.enabled = false;
            
            health--;
            if (health <= 0)
            {
                GameManager.instance.EnemyDestroy(); // Notificamos al GameManager que un enemigo fue destruido
                // 3. Destruimos al enemigo después de que termine la animación
                Destroy(gameObject, tiempoDestruccion);
            }
        }
    }
}
