using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelOnePassword : MonoBehaviour
{
    public Text[] inputs;
    public Button[] increaseButtons;
    public Button[] decreaseButtons;
    public Button enterButton;
    public Door door = null;

    private int[] digits = new int[] { 0, 0, 0, 0 };
    public int[] password = new int[] { 8, 3, 5, 1 };
    private bool pass = false;

    private bool first = true;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();   
    }

    // callback for IncreaseButton
    public void IncreaseDigit(int idx)
    {
        if (!pass)
        {
            digits[idx]++;
            if (digits[idx] > 9)
            {
                digits[idx] = 0;
            }
            inputs[idx].text = digits[idx] + "";
            inputs[idx].color = Color.black;
        }
    }

    // callback for DecreaseButton
    public void DecreaseDigit(int idx)
    {
        if (!pass)
        {
            digits[idx]--;
            if (digits[idx] < 0)
            {
                digits[idx] = 9;
            }
            inputs[idx].text = digits[idx] + "";
            inputs[idx].color = Color.black;
        }
    }

    public void Enter()
    {
        if (Enumerable.SequenceEqual(digits, password) && first)
        {
            first = false;
            pass = true;
            if (door != null)
                door.Open();
        }
    }

    public void CloseUI()
    {
        player.canMove = true;

        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
