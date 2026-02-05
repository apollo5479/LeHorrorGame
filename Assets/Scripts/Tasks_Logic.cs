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
    private a_movement movementScript;
    private int breathsDone = 0;
    private bool didYouInhale = false;
    private bool presenting = false;

    void Start() {
        GameObject target = GameObject.Find("UI_Manager");
        breathePrompt.SetActive(false);
        UI_script = target.GetComponent<ui_manager>();
        target = GameObject.Find("Player");
        movementScript = target.GetComponent<a_movement>();
        // right now it's only spawning tasks at the start. but maybe the player can't finish them all so need to respawn some as time goes on
        // try referencing Task_SpawnBehaviour and it's activate random function every 3 minutes?, also instead of destroying tasks, just set active to false
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
            // .Play();
            Debug.Log("Paper hit");
            Destroy(other.gameObject);
            UI_script.gain_hp(4f);
            Debug.Log("You gained hp");
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
            UI_script.gain_hp(10f);
            Debug.Log("You gained hp");
            // .Play();
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
            UI_script.gain_hp(30f);
            Debug.Log("You gained hp");
            Destroy(other.gameObject);
            StartCoroutine(HandlePresentation());
            movementScript.toggleMovement(false);
        }
    }

    IEnumerator HandlePresentation()
    {
        Debug.Log("presentation 1");
        presenting = true;
        breathsDone = 0;

        presentation.Play();
        Debug.Log("presentation 2");
        while (breathsDone < 5 && presenting)
        {
            Debug.Log("presentation loop");
            didYouInhale = false;

            yield return new WaitForSeconds(Random.Range(1f, 4f));
            breathePrompt.SetActive(true);
            Debug.Log("BREATHE NOW");

            yield return new WaitForSeconds(windowToBreathe);
            breathePrompt.SetActive(false);

            if (!didYouInhale)
            {
                Debug.Log("Failed to breathe.");
                presenting = false;
                movementScript.toggleMovement(true);
                stressMonsterAngry();
                yield break;
            }

            breathsDone++;
        }

        presenting = false;
        movementScript.toggleMovement(true);
        UI_script.gain_hp(60f);
        Debug.Log("You gained hp");
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
        if (taskIndex == 0) { // paper
            UI_script.completed_task(20f);
        }
        if (taskIndex == 1) // class
        {
            UI_script.completed_task(20f);
        }
        if (taskIndex == 2) // presentation
        {
            UI_script.completed_task(60f);
        }
        if (taskIndex == 3) // TO DO
        {
            UI_script.completed_task(99f); // TO DO
        }
    }

    void stressMonsterAngry()
    {
        stressMonsterAnger.Play();
        // TODO: monster enters aggressive tracking mode
    }
}
