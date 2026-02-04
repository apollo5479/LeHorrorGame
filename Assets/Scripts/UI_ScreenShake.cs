using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ScreenShake : MonoBehaviour
{
    [Header("Menu Shake")]
    public float menuMagnitude = 0.05f;
    public float menuSpeed = 0.5f;

    [Header("Gameplay Shake")]
    public float gameMagnitude = 0.25f;
    public float gameSpeed = 20f;
    public float gameShakeDuration = 0.3f;

    private float shakeTimer;
    private float currentMagnitude;
    private float currentSpeed;
    private bool infiniteShake;

    private Vector3 originalLocalPos;
    private Scene currentScene;

    void Start()
    {
        originalLocalPos = transform.localPosition;
        currentScene = SceneManager.GetActiveScene();

        if (IsMenuScene())
        {
            infiniteShake = true;
            currentMagnitude = menuMagnitude;
            currentSpeed = menuSpeed;
        }
    }

    void LateUpdate()
    {
        if (!infiniteShake && shakeTimer <= 0f)
        {
            transform.localPosition = originalLocalPos;
            return;
        }

        float x = (Mathf.PerlinNoise(Time.time * currentSpeed, 0f) - 0.5f) * 2f * currentMagnitude;
        float y = (Mathf.PerlinNoise(0f, Time.time * currentSpeed) - 0.5f) * 2f * currentMagnitude;

        Vector3 shakeOffset = new Vector3(x, y, 0f);
        transform.localPosition = originalLocalPos + shakeOffset;

        if (!infiniteShake)
            shakeTimer -= Time.deltaTime;
    }

    public void TriggerGameShake()
    {
        infiniteShake = false;
        shakeTimer = gameShakeDuration;
        currentMagnitude = gameMagnitude;
        currentSpeed = gameSpeed;
    }

    bool IsMenuScene()
    {
        return currentScene.name == "MainMenu"
            || currentScene.name == "WinGame"
            || currentScene.name == "EndGame";
    }
}
