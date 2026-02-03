using UnityEngine;

public class Monster_SpawnLogic : MonoBehaviour
{

    public float spawnTimer = 15f;
    public GameObject objectToSpawn;
    public Vector3 whereToSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(spawnMonster), spawnTimer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnMonster() {
        Instantiate(objectToSpawn, whereToSpawn, objectToSpawn.transform.rotation);
    }
}
