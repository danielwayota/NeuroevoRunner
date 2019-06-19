using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Wall : MonoBehaviour
{
    public float speed = 2;

    void Start()
    {
        var body = GetComponent<Rigidbody>();

        body.velocity = -this.transform.forward * this.speed;
    }
}