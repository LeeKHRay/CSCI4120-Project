using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePic : MonoBehaviour
{
    public Transform[] pictures;
    public GameObject key;

    public static bool gamewin;
    private bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        gamewin = false;
    }

    // Update is called once per frame
    void Update()
    {
        gamewin = CheckWin();
        if (key != null && gamewin && first)
        {
            key.SetActive(true);
            first = false;
        }

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
}
