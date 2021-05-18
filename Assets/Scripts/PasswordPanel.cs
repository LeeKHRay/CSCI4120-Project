using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PasswordPanel : MonoBehaviour, IInteractableObject
{
    public ChargePlatform chargePlatform;
    public GameObject passwordPanel;
    public Text[] inputs;
    public Button[] increaseButtons;
    public Button[] decreaseButtons;
    public Button enterButton;
    public Text attemptsText;
    public GameObject inputRecord;
    public Transform inputHistory;

    public GameObject[] walls;
    public GameObject bridge;
    public Animator animator;

    private PlayerController player;
    private int[] digits = new int[] { 0, 0, 0, 0 };
    private int[] password = new int[4];
    private bool[] correctDigit = new bool[] { false, false, false, false };
    private int attempts = 6;
    private bool hacked = false;
    private bool panelOpened = false;

    void Start()
    {
        GeneratePassword();
    }

    public void Interact(PlayerController player)
    {
        if (!hacked && chargePlatform.charged)
        {
            this.player = player;
            player.canMove = panelOpened;

            // open password panel
            panelOpened = !panelOpened;
            passwordPanel.SetActive(panelOpened);
            Cursor.lockState = panelOpened ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = panelOpened;
        }
    }

    // callback for IncreaseButton
    public void IncreaseDigit(int idx)
    {
        if (!correctDigit[idx])
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
        if (!correctDigit[idx])
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

    // callback for EnterButton
    public void Enter()
    {
        if (Enumerable.SequenceEqual(digits, password))
        {
            hacked = true;
            for (int i = 0; i < 4; i++)
            {
                correctDigit[i] = true;
                inputs[i].color = Color.green;
            }

            player.canMove = true;
            panelOpened = false;
            passwordPanel.SetActive(panelOpened);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = panelOpened;

            animator.SetBool("Rise", false);
            bridge.SetActive(true);
            walls[0].SetActive(false);
            walls[1].SetActive(false);

        }
        else
        {
            bool[] checkedPasswordDigit = new bool[4] { false, false, false, false };

            // mark all incorrect digits as red
            for (int i = 0; i < 4; i++)
            {
                if (!correctDigit[i])
                {
                    inputs[i].color = Color.red;
                }
            }

            // mark correct digits as green
            for (int i = 0; i < 4; i++)
            {
                if (!correctDigit[i])
                {
                    if (digits[i] == password[i])
                    {
                        correctDigit[i] = true;
                        checkedPasswordDigit[i] = true;
                        inputs[i].color = Color.green;
                    }
                }
            }

            // mark wrong position digits
            for (int i = 0; i < 4; i++)
            {
                if (!correctDigit[i])
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (i != j && !checkedPasswordDigit[j] && !correctDigit[j])
                        {
                            if (digits[i] == password[j])
                            {
                                checkedPasswordDigit[j] = true;
                                inputs[i].color = Color.yellow;
                                break;
                            }
                        }
                    }
                }
            }

            AddInputRecord();

            attempts--;
            if (attempts <= 0)
            {
                attempts = 6;
                GeneratePassword();
                ResetInputs();
                ClearInputHistory();
            }
            string attemptsStr = attemptsText.text;
            attemptsText.text = attemptsStr.Substring(0, attemptsStr.Length - 1) + attempts;
        }
    }

    private void AddInputRecord()
    {
        Transform inputRecordTransform = Instantiate(inputRecord, inputHistory).transform;
        for (int i = 0; i < 4; i++)
        {
            Text inputRecordDigit = inputRecordTransform.GetChild(i).GetComponent<Text>();
            inputRecordDigit.text = inputs[i].text;
            inputRecordDigit.color = inputs[i].color;
        }
    }

    private void GeneratePassword()
    {
        for (int i = 0; i < 4; i++)
        {
            password[i] = Random.Range(0, 9);
        }
    }

    private void ResetInputs()
    {
        for (int i = 0; i < 4; i++)
        {
            inputs[i].text = "0";
            inputs[i].color = Color.black;
        }
        digits = new int[] { 0, 0, 0, 0 };
        correctDigit = new bool[] { false, false, false, false };
    }

    private void ClearInputHistory()
    {
        foreach (Transform record in inputHistory)
        {
            Destroy(record.gameObject);
        }
    }
}
