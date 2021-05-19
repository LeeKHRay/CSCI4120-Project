using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePic : MonoBehaviour
{
    public Transform[] pictures;
    public GameObject wintext;

    public static bool gamewin;
    private bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        wintext.SetActive(false);
        gamewin = false;
    }

    bool CheckWin()
    {
        for (int i = 0; i < pictures.Length; i++)
        {
            if (pictures[i].rotation.z != 0)
                return false;
        }
        return true;
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        gamewin = CheckWin();
        if (wintext != null && gamewin && first)
        {
            wintext.SetActive(true);
            first = false;
        }
        
    }
}
