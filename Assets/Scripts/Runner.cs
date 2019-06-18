using UnityEngine;

using CerebroML;
using CerebroML.Factory;

public class Runner : MonoBehaviour
{
    public float movementSpeed = 1f;

    public Cerebro brain;

    private float time;
    private float tickTime = 0.5f;

    private Rigidbody body;

    public bool isDead
    {
        get; protected set;
    }

    // ============================================
    void Start()
    {
        // Move to other place
        this.brain = Factory.Create()
            .WithInput(6)
            .WithLayer(5, LayerType.Tanh)
            .WithLayer(4, LayerType.Tanh)
            .WithLayer(1, LayerType.Tanh)
            .Build();

        this.body = GetComponent<Rigidbody>();

        this.Restart(this.transform.position);
    }

    // ============================================
    void Update()
    {
        this.time += Time.deltaTime;

        if (this.time > this.tickTime)
        {
            this.time -= this.tickTime;

            this.Decide();
        }
    }

    // ============================================
    void Decide()
    {
        // TODO:
        Vector3 front = this.transform.forward;

        float baseAngle = Mathf.Atan2(front.z, front.x);
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
        this.gameObject.SetActive(true);
        this.body.interpolation = RigidbodyInterpolation.None;
        this.body.MovePosition(startposition);
    }
}
