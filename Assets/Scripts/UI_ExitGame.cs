using UnityEngine;

public class UI_ExitGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitGame() {
        Debug.Log("Quitting game...");
        Application.Quit(); // Quits the application
    }
}
