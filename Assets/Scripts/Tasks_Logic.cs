using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class Tasks_Logic : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource taskProgress;
    public AudioSource stressMonsterAnger;
    public AudioSource presentation;
    public AudioSource breatheIn;

    [Header("Tasks")]
    public bool[] tasksCompleted = { false, false, false, false };

    public int papersCollected = 10;
    public int classesWentTo = 4;
    public int presentationsDone = 1;

    [Header("Presentation Gameplay")]
    public float windowToBreathe = 0.7f;

    [SerializeField] private GameObject breathePrompt;
    private ui_manager UI_script;
    private int breathsDone = 0;
    private bool didYouInhale = false;
    private bool presenting = false;

    void Start() {
        GameObject target = GameObject.Find("UI_Manager");
        breathePrompt.SetActive(false);
        UI_script = target.GetComponent<ui_manager>();
        // right now it's only spawning tasks at the start. but maybe the player can't finish them all so need to respawn some as time goes on
    }

    void Update()
    {
        if (presenting && Input.GetKeyDown(KeyCode.B))
        {
            didYouInhale = true;
            if (!breatheIn.isPlaying)
                breatheIn.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskModel")) {
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Paper"))
        {
            Debug.Log("Paper hit");
            Destroy(other.gameObject);

            if (papersCollected > 0)
            {
                papersCollected--;
            }
            else
            {
                CompleteTask(0);
            }
        }

        if (other.CompareTag("Classes"))
        {
            Debug.Log("Class Visited");
            Destroy(other.gameObject);

            if (classesWentTo > 0)
            {
                classesWentTo--;
            }
            else
            {
                CompleteTask(1);
            }
        }

        if (other.CompareTag("Presentation") && !presenting)
        {
            Destroy(other.gameObject);
            StartCoroutine(HandlePresentation());
            // TODO turn off player movement
        }
    }

    IEnumerator HandlePresentation()
    {
        presenting = true;
        breathsDone = 0;

        presentation.Play();

        while (breathsDone < 5 && presenting)
        {
            didYouInhale = false;

            yield return new WaitForSeconds(Random.Range(1f, 4f));
            breathePrompt.SetActive(true);
            Debug.Log("BREATHE NOW");

            yield return new WaitForSeconds(windowToBreathe);
            breathePrompt.SetActive(false);

            if (!didYouInhale)
            {
                presenting = false;
                // TODO turn on player movement
                stressMonsterAngry();
                yield break;
            }

            breathsDone++;
        }

        presenting = false;
        // TODO player movement back on
        if (presentationsDone > 0)
        {
            presentationsDone--;
        }
        else
        {
            CompleteTask(2);
        }
    }

    void CompleteTask(int taskIndex)
    {
        bool gameDone = true;
        taskProgress.Play();
        tasksCompleted[taskIndex] = true;
        foreach (bool x in tasksCompleted) {
            if (x == false) {
                gameDone = false;
                break;
            }
        }
        if (gameDone) 
        {
            SceneManager.LoadScene("WinGame");
        }
        UI_script.completed_task();
    }

    void stressMonsterAngry()
    {
        stressMonsterAnger.Play();
        // TODO: monster enters aggressive tracking mode
    }
}
