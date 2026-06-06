using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class CanvasGroupUpdate : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _transitionDuration = 0.3f;
    [HideInInspector] public UnityEvent UpdateValues = new();

    private float _halfTransition;

    private void Awake()
    {
        _halfTransition = _transitionDuration / 2f;
    }

    public void Switch()
    {
        _canvasGroup.DOFade(0, _halfTransition).OnComplete(() =>
        {
            UpdateValues.Invoke();
            _canvasGroup.DOFade(1, _halfTransition);
        });
    }
}
