using UnityEngine;
using System.Collections.Generic;

public class UI_TaskTracker : MonoBehaviour
{
    public List<UI_TaskProgress> tasks = new List<UI_TaskProgress>();

    public System.Action OnTasksChanged;

    public void IncrementTask(string taskName)
    {
        UI_TaskProgress task = tasks.Find(t => t.displayName == taskName);
        if (task == null) return;

        task.current = Mathf.Min(task.current + 1, task.required);
        OnTasksChanged?.Invoke();
    }
}