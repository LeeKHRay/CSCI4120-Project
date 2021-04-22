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
    public Color[] colors;

    private Image lifePoint;
    private Text lifePointText;
    private Image energy;
    private Text energyText;

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

        laserEnergyBar.value = weapon.ammoNum();
        energyText.text = weapon.ammoInfo();
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
