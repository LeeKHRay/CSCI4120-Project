using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public PlayerController player;
    public Weapon weapon;
    public Slider lifePointBar;
    public Slider laserEnergyBar;
    public RawImage[] energyCellImages;
    public Color[] colors;

    private Image lifePoint;
    private Text lifePointText;
    private Image energy;
    private Text energyText;
    private int energyCellNum = 3;

    void Start()
    {
        lifePoint = lifePointBar.transform.GetChild(2).GetComponent<Image>();
        lifePointText = lifePointBar.transform.GetChild(3).GetComponent<Text>();
        energy = laserEnergyBar.transform.GetChild(2).GetComponent<Image>();
        energyText = laserEnergyBar.transform.GetChild(3).GetComponent<Text>();
    }

    void Update()
    {
        lifePointBar.value = player.lifePoint;
        lifePointText.text = player.lifePoint + "/300";

        laserEnergyBar.value = weapon.AmmoNum();
        energyText.text = weapon.AmmoInfo();

        // update energy cell images
        if (energyCellNum != player.energyCellNum)
        {
            energyCellNum = player.energyCellNum;
            for (int i = 0; i < 3; i++)
            {
                if (i < energyCellNum)
                {
                    energyCellImages[i].enabled = true;
                }
                else
                {
                    energyCellImages[i].enabled = false;
                }
            }
        }
    }

    public void ChangeLifePointColor()
    {
        if (lifePointBar.value > 150)
        {
            lifePoint.color = colors[0];
        }
        else if (lifePointBar.value > 50)
        {
            lifePoint.color = colors[1];
        }
        else
        {
            lifePoint.color = colors[2];
        }
    }

    public void ChangeLaserEnergyColor()
    {
        if (laserEnergyBar.value > 10)
        {
            energy.color = colors[0];
        }
        else if (laserEnergyBar.value > 3)
        {
            energy.color = colors[1];
        }
        else
        {
            energy.color = colors[2];
        }
    }
}
