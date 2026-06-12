using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.Core
{
    public class LoadingScreenView : MonoBehaviour
    {
        private int _sortingOrder = 500; 
        
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Slider progressBar;

        private void Awake()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            if (progressBar != null)
                progressBar.value = 0f;
            var canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = GetComponentInParent<Canvas>();
            }

            if (canvas != null)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.overrideSorting = true;
                canvas.sortingOrder = _sortingOrder;
            }
            else
            {
                Debug.LogWarning("[LoadingScreenView] No Canvas found to configure sorting.");
            }
        }

        public void ShowImmediate()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            if (progressBar != null)
                progressBar.value = 0f;
        }

        public void SetProgress(float value)
        {
            if (progressBar != null)
                progressBar.value = Mathf.Clamp01(value);
        }

        public IEnumerator FadeOut(float duration = 0.3f)
        {
            float start = canvasGroup.alpha;
            float t = 0f;
            canvasGroup.blocksRaycasts = false;

            while (t < duration)
            {
                t += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(start, 0f, t / duration);
                yield return null;
            }

            canvasGroup.alpha = 0f;
        }
    }
}