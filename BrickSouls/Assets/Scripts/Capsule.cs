using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    // 1. El Enum con los tipos de Power-Up
    public enum TipoPowerUp 
    { 
        MultiBola, 
        VelocidadExtra,
        VidaExtra,
        Inmundad,
    }

    public TipoPowerUp miTipo;

    [Header("Sprites de los Power-Ups")]
    public Sprite spriteMultiBola;
    public Sprite spriteVelocidad;
    public Sprite spriteVidaExtra;
    public Sprite spriteInmundad;

    [Header("Configuración")]
    public float speed = 5f;
    public GameObject prefabBall;
    private Transform currentBall;

    // Referencias a los componentes
    private SpriteRenderer spriteRenderer;
    private Collider miCollider;

    [Header("Efectos de Sonido")]
    public AudioClip getpowerUpSound;

    void Awake()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        miCollider = GetComponent<Collider>();
    }

    void Start()
    {
        // Buscamos la pelota actual
        GameObject ballObj = GameObject.FindGameObjectWithTag("Ball");
        if (ballObj != null)
        {
            currentBall = ballObj.transform;
        }

        int tipoAleatorio = Random.Range(0, 3);
        ConfigurarPowerUp((TipoPowerUp)tipoAleatorio);
    }

    void Update()
    {
        
        this.transform.Translate(Vector3.right * -1 * speed * Time.deltaTime);        
    }

    // Método para asignar el sprite según el tipo
    public void ConfigurarPowerUp(TipoPowerUp nuevoTipo)
    {
        miTipo = nuevoTipo;

        switch (miTipo)
        {
            case TipoPowerUp.MultiBola:
                spriteRenderer.sprite = spriteMultiBola;
                break;
            case TipoPowerUp.VelocidadExtra:
                spriteRenderer.sprite = spriteVelocidad;
                break;
            case TipoPowerUp.VidaExtra:
                spriteRenderer.sprite = spriteVidaExtra; 
                break;
            case TipoPowerUp.Inmundad:
                spriteRenderer.sprite = spriteInmundad;  
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            spriteRenderer.enabled = false;
            miCollider.enabled = false;

            switch (miTipo)
            {
                case TipoPowerUp.MultiBola:
                    Debug.Log("PowerUp 1: Multibola activado");
                    StartCoroutine(MultiBallCoroutine());
                    break;
                case TipoPowerUp.VelocidadExtra:
                    Debug.Log("PowerUp 2: Velocidad Extra activada");
                    StartCoroutine(ExtraSpeedCoroutine());
                    break;
                case TipoPowerUp.VidaExtra:
                    Debug.Log("PowerUp 3: Vida Extra activada");
                    StartCoroutine(ExtraHealthCoroutine());
                    break;
                case TipoPowerUp.Inmundad:
                    Debug.Log("PowerUp 4: Inmundad activada");
                    StartCoroutine(ExtraMundanityCoroutine());
                    break;
            }
            if (GameManager.instance != null)
            {
                GameManager.instance.PlaySFX(getpowerUpSound);
            }
        }
    }

    // --- CORRUTINAS ---

    IEnumerator MultiBallCoroutine()
    {
        var newBall1 = Instantiate(prefabBall, currentBall.position, Quaternion.identity);
        newBall1.GetComponent<Ball>().Launch();
        
        var newBall2 = Instantiate(prefabBall, currentBall.position, Quaternion.identity);
        newBall2.GetComponent<Ball>().Launch();

        yield return new WaitForSeconds(15f);

        // Limpiamos las pelotas creadas
        if (newBall1 != null) Destroy(newBall1.gameObject);
        if (newBall2 != null) Destroy(newBall2.gameObject);

        // Ya terminó el poder, ahora sí destruimos este power-up
        Destroy(gameObject);
    }

    IEnumerator ExtraSpeedCoroutine() 
    { 
        Ball scriptPelota = currentBall.GetComponent<Ball>();
        
        if (scriptPelota != null)
        {
            scriptPelota.MultiplySpeed(2);
            yield return new WaitForSeconds(3f);
            scriptPelota.DivideSpeed(2);
        }

        // Ya terminó el poder, ahora sí destruimos este power-up
        Destroy(gameObject);
    }

    IEnumerator ExtraHealthCoroutine() 
    { 
        GameManager.instance.AddLife();

        yield return new WaitForSeconds(3f);

        // Ya terminó el poder, ahora sí destruimos este power-up
        Destroy(gameObject);
    }

    IEnumerator ExtraMundanityCoroutine() 
    { 
       
        yield return new WaitForSeconds(3f);

        // Ya terminó el poder, ahora sí destruimos este power-up
        Destroy(gameObject);
    }
}