using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public enum TransitionType
{
    Scale,
    Fade
}

public class UITransition : MonoBehaviour
{
    [SerializeField] private TransitionType transitionType;
    [SerializeField] private float delay;
    [SerializeField] private Ease ease;
    [SerializeField] private float duration;
    [SerializeField] private float transitionValue;
    
    private Image image;
    private Vector3 initScale;
    private Color initColor;
    
    private void Awake()
    {
        if (transitionType == TransitionType.Fade)
        {
            image = GetComponent<Image>();
            initColor = image.color;
        }
        initScale=transform.localScale;
    }

    private void OnEnable()
    {
        if (transitionType == TransitionType.Fade)
        {
            DOTween.Kill(image);
            initColor.a = 0;
            image.color = initColor;
            image.DOFade(transitionValue, duration).SetEase(ease).SetDelay(delay);
        }

        if (transitionType == TransitionType.Scale)
        {
            DOTween.Kill(image);
            transform.localScale=Vector3.zero;
            transform.DOScale(initScale*transitionValue, duration).SetEase(ease).SetDelay(delay);
        }
    }
}
