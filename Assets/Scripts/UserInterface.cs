using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public PlayerController player;
    public LaserGun laserGun;
    public Slider lifePointBar;
    public Slider laserEnergyBar;
    public RawImage[] energyCellImages;
    public Color[] colors;

    private Image lifePoint;
    private Image energy;
    private int energyCellNum = 3;

    void Start()
    {
        lifePoint = lifePointBar.transform.GetChild(2).GetComponent<Image>();
        energy = laserEnergyBar.transform.GetChild(2).GetComponent<Image>();
    }

    void Update()
    {
        lifePointBar.value = player.lifePoint;

        laserEnergyBar.value = laserGun.GetEnergy();

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

    public void Retry()
    {
        SceneManager.LoadScene("TheRebellionofRobotSoldier");
    }
}
