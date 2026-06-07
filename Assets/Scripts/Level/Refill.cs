using UnityEngine;

public class Refill : MonoBehaviour
{
    private bool _isConsumed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isConsumed) return;
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().RefillFluid(50f);
            _isConsumed = true;
            Destroy(gameObject);
        }
    }
}
