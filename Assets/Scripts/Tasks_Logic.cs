using UnityEngine;
using System.Collections;

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

    private int breathsDone = 0;
    private bool didYouInhale = false;
    private bool presenting = false;

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
            // TODO: Show "BREATHE IN" UI prompt
            Debug.Log("BREATHE NOW");

            yield return new WaitForSeconds(windowToBreathe);
            // TODO: Hide UI prompt

            if (!didYouInhale)
            {
                presenting = false;
                stressMonsterAngry();
                yield break;
            }

            breathsDone++;
        }

        presenting = false;

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
        taskProgress.Play();
        tasksCompleted[taskIndex] = true;
        // TODO: Update task UI
    }

    void stressMonsterAngry()
    {
        stressMonsterAnger.Play();
        // TODO: monster enters aggressive tracking mode
    }
}
