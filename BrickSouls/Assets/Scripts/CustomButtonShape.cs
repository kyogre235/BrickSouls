using UnityEngine;
using UnityEngine.UI; 

[RequireComponent(typeof(Image))]
public class CustomButtonShape : MonoBehaviour
{
    [Tooltip("Ignorará los clics en zonas con transparencia menor a este valor (0.1 = 10% opaco)")]
    public float alphaThreshold = 0.1f;

    void Start()
    {
        // Obtenemos el componente Image de nuestro botón
        Image buttonImage = GetComponent<Image>();
        
        // Le decimos a Unity que ignore los clics en las partes transparentes
        buttonImage.alphaHitTestMinimumThreshold = alphaThreshold;
    }
}
