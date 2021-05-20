using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ColorPasswordPanel : MonoBehaviour, IInteractableObject
{
    public ColorSwitcher[] colorSwitchers;
    public ChargePlatform[] chargePlatforms;
    public Door door;

    private Color[] colors = new Color[] { Color.white, Color.red, Color.green, Color.blue, Color.yellow, Color.black };

    private MeshRenderer[] meshRenderers;
    private Light[] lights;

    private bool hacked = false;
    private bool charged = false;
    private int[] password = new int[] { -1, -1, -1, -1, -1, -1 };
    private Color[] passwordColors = new Color[6];


    void Start()
    {
        meshRenderers = colorSwitchers.Select(colorSwitcher => colorSwitcher.meshRenderer).ToArray();
        lights = colorSwitchers.Select(colorSwitcher => colorSwitcher.light).ToArray();
    }

    void Update()
    {
        if (!hacked)
        {
            if (!charged && chargePlatforms[0].charged && chargePlatforms[1].charged)
            {
                charged = true;
                GeneratePassword();
                StartCoroutine("ShowPassword");
            }
            else if (!chargePlatforms[0].charged || !chargePlatforms[1].charged)
            {
                StopCoroutine("ShowPassword");
                charged = false;
                password = new int[] { -1, -1, -1, -1, -1, -1 };
                TurnOffLight();
            }
        }
    }

    public void Interact(PlayerController player)
    {
        if (!hacked)
        {
            CheckPassword();
        }
    }

    public void CheckPassword()
    {
        int[] colorIndices = colorSwitchers.Select(colorSwitcher => colorSwitcher.colorIdx).ToArray();
        if (Enumerable.SequenceEqual(colorIndices, password))
        {
            hacked = true;
            StopCoroutine("CountDown");
            door.Open();
        }
    }

    private IEnumerator ShowPassword()
    {
        for (int i = 0; i < password.Length; i++)
        {
            meshRenderers[i].materials[1].SetColor("_EmissionColor", colors[password[i]]);
            lights[i].color = colors[password[i]];
        }
        yield return new WaitForSeconds(5);
        TurnOffLight();
        StartCoroutine("CountDown");
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(30);
        password = new int[] { -1, -1, -1, -1, -1, -1 };
        TurnOffLight();
    }

    private void GeneratePassword()
    {
        for (int i = 0; i < password.Length; i++)
        {
            password[i] = Random.Range(0, colors.Length - 2);
            passwordColors[i] = colors[Random.Range(0, colors.Length - 2)];
        }
    }

    private void TurnOffLight()
    {
        for (int i = 0; i < password.Length; i++)
        {
            colorSwitchers[i].colorIdx = 4;
            meshRenderers[i].materials[1].SetColor("_EmissionColor", colors[5]);
            lights[i].color = colors[5];
        }
    }
}
