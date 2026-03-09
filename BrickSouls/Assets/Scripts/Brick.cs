using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
public GameObject capsule;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("BallClone"))
        {
            int posibility = Random.Range(0, 10);
            if (posibility < 5)
            {
                Instantiate(capsule, this.transform.position, capsule.transform.rotation);
            }
            gameObject.SetActive(false);
            GameManager.instance.BlockDestroy();
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Sprite[] sprites = Resources.LoadAll<Sprite>("Bricks/Level_1");

        if (sprites.Length > 0)
        {
            
            int indiceAzar = Random.Range(0, sprites.Length);
            Sprite spriteElegido = sprites[indiceAzar];

           
            GetComponent<SpriteRenderer>().sprite = spriteElegido;
            
            Debug.Log("Sprite seleccionado: " + spriteElegido.name);
        }
        else
        {
            Debug.LogError("¡No se encontraron sprites en Resources/Bricks/Level_1!");
        }
    }
}
