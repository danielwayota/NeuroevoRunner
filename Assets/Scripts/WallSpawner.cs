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

    private float currentSpawnTime;

    private float time = 0f;

    // Random spawn point
    private int lastRandomSpawnIndex = -1;

    private int wallCount;

    // ========================================
    void Start()
    {
        this.time = this.spawnTime;
        this.currentSpawnTime = this.spawnTime;
    }

    // ========================================
    void Update()
    {
        this.time += Time.deltaTime;

        if (this.time > this.currentSpawnTime)
        {
            this.time -= this.currentSpawnTime;

            // this.SpawnInRandomPoint();
            this.SpawnAlternate();
            // this.SpawnSequential();
            this.wallCount++;

            this.currentSpawnTime -= this.currentSpawnTime * this.spawnTimeReductionPercentage;
            this.currentSpawnTime = Mathf.Max(this.currentSpawnTime, this.minSpawnTime);
        }
    }

    // ========================================
    void SpawnAlternate()
    {
        if (this.wallCount % 2 == 0)
        {
            this.SpawnWall(this.spawnPoints[1].position);
        }
        else
        {
            this.SpawnWall(this.spawnPoints[0].position);
            this.SpawnWall(this.spawnPoints[2].position);
        }
    }

    // ========================================
    void SpawnSequential()
    {
        int index = this.wallCount % this.spawnPoints.Length;

        this.SpawnWall(this.spawnPoints[index].position);
    }

    // ========================================
    void SpawnInRandomPoint()
    {
        var pos = this.GetRandomSpawnPoint();
        this.SpawnWall(pos);
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
        this.currentSpawnTime = this.spawnTime;

        this.time = this.spawnTime;
        for (int i = 0; i < this.wallsHolder.childCount; i++)
        {
            Destroy(this.wallsHolder.GetChild(i).gameObject);
        }

        this.wallCount = 0;

        // this.spawnTime = this.spawnTime - (this.spawnTime * this.spawnTimeReductionPercentage);

        // this.spawnTime = Mathf.Max(this.spawnTime, this.minSpawnTime);
    }
}