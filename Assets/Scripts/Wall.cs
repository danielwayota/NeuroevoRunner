using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Wall : MonoBehaviour
{
    public float speed = 2;

    private Rigidbody body;

    void Start()
    {
        this.body = GetComponent<Rigidbody>();
        StartCoroutine(this.KeepRunning());
    }

    IEnumerator KeepRunning()
    {
        while(true)
        {
            this.body.velocity = -this.transform.forward * this.speed;
            yield return new WaitForSeconds(2f);
        }
    }
}