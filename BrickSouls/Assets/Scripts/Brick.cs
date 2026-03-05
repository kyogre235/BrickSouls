using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public GameObject capsule;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            int posibility = Random.Range(0,10);
            if (posibility < 5)
            {
                Instantiate(capsule, this.transform.position, capsule.transform.rotation);
            }
            gameObject.SetActive(false);
            //GameManager.instance.BlockDestroy();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // 1. Cargamos todas las texturas de la carpeta Resources/Bricks
        Object[] texturas = Resources.LoadAll("Bricks", typeof(Texture2D));

        if (texturas.Length > 0)
        {
            // 2. Elegimos un índice al azar
            int indiceAzar = Random.Range(0, texturas.Length);
            Texture2D texturaElegida = (Texture2D)texturas[indiceAzar];

            // 3. La asignamos al material del Quad
            // Usamos "_MainTex" que es el nombre estándar en la mayoría de Shaders
            GetComponent<Renderer>().material.mainTexture = texturaElegida;
            
            Debug.Log("Textura seleccionada: " + texturaElegida.name);
        }
        else
        {
            Debug.LogError("¡No se encontraron texturas en Resources/Bricks!");
        }
        //Color randomColor = new Color(Random.value, Random.value, Random.value);
        //GetComponent<Renderer>().material.color = randomColor;
    }

    // Update is called once per frame
}
