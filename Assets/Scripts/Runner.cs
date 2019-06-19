using UnityEngine;

using CerebroML;
using CerebroML.Factory;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Runner : MonoBehaviour
{
    public float movementSpeed = 1f;
    public float fovInDegrees = 10f;
    public float viewDistance = 10f;

    public LayerMask wallsLayer;

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

    Vector3[] lookDirections;


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

            seeLeft
            wx
            wy

            seeRight
            wx
            wy

         */
        // Move to other place
        this.brain = Factory.Create()
            .WithInput(17)
            .WithLayer(4, LayerType.Tanh)
            .WithLayer(1, LayerType.Tanh)
            .Build();

        this.body = GetComponent<Rigidbody>();

        this.Restart(this.transform.position);

        this.GenerateLookDirections();
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
    void GenerateLookDirections()
    {
        this.lookDirections = new Vector3[5];

        Vector3 front = this.transform.forward;
        float baseAngle = Mathf.Atan2(front.z, front.x);

        int index = 0;
        // Calculate the base angle and the offset for the three eyes
        for (int i = -1; i <= 1; i++)
        {
            float angle = baseAngle + (this.fov * i);
            Vector3 lookDir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

            this.lookDirections[index] = lookDir;
            index++;
        }

        // Left eye
        this.lookDirections[index] = -this.transform.right;
        index++;
        // Right eye
        this.lookDirections[index] = this.transform.right;
        index++;
    }

    // ============================================
    float Decide()
    {
        List<float> sensorData = new List<float>();

        // First, insert the runner position into the sensor data
        Vector3 position = this.transform.position;

        sensorData.Add(position.x);
        sensorData.Add(position.z);

        
        Vector3 front = this.transform.forward;
        float baseAngle = Mathf.Atan2(front.z, front.x);

        // Detect what each eye 'sees' and put the data in the sensor
        // If nothing is seen, just put 0
        foreach (var lookDir in this.lookDirections)
        {
            RaycastHit hit;
            // 'see' is the way to indicate the type of object the eye is seeing
            //  0: nothing
            //  1: coin
            // -1: wall
            float see = 0;

            // The seen object position
            float seeX = 0;
            float seeZ = 0;

            // Three front eyes
            if (Physics.Raycast(position, lookDir, out hit, this.viewDistance, this.wallsLayer))
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
                seeX = hit.point.x;
                seeZ = hit.point.z;

                Debug.DrawLine(position, position + lookDir * this.viewDistance, Color.red, 1f );
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
        Wall w = other.gameObject.GetComponent<Wall>();

        if (w != null)
        {
            this.isDead = true;
            this.gameObject.SetActive(false);
        }
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
        this.body.velocity = Vector3.zero;
    }
}
