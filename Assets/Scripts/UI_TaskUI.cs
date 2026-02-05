using TMPro;
using System.Text;
using UnityEngine;

public class UI_TaskUI : MonoBehaviour
{
    [SerializeField] private TMP_Text taskText;
    [SerializeField] private UI_TaskTracker tracker;

    private void OnEnable()
    {
        tracker.OnTasksChanged += RefreshText;
        RefreshText();
    }

    private void OnDisable()
    {
        tracker.OnTasksChanged -= RefreshText;
    }

    void RefreshText()
    {
        StringBuilder sb = new StringBuilder();

        foreach (var task in tracker.tasks)
        {
            string line =
                $"{task.displayName}: [{task.current} / {task.required}]";

            if (task.current >= task.required)
            {
                line = $"<color=green>{line}</color>";
            }

            sb.AppendLine(line);
        }

        taskText.text = sb.ToString();
    }
}