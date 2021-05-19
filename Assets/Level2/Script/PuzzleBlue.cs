using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleBlue : MonoBehaviour
{
    public Button[] controls;
    public bool[] status;
    public GameObject parent;
    private bool gameEnd = false;
    public LevelOneDoor access;

    // Start is called before the first frame update
    void Start()
    {
        controls = parent.GetComponentsInChildren<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnd)
        {
            StartCoroutine("TerminateGame");
        }
        
    }

    IEnumerator TerminateGame()
    {
        yield return new WaitForSeconds(2.0f);
        access.Open();
        CloseUI();
    }

    bool CheckEnd()
    {
        for (int i = 0; i < controls.Length; i++)
        {
            if (!status[i])
                return false;
        }
        return true;
    }

    public void ChangeColor(int ans)
    {
        if (!status[ans])
        {
            ColorBlock colors = controls[ans].colors;
            colors.normalColor = Color.red;
            colors.highlightedColor = new Color32(255, 100, 100, 255);
            colors.selectedColor = Color.red;
            controls[ans].colors = colors;
        }
        else
        {
            ColorBlock colors = controls[ans].colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color32(225, 225, 225, 255);
            colors.selectedColor = Color.white;
            controls[ans].colors = colors;
        }
        status[ans] = !status[ans];
    }

    public void UpdateButton(int t)
    {
        if (!gameEnd)
        {
            int i = t / 10;
            int j = t % 10;
            int[] row = { -1, 0, 0, 0, 1 };
            int[] col = { 0, -1, 0, 1, 0 };
            for (int e = 0; e < 5; e++)
            {
                int x = i + row[e];
                int y = j + col[e];
                if (x >= 0 && x < 4 && y >= 0 && y < 4)
                {
                    int ans = x * 4 + y;
                    ChangeColor(ans);
                }
            }
            gameEnd = CheckEnd();
        }
    }

    public void Resetall()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int ans = i * 4 + j;
                ColorBlock colors = controls[ans].colors;
                colors.normalColor = Color.white;
                colors.highlightedColor = new Color32(225, 225, 225, 255);
                colors.selectedColor = Color.white;
                controls[ans].colors = colors;
                status[ans] = false;
            }
        }
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

}
