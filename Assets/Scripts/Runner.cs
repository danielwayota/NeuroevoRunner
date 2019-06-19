using UnityEngine;

using CerebroML;
using CerebroML.Factory;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Runner : MonoBehaviour
{
    public float movementSpeed = 1f;
    public float fovInDegrees = 10f;

    public float aliveTime
    {
        get; protected set;
    }

    public int coins
    {
        get; set;
    }

    public bool isDead
    {
        get; protected set;
    }

    public Cerebro brain;

    private float time;
    private float tickTime = 0.5f;

    private float fov;

    private Rigidbody body;


    // ============================================
    void Start()
    {
        this.fov = Mathf.Deg2Rad * this.fovInDegrees;

        /**
            x
            y

            see1
            wx1
            wy1

            see1
            wx1
            wy1

            see1
            wx1
            wy1

         */
        // Move to other place
        this.brain = Factory.Create()
            .WithInput(11)
            .WithLayer(4, LayerType.Tanh)
            .WithLayer(2, LayerType.Tanh)
            .WithLayer(1, LayerType.Tanh)
            .Build();

        this.body = GetComponent<Rigidbody>();

        this.Restart(this.transform.position);
    }

    // ============================================
    void Update()
    {
        this.aliveTime += Time.deltaTime;

        this.time += Time.deltaTime;

        if (this.time > this.tickTime)
        {
            this.time -= this.tickTime;

            float direction = this.Decide();
            this.body.velocity = Vector3.right * direction * movementSpeed;
        }
    }

    // ============================================
    float Decide()
    {
        List<float> sensorData = new List<float>();

        // First, insert the runner position into the sensor data
        Vector3 position = this.transform.position;

        sensorData.Add(position.x);
        sensorData.Add(position.z);

        // Calculate the base angle and the offset for the three eyes
        Vector3 front = this.transform.forward;
        float baseAngle = Mathf.Atan2(front.z, front.x);

        // Detect what each eye 'sees' and put the data in the sensor
        // If nothing is seen, just put 0
        for (int i = -1; i <= 1; i++)
        {
            float angle = baseAngle + (this.fov * i);

            Vector3 raydirection = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

            RaycastHit hit;
            // 'see' is the way to indicate the type of object the eye is seeing
            //  0: nothing
            //  1: coin
            // -1: wall
            float see = 0;

            // The seen object position
            float seeX = 0;
            float seeZ = 0;

            if (Physics.Raycast(position, raydirection, out hit, 10))
            {
                GameObject go = hit.collider.gameObject;

                if (go.GetComponent<Coin>() != null)
                {
                    see = 1;
                }
                else if (go.GetComponent<Wall>() != null)
                {
                    see = -1;
                }
                seeX = go.transform.position.x;
                seeZ = go.transform.position.z;
            }
            sensorData.Add(see);
            sensorData.Add(seeX);
            sensorData.Add(seeZ);
        }

        return this.brain.Run(sensorData.ToArray())[0];
    }

    // ============================================
    private void OnCollisionEnter(Collision other)
    {
        this.isDead = true;
        this.gameObject.SetActive(false);
    }

    // ============================================
    // Public methods
    // ============================================
    public void Restart(Vector3 startposition)
    {
        this.isDead = false;
        this.aliveTime = 0;
        this.coins = 0;
        this.gameObject.SetActive(true);
        this.body.interpolation = RigidbodyInterpolation.None;
        this.body.MovePosition(startposition);
    }
}
