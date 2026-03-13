using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways] // Esto hace que el script funcione incluso sin darle al Play
[RequireComponent(typeof(Renderer))] // Asegura que el objeto tenga un renderizador
public class AutoTiling : MonoBehaviour
{
    // Puedes ajustar este multiplicador si los ladrillos se ven muy grandes o pequeños
    public float textureScale = 1f; 

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateTiling();
    }

    void Update()
    {
        // Si estamos en el editor (sin jugar), actualiza el Tiling constantemente 
        // para que veas los cambios mientras arrastras y escalas el Quad.
        if (!Application.isPlaying)
        {
            UpdateTiling();
        }
    }

    void UpdateTiling()
    {
        if (rend != null && rend.sharedMaterial != null)
        {
            // Tomamos la escala real del objeto en X e Y
            float scaleX = transform.lossyScale.x * textureScale;
            float scaleY = transform.lossyScale.y * textureScale;

            // Le aplicamos esa escala al Tiling del material
            rend.material.mainTextureScale = new Vector2(scaleX, scaleY);
        }
    }
}
