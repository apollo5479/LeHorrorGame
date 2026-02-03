using UnityEngine;
using System.Collections.Generic;

public class Tasks_SpawnBehaviour : MonoBehaviour
{
    [Header("Objects to choose from")]
    public List<GameObject> objects = new List<GameObject>();

    [Header("How many should be active")]
    public int activeCount = 10;

    void Start()
    {
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
            objects[i].SetActive(true);
        }
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