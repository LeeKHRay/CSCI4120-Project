using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ReferencesScriptableObject references;

    public PlayerController player;
    public GameObject minionPrefab;
    public Transform[] spawnPoints;
    public Transform bossReturnPoint;
    public Boss boss;
    public GameObject[] minions;
    public GameObject gameOverMenu;
    public GameObject winMenu;
    public Door door;

    private bool spawned = false;
    private bool bossInScene = true;
    private GameObject[] spawnedMinions;
    private ParticleSystem[][] particleSystems;

    private bool isLose = false;
    private bool bossDead = false;

    void Awake()
    {
        SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("Level2", LoadSceneMode.Additive);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        particleSystems = new ParticleSystem[spawnPoints.Length + 1][];

        particleSystems[0] = new ParticleSystem[2];
        particleSystems[0][0] = bossReturnPoint.GetChild(0).GetComponent<ParticleSystem>();
        particleSystems[0][1] = bossReturnPoint.GetChild(1).GetComponent<ParticleSystem>();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            particleSystems[i + 1] = new ParticleSystem[2];
            particleSystems[i + 1][0] = spawnPoints[i].GetChild(0).GetComponent<ParticleSystem>();
            particleSystems[i + 1][1] = spawnPoints[i].GetChild(1).GetComponent<ParticleSystem>();
        }
    }

    void Update()
    {
        if (boss.lifePoint < 250 && bossInScene && !spawned)
        {
            StartCoroutine(SpawnMinions());
            bossInScene = false;
            boss.Escape();
        }

        if (!bossInScene && SpawnedMinionsDead())
        {
            StartCoroutine("BossReturn");
            bossInScene = true;
        }

        if (player.lifePoint <= 0 && !isLose)
        {
            ShowGameOverMenu();
        }

        if (boss.lifePoint <= 0 && !bossDead)
        {
            player.canMove = false;
            ShowWinMenu();
        }
    }

    private IEnumerator SpawnMinions()
    {
        spawnedMinions = new GameObject[4];
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            foreach (ParticleSystem ps in particleSystems[i + 1])
            {
                ps.Play();
            }
        }
        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnedMinions[i] = Instantiate(minionPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
        }

        spawned = true;
    }

    private bool SpawnedMinionsDead()
    {
        if (spawned)
        {
            foreach (GameObject minion in spawnedMinions)
            {
                if (minion != null)
                {
                    return false;
                }
            }
            Debug.Log("SpawnedMinionsDead");
            return true;
        }
        return false;
    }

    private IEnumerator BossReturn()
    {
        foreach (ParticleSystem ps in particleSystems[0])
        {
            ps.Play();
        }
        yield return new WaitForSeconds(2.0f);
        boss.transform.position = bossReturnPoint.position;
        boss.gameObject.SetActive(true);
        boss.Return();
    }

    private IEnumerator SpawnEnemies()
    {
        particleSystems[0][0].Play();
        particleSystems[0][1].Play();
        particleSystems[1][0].Play();
        particleSystems[1][1].Play();
        particleSystems[3][0].Play();
        particleSystems[3][1].Play();
        yield return new WaitForSeconds(2.0f);
        boss.gameObject.SetActive(true);
        minions[0].SetActive(true);
        minions[1].SetActive(true);
    }

    private void ShowGameOverMenu()
    {
        isLose = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameOverMenu.SetActive(true);
    }

    private void ShowWinMenu()
    {
        bossDead = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        winMenu.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene("Level3");
        /*#if UNITY_EDITOR
                SceneManager.LoadScene("Level2", LoadSceneMode.Additive);
                SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        #endif*/
    }

    public void Return()
    {
        SceneManager.LoadScene("StartMenu");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level3"));
            SceneManager.UnloadSceneAsync("Level1");
            SceneManager.UnloadSceneAsync("Level2");
            door.auto = false;
            door.Close();

            StartCoroutine("SpawnEnemies");
            Destroy(GetComponent<Collider>());
        }
    }
}
