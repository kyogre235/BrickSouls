
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwel : MonoBehaviour
{
 [Header("Configuración del Contraataque")]
    [Tooltip("Probabilidad de atrapar la bola (0.2 = 20%)")]
    public float catchChance = 0.2f; 
    public GameObject enemyBallPrefab; // El prefab de la bola que Throwel va a lanzar
    public Transform throwPoint;       // De dónde saldrá la bola
    public float throwForce = 10f;     // Fuerza de la bola devuelta

    private GameObject caughtBall;
    private Animator anim;
    private Transform player;

    void Start()
    {
        anim = GetComponent<Animator>();
        
        // Buscamos al jugador para saber a dónde apuntar
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    // ATENCIÓN: Ahora usamos OnTriggerEnter y Collider (sin el "2D" al final)
    void OnTriggerEnter(Collider collision)
    {
        // Verificamos si lo que nos golpeó es la bola del jugador
        if (collision.CompareTag("Ball"))
        {   
            Debug.Log("golpeado");
            // Tiramos los dados: ¿Cayó en el 20% de probabilidad?
            if (Random.value <= catchChance)
            {
              // 1. Guardamos la bola exacta que nos golpeó
                caughtBall = collision.gameObject;
                
                // 2. La desactivamos (desaparece visualmente y físicamente)
                caughtBall.SetActive(false);
                
                // Y ejecuta su habilidad de devolverla
                CatchAndThrow();
            }
            else
            {
                // Cayó en el 80%: Throwel no reacciona a tiempo y muere.
                // Como NO pusimos Destroy(collision.gameObject) aquí, 
                // la bola del jugador seguirá existiendo y continuará su camino normal.
                GameManager.instance.EnemyDestroy(); // Notificamos al GameManager que un enemigo fue destruido
                Destroy(gameObject);
            }
        } else if (collision.CompareTag("BallClone"))
        {
            // Si nos golpea una bola clon, simplemente destruimos esa bola clon sin activar el contraataque
            Destroy(collision.gameObject);
            GameManager.instance.EnemyDestroy(); // Notificamos al GameManager que un enemigo fue destruido
        }
    }

    void CatchAndThrow()
    {
        // Activamos la animación de lanzar
        if (anim != null) anim.SetTrigger("Throw");

        // Iniciamos la pausa para sincronizar con la animación
        StartCoroutine(ThrowRoutine());
    }

    IEnumerator ThrowRoutine()
    {
        // Esperamos 0.4 segundos (ajusta esto según lo que dure tu animación)
        yield return new WaitForSeconds(0.4f);

        if (player != null)
        {
            caughtBall.transform.position = throwPoint.position;
            caughtBall.SetActive(true);

            // Calculamos la dirección hacia el jugador en 3D (Vector3)
            Vector3 direction = (player.position - throwPoint.position).normalized;

            // ATENCIÓN: Usamos Rigidbody (sin el "2D") para aplicarle la fuerza
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
