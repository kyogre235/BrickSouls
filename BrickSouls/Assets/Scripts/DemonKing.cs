using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonKing : MonoBehaviour
{
   [Header("Estadísticas del Jefe")]
    public int maxHealth = 5; // Cuántos golpes resiste
    private int currentHealth;

    [Header("Comportamiento de Risa")]
    public float minLaughTime = 3f; // Segundos mínimos antes de reír
    public float maxLaughTime = 8f; // Segundos máximos antes de reír

    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        // Iniciamos el ciclo infinito de risas aleatorias
        StartCoroutine(RandomLaughRoutine());
    }

    // Usamos OnTriggerEnter (asumiendo que tu pelota es un Trigger para los enemigos, como con Throwel)
    // NOTA: Si tu jefe usa físicas sólidas para que la pelota rebote, cambia esto a "void OnCollisionEnter(Collision collision)"
    void OnCollisionEnter(Collision collision)
    {
        // Agregamos ".gameObject" antes de revisar el Tag
        if (collision.gameObject.CompareTag("PlayerBall") || 
            collision.gameObject.CompareTag("Ball") || 
            collision.gameObject.CompareTag("BallClone"))
        {
            if (isDead) return; // Si ya se está muriendo, ignoramos más golpes

            TakeDamage();
        }
    }

    void TakeDamage()
    {
        currentHealth--; // Le restamos 1 a la salud

        if (currentHealth > 0)
        {
            // Aún está vivo: reproducimos animación de golpe
            if (anim != null) anim.SetTrigger("Golpe");
            Debug.Log("¡El jefe recibió un golpe! Salud restante: " + currentHealth);
        }
        else
        {
            // La salud llegó a 0
            Die();
        }
    }

    void Die()
    {
        isDead = true; // Evita que siga riendo o recibiendo daño
        Debug.Log("¡Jefe derrotado!");

        // Le avisamos al GameManager para la condición de victoria
        if (GameManager.instance != null)
        {
            GameManager.instance.EnemyDestroy();
        }

        // Destruimos al jefe (puedes poner un retraso aquí si luego le haces animación de muerte)
        Destroy(gameObject);
    }

    // Esta rutina se ejecuta en segundo plano constantemente
    IEnumerator RandomLaughRoutine()
    {
        while (!isDead)
        {
            // Elige un tiempo al azar
            float waitTime = Random.Range(minLaughTime, maxLaughTime);
            
            // Espera esa cantidad de segundos
            yield return new WaitForSeconds(waitTime);

            // Si después de esperar sigue vivo, se ríe
            if (!isDead && anim != null)
            {
                anim.SetTrigger("Laugh");
            }
        }
    }
}
