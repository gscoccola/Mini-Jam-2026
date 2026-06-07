using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float _walkSFXCooldown = 0.5f;
    private float _walkSFXTimer;
    private bool _playingExpelSFX;

    [Header("References")]
    [SerializeField] private Animator _bodyAnimator;
    [SerializeField] private Animator _faceAnimator;
    private SpriteRenderer _bodyRenderer;
    private SpriteRenderer _faceRenderer;
    private AudioSource _audioSource;

    [Header("Debug")]
    [SerializeField][ReadOnly] private PlayerFaceState _faceState;
    [SerializeField][ReadOnly] private int _bodySize = 2;

    private void Awake()
    {
        _bodyRenderer = _bodyAnimator.GetComponent<SpriteRenderer>();
        _faceRenderer = _faceAnimator.GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0f;
        _bodyAnimator.SetInteger("Size", 2);
    }

    private void Update()
    {
        _walkSFXTimer += Time.deltaTime;
        if (_faceState == PlayerFaceState.Walking && _walkSFXTimer >= _walkSFXCooldown)
        {
            SFXManager.Instance.PlaySFX(SFXManager.Instance.PlayerWalk, true);
            _walkSFXTimer = 0f;
        }

        if (_faceState == PlayerFaceState.Shooting)
        {
            if (_audioSource.volume < 0.87f) SFXManager.Instance.PlaySFX(SFXManager.Instance.PlayerExpelStart, true);
            _audioSource.volume = 1f;
        }
            
            
        else _audioSource.volume -= Time.deltaTime * 1.4f;

        /*if (_faceState == PlayerFaceState.Shooting)
        {
            if (!_playingExpelSFX)
            {
                SFXManager.Instance.PlayContinuousSFX(SFXManager.Instance.PlayerExpelCont, "PlayerExpel");
                SFXManager.Instance.PlaySFX(SFXManager.Instance.PlayerExpelStart, true);
                _playingExpelSFX = true;
            }
        }
        else if (_playingExpelSFX)
        {
            SFXManager.Instance.StopContinuousSFX("PlayerExpel");
            _playingExpelSFX = false;
        }*/

    }

    public void SetFaceAnimation(PlayerFaceState state)
    {
        if (_faceState == state) return;
        _faceState = state;
        _faceAnimator.SetInteger("State", (int)state);
    }

    public void SetBodyAnimation(int size, float rest)
    {
        _bodyRenderer.transform.localScale = Vector3.one * rest;
        if (_bodySize == size) return;
        _bodySize = size;
        _bodyAnimator.SetInteger("Size", size);
    }

    public void Die()
    {
        _bodyRenderer.enabled = false;
        _faceRenderer.enabled = false;
    }
}


public enum PlayerFaceState
{
    Idle,
    Walking,
    Shooting,
    Holding,
}