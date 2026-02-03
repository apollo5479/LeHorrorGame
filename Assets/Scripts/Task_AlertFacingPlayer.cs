using UnityEngine;
using TMPro;

public class Task_AlertFacingPlayer : MonoBehaviour
{
    [Header("Facing")]
    public float rotateSpeed = 180f;

    [Header("Bobbing")]
    public float bobHeight = 0.25f;
    public float bobSpeed = 2f;

    [Header("Scaling")]
    public float minScale = 0.9f;
    public float maxScale = 1.1f;
    public float scaleSpeed = 2f;

    private Transform player;
    private Vector3 startPosition;
    private Vector3 startScale;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("No GameObject with tag 'Player' found!");

        startPosition = transform.position;
        startScale = transform.localScale;
    }

    void Update()
    {
        if (player == null) return;

        FacePlayer();
        Bob();
        Pulse();
    }

    void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 180f, 0f);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    void Bob()
    {
        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        Vector3 targetPos = startPosition + Vector3.up * bobOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * bobSpeed
        );
    }

    void Pulse()
    {
        float t = (Mathf.Sin(Time.time * scaleSpeed) + 1f) / 2f;
        float scale = Mathf.Lerp(minScale, maxScale, t);

        Vector3 targetScale = startScale * scale;

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * scaleSpeed
        );
    }
}