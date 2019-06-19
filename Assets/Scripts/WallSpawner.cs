using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public GameObject wallPrefab;

    [Header("Spawn points")]
    public Transform[] spawnPoints;

    [Header("Anchor points")]
    public Transform wallsHolder;

    [Header("Spawn timers")]
    public float spawnTime = 1f;
    public float minSpawnTime = 1f;
    public float spawnTimeReductionPercentage = 0.05f;

    private float time = 0f;

    private int lastRandomSpawnIndex = -1;

    // ========================================
    void Start()
    {
        this.time = this.spawnTime;    
    }

    // ========================================
    void Update()
    {
        this.time += Time.deltaTime;

        if (this.time > this.spawnTime)
        {
            this.time -= this.spawnTime;

            var pos = this.GetRandomSpawnPoint();
            this.SpawnWall(pos);
        }
    }

    // ========================================
    Vector3 GetRandomSpawnPoint()
    {
        int index = this.lastRandomSpawnIndex;
        while(index == this.lastRandomSpawnIndex)
        {
            index = Random.Range(0, this.spawnPoints.Length);
        }
        this.lastRandomSpawnIndex = index;
        var pointTransfom = this.spawnPoints[index];

        return pointTransfom.position;
    }

    // ========================================
    void SpawnWall(Vector3 position)
    {
        GameObject newWall = Instantiate(this.wallPrefab, position, Quaternion.identity);
        newWall.transform.parent = this.wallsHolder;
    }

    // ========================================
    public void Restart()
    {
        this.time = this.spawnTime;
        for (int i = 0; i < this.wallsHolder.childCount; i++)
        {
            Destroy(this.wallsHolder.GetChild(i).gameObject);
        }

        this.spawnTime = this.spawnTime - (this.spawnTime * this.spawnTimeReductionPercentage);

        this.spawnTime = Mathf.Max(this.spawnTime, this.minSpawnTime);
    }
}