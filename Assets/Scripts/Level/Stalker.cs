using UnityEngine;

public class Stalker : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private StalkerStatus _currentStatus;
    private Vector2 _relativePosition => transform.position - _playerMovement.transform.position;

    private void Awake()
    {
        _playerMovement = FindAnyObjectByType<PlayerMovement>();
        _currentStatus = StalkerStatus.Idle;
    }

    private void Update()
    {
        switch (_currentStatus) {
            case StalkerStatus.Idle:
                IdleUpdate();
                break;
            case StalkerStatus.Hovering:
                HoveringUpdate();
                break;
            case StalkerStatus.Attacking:
                AttackingUpdate();
                break;
            case StalkerStatus.Stuck:
                StuckUpdate();
                break;
            case StalkerStatus.Retreating:
                RetreatingUpdate();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            InGameUI.Instance.TriggerGameOver();
        }
    }

    #region UpdateFunctions
    private void IdleUpdate()
    {
        if (_relativePosition.magnitude < 5f)
        {
            _currentStatus = StalkerStatus.Hovering;
        }
    }
    private void HoveringUpdate()
    {
        
    }
    private void AttackingUpdate()
    {
        
    }
    private void StuckUpdate()
    {
        
    }
    private void RetreatingUpdate()
    {
        
    }



    #endregion
}




public enum StalkerStatus
{
    Idle,
    Hovering,
    Attacking,
    Stuck,
    Retreating
}