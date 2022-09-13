using DG.Tweening;
using Sources.Architecture.Interfaces;
using TMPro;
using UnityEngine;

namespace Sources.Presenters.HelperViews
{
    public class Popup : MonoBehaviour, ILoadable
    {
        [SerializeField] private TextMeshProUGUI PopupText;
        private const int UpOffset = 50;
        private RectTransform _transform;
        private RectTransform Rect => _transform ??= GetComponent<RectTransform>();

        public void Show(Vector2 position, string message)
        {
            Rect.position = position;
            PopupText.text = message;
            Pop(position);
        }

        private void Pop(Vector2 position)
        {
            Rect.DOMove(position + Vector2.up * 0.4f, 0.5f).OnComplete(() => gameObject.SetActive(false));
        }
    }
}