using CerebroML.Genetics;
using UnityEngine;

public class Population : MonoBehaviour
{
    [Header("Stuff")]
    private WallSpawner wallSpawner;

    [Header("Runners stuff")]
    public GameObject runnerPrefab;
    public int targetCoins = 8;

    [Header("Population")]
    public int populationSize = 8;
    public float maxGenerationTime = 10f;

    [Header("Map points")]
    public Transform startPoint;

    private Runner[] runnerPopulation;

    private float checkTime = 0;
    private float checkTimeOut = 1;
    private float currentGenerationTime = 0;

    private int currentGeneration;


    // ========================================================
    void Start()
    {
        this.runnerPopulation = new Runner[this.populationSize];

        for (int i = 0; i < this.populationSize; i++)
        {
            GameObject e = Instantiate(this.runnerPrefab, this.startPoint.position, Quaternion.identity);
            e.transform.SetParent(this.transform);

            this.runnerPopulation[i] = e.GetComponent<Runner>();
        }
        this.currentGeneration = 1;
        // this.statusWindow.SetGenerationNumber(this.currentGeneration);
    }

    // ========================================================
    void Update()
    {
        this.checkTime += Time.deltaTime;

        if (this.checkTime > this.checkTimeOut)
        {
            this.checkTime -= this.checkTimeOut;

            if (this.IsEveryRunnerDead())
            {
                this.AdvanceGeneration();
                this.currentGenerationTime = 0;
            }
        }

        this.currentGenerationTime += Time.deltaTime;

        if (this.currentGenerationTime > this.maxGenerationTime)
        {
            this.AdvanceGeneration();
            this.currentGenerationTime -= this.maxGenerationTime;
        }
    }

    // ========================================================
    bool IsEveryRunnerDead()
    {
        bool allDead = true;

        for (int i = 0; i < this.runnerPopulation.Length; i++)
        {
            if (!this.runnerPopulation[i].isDead)
            {
                allDead = false;
            }
        }
        return allDead;
    }

    // ========================================================
    void AdvanceGeneration()
    {
        this.wallSpawner.Restart();
        this.currentGeneration++;
        // this.statusWindow.SetGenerationNumber(this.currentGeneration);

        // Generate new population
        Genome[] newGenomes = new Genome[this.populationSize];
        Genome[] parents = new Genome[2];

        for (int i = 0; i < this.populationSize; i++)
        {
            int parentCount = 0;

            int safety = 0;
            while (parentCount < 2 && safety < 10000)
            {
                safety++;
                int index = Random.Range(0, this.populationSize);
                Runner candidate = this.runnerPopulation[index];
                float dice = Random.Range(0f, 1f);

                // Calculate fitness
                if (this.CalculateRunnerFitness(candidate) > dice)
                {
                    parents[parentCount] = candidate.brain.GetGenome();
                    parentCount++;
                }
            }

            if (parents.Length == 2)
            {
                // Crossover
                Genome g1 = parents[0];
                Genome g2 = parents[1];

                Genome offspring = Genome.Crossover(g1, g2);
                offspring.Mutate(0.05f, 10f);

                newGenomes[i] = offspring;
            }
            else
            {
                Debug.LogError("No parents found!");
            }
        }

        // Hail to the new population!
        for (int i = 0; i < this.populationSize; i++)
        {
            this.runnerPopulation[i].brain.SetGenome(newGenomes[i]);
            this.runnerPopulation[i].Restart(this.startPoint.position);
        }
    }

    // ========================================================
    float CalculateRunnerFitness(Runner e)
    {
        float points = (e.aliveTime / this.currentGenerationTime) * 0.75f;

        float coinPoints = Mathf.Clamp01(e.coins / (float)this.targetCoins) * 0.25f;
        points += coinPoints;

        return Mathf.Pow(points, 2);
    }
}
