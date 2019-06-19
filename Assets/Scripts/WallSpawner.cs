using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public GameObject wallPrefab;

    [Header("Spawn points")]
    public Transform[] spawnPoints;

    [Header("Anchor points")]
    public Transform wallsHolder;

    public float spawnTime = 1f;

    private float time = 0f;

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
        int index = Random.Range(0, this.spawnPoints.Length);
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
        this.time = 0;
        for (int i = 0; i < this.wallsHolder.childCount; i++)
        {
            Destroy(this.wallsHolder.GetChild(i).gameObject);
        }
    }
}