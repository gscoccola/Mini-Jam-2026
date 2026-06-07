using UnityEngine;
using System.Collections.Generic;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private List<PlayerFollower> _followers = new List<PlayerFollower>();
    private Transform _playerTransform;
    private void Awake()
    {
        _playerTransform = FindAnyObjectByType<PlayerMovement>().transform;
        foreach (var follower in _followers)
        {
            follower.Initialize();
        }
    }

    private void LateUpdate()
    {
        foreach (var follower in _followers)
        {
            follower.UpdatePosition(_playerTransform);
        }
    }
}

[System.Serializable]
public class PlayerFollower
{
    public Transform Transform;
    public float DisplacementRatio;
    private float InitialX;

    public void Initialize()
    {
        InitialX = Transform.position.x;
    }

    public void UpdatePosition(Transform playerTransform)
    {
        float x = (1f - DisplacementRatio) * playerTransform.position.x + InitialX;
        Transform.position = new Vector3(x, Transform.position.y, Transform.position.z);
    }
}
