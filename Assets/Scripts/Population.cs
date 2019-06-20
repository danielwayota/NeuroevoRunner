using CerebroML.Genetics;
using UnityEngine;

public class Population : MonoBehaviour
{
    [Header("Stuff")]
    public WallSpawner wallSpawner;
    public GenerationPanel genPanel;

    [Header("Runners stuff")]
    public GameObject runnerPrefab;
    public int targetCoins = 8;

    [Header("Population")]
    public int populationSize = 8;

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
        this.genPanel.SetGenerationNumber(this.currentGeneration);
    }

    // ========================================================
    void Update()
    {
        this.checkTime += Time.deltaTime;
        this.currentGenerationTime += Time.deltaTime;

        if (this.checkTime > this.checkTimeOut)
        {
            this.checkTime -= this.checkTimeOut;

            if (this.IsEveryRunnerDead())
            {
                this.AdvanceGeneration();
                this.currentGenerationTime = 0;
            }
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
        this.genPanel.SetGenerationNumber(this.currentGeneration);

        // Generate new population
        Genome[] newGenomes = new Genome[this.populationSize];
        Genome[] parents = new Genome[2];

        // Get all the fitness normalized
        float[] fitnessList = new float[this.populationSize];

        for (int i = 0; i < this.populationSize; i++)
        {
            float normalizedFitness = this.runnerPopulation[i].aliveTime / this.currentGenerationTime;

            fitnessList[i] = Mathf.Pow(normalizedFitness, 2);
        }

        // New population
        for (int i = 0; i < this.populationSize; i++)
        {
            int parentCount = 0;

            int safety = 0;
            while (parentCount < 2 && safety < 10000)
            {
                safety++;

                int index = Random.Range(0, this.populationSize);

                Runner candidate = this.runnerPopulation[index];
                float candidateFitness = fitnessList[index];

                float dice = Random.Range(0f, 1f);

                // Calculate fitness
                if (candidateFitness > dice)
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
}
