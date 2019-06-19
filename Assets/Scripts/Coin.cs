using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Runner runner = other.GetComponent<Runner>();

        if (runner != null)
        {
            runner.coins += 1;
            Destroy(this.gameObject);
        }
    }
}