using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class TextAndIconSwitcher : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI[] _texts;
    [SerializeField] private Image[] _images;
    [SerializeField] private SpriteRenderer[] _spriteRenderer;

    public void Switch(string[] textReplacements, Sprite[] imageReplacements, Sprite[] spriteReplacements)
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.DOFade(0, 0.5f).OnComplete(() =>
            {
                UpdateValues(textReplacements, imageReplacements, spriteReplacements);
                _canvasGroup.DOFade(1, 0.5f);
            });
            return;
        }
}

    private void UpdateValues(string[] textReplacements, Sprite[] imageReplacements, Sprite[] spriteReplacements)
    {
        if (_texts != null)
        {
            UpdateTexts(textReplacements);
        }
        if (_images != null)
        {
            UpdateImages(imageReplacements);
        }
        if (_spriteRenderer != null)
        {
            UpdateSprites(spriteReplacements);
        }
    }

    private void UpdateSprites(Sprite[] spriteReplacements)
    {
        for (int i = 0; i < _spriteRenderer.Length && i < spriteReplacements.Length; i++)
        {
            _spriteRenderer[i].sprite = spriteReplacements[i];
        }
    }

    private void UpdateImages(Sprite[] imageReplacements)
    {
        for (int i = 0; i < _images.Length && i < imageReplacements.Length; i++)
        {
            _images[i].sprite = imageReplacements[i];
        }
    }

    private void UpdateTexts(string[] textReplacements)
    {
        for (int i = 0; i < _texts.Length && i < textReplacements.Length; i++)
        {
            _texts[i].text = textReplacements[i];
        }
    }
}