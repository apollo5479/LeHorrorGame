using UnityEngine;
using System.Collections.Generic;

public class Tasks_SpawnBehaviour : MonoBehaviour
{
    [Header("Objects to choose from")]
    public List<GameObject> objects = new List<GameObject>();

    [Header("How many should be active")]
    public int activeCount = 10;

    [Header("How much stress is this task?")]
    public float stress_value;

    private ui_manager UI_script;

    void Start()
    {
        GameObject target = GameObject.Find("UI_Manager");
        UI_script = target.GetComponent<ui_manager>();
        ActivateRandom();
    }

    public void ActivateRandom()
    {
        // Safety
        activeCount = Mathf.Clamp(activeCount, 0, objects.Count);

        // Turn everything off first
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }

        // Shuffle list
        Shuffle(objects);

        // Activate first N
        for (int i = 0; i < activeCount; i++)
        {
            UI_script.new_task(stress_value);
            Debug.Log("New task.");
            objects[i].SetActive(true);
        }

        Invoke(nameof(ActivateRandom), 180f);
    }

    void Shuffle(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            GameObject temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}