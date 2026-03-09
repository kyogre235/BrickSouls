using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DK_Hand : MonoBehaviour
{
[Header("Estadísticas")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Ataque de Fuego")]
    public GameObject fireballPrefab;  
    public Transform firePoint;        
    public float throwForce = 3f;     
    public float cooldown = 12f;       
    
    // Variables de control interno
    private float nextFireTime = 0f;
    private Transform player;
    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        
        // Buscamos al jugador
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Iniciamos el ciclo de pensar y atacar
        StartCoroutine(ThinkAndActRoutine());
    }

    // Usamos física 3D como acordamos para los enemigos anteriores
    void OnCollisionEnter(Collision collision)
    {
        if ( 
            collision.gameObject.CompareTag("Ball") || 
            collision.gameObject.CompareTag("BallClone"))
        {
            if (isDead) return;
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        currentHealth--;
        
        if (currentHealth > 0)
        {
            if (anim != null) anim.SetTrigger("Hit");
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        
        // Avisamos al GameManager para la condición de victoria
        if (GameManager.instance != null)
        {
            GameManager.instance.EnemyDestroy();
        }
        
        Destroy(gameObject);
    }

    // El "Cerebro" del enemigo
    IEnumerator ThinkAndActRoutine()
    {
        while (!isDead)
        {
            // Revisa la situación cada 1 a 2 segundos de forma aleatoria (para que no se vea robótico)
            yield return new WaitForSeconds(Random.Range(1f, 2f));

            // 1. ¿Ya pasó el Cooldown?
            // 2. ¿Tengo línea de visión directa al jugador?
            if (Time.time >= nextFireTime && HasLineOfSight())
            {
                // Bloqueamos el siguiente disparo por 10 segundos
                nextFireTime = Time.time + cooldown;
                
                // Disparamos
                StartCoroutine(ShootRoutine());
            }
        }
    }

    bool HasLineOfSight()
    {
        if (player == null) return false;

        // Calculamos la dirección y la distancia exacta hacia el jugador
        Vector3 direction = player.position - firePoint.position;
        float distanceToPlayer = direction.magnitude;

        // Disparamos un Raycast que atraviesa todo hasta llegar al jugador
        RaycastHit[] hits = Physics.RaycastAll(firePoint.position, direction.normalized, distanceToPlayer);

        // Revisamos cada cosa que el rayo tocó
        foreach (RaycastHit hit in hits)
        {
            // Si el rayo choca con el propio cuerpo del FireEnemy, lo ignoramos
            if (hit.collider.gameObject == gameObject) continue;

            // Si choca con un compañero (Enemy) o un muro (Brick), la visión está bloqueada
            if (hit.collider.CompareTag("Enemy") || hit.collider.GetComponent<Brick>() != null)
            {
                return false; // No dispares
            }
        }

        // Si el rayo llegó al jugador sin chocar con Ladrillos ni Enemigos, ¡camino libre!
        return true;
    }

    IEnumerator ShootRoutine()
    {
        // Activamos la animación
        if (anim != null) anim.SetTrigger("Throw");

        // Esperamos un instante para que la bola salga cuando el brazo se mueva (como en el Thrower)
        yield return new WaitForSeconds(0.7f);

        if (player != null && !isDead)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            
            // Calculamos de nuevo la dirección por si el jugador se movió en esos 0.4 segundos
            Vector3 direction = (player.position - firePoint.position).normalized;
            
            Rigidbody rb = fireball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * throwForce;
            }
        }
    }
}
