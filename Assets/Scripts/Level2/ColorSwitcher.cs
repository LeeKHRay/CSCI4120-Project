using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitcher : MonoBehaviour, IInteractableObject
{
    public MeshRenderer meshRenderer;
    public Light light;
    public int colorIdx = 4;

    private Color[] colors = new Color[] { Color.white, Color.red, Color.green, Color.blue, Color.yellow, Color.black };

    public void Interact(PlayerController player)
    {
        colorIdx++;
        if (colorIdx > colors.Length - 2)
        {
            colorIdx = 0;
        }
        meshRenderer.materials[1].SetColor("_EmissionColor", colors[colorIdx]);
        light.color = colors[colorIdx];
    }
}
