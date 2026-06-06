using UnityEngine;

public class Refill : MonoBehaviour
{
    private bool _isConsumed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isConsumed) return;
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerMovement>().RefillFluid(50f);
            _isConsumed = true;
            Destroy(gameObject);
        }
    }
}
