using System;
using DG.Tweening;
using UnityEngine;

public enum PanelViewState
    {
        Undefined = 0,
        Hiding = 1,
        Showing = 2
    }
    
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class IPanel : MonoBehaviour
    {
        [SerializeField] protected float fadeInDuration = 0f;
        [SerializeField] protected float fadeOutDuration = 0f;

        private CanvasGroup canvasGroup;

        private PanelViewState currentViewState = PanelViewState.Undefined;
        
        protected void Awake()
        {
            TryGetComponent(out canvasGroup);
            OnAwake();
            
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        protected virtual void OnAwake()
        {
        }

        public void Show()
        {
            if (currentViewState == PanelViewState.Showing)
            {
                return;
            }

            gameObject.SetActive(true);
            canvasGroup.alpha = 0f;

            var openingSequence = DOTween.Sequence()
                .Append(canvasGroup.DOFade(1f, fadeInDuration).SetEase(Ease.Linear));

            var openingAnimation = CreateShowAnimations();
            if (openingAnimation != null)
            {
                openingSequence.Append(openingAnimation);
            }

            openingSequence
                .OnComplete(() =>
                {
                    canvasGroup.alpha = 1f;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                    currentViewState = PanelViewState.Showing;
                    OnShowed();
                })
                .Play();
        }

        protected virtual void OnShowed()
        {
        }

        public void Hide()
        {
            if (currentViewState == PanelViewState.Hiding)
            {
                return;
            }

            if (currentViewState == PanelViewState.Undefined)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                gameObject.SetActive(false);
                return;
            }
            
            var hidingSequence = DOTween.Sequence();

            var hidingAnimation = CreateHideAnimations();
            if (hidingAnimation != null)
            {
                hidingSequence.Append(hidingAnimation);
            }

            hidingSequence
                .Append(canvasGroup.DOFade(0, fadeOutDuration).SetEase(Ease.Linear))
                .OnComplete(() =>
                {
                    currentViewState = PanelViewState.Hiding;
                    OnHid();
                    canvasGroup.alpha = 0f;
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                    gameObject.SetActive(false);
                })
                .Play();
        }

        protected virtual void OnHid()
        {
        }

        protected virtual Tween CreateShowAnimations()
        {
            return null;
        }

        protected virtual Tween CreateHideAnimations()
        {
            return null;
        }
    }   
