using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sources.Architecture.Interfaces;
using Sources.Presenters.HelperViews;
using UnityEngine;

namespace Sources.GameLoop.Services
{
    public class PopupService: IPopupService
    {
        private const int IncreaseAmount = 10;
        private readonly List<Popup> _popupPool = new List<Popup>(IncreaseAmount);
        private readonly Popup _popupPrefab;
        private readonly Transform _parent;

        public PopupService(Popup popupPrefab, Transform parent)
        {
            _popupPrefab = popupPrefab;
            _parent = parent;
        }

        public void ShowPopup(Vector2 position, string message)
        {
            var freePopup = GetFreePopup;
            if (freePopup == null)
            {
                CreateNewPopups();
                freePopup = GetFreePopup;
            }
            freePopup.gameObject.SetActive(true);
            freePopup.Show(position, message);
        }

        private Popup GetFreePopup => _popupPool.FirstOrDefault(p => !p.gameObject.activeSelf);

        private void CreateNewPopups()
        {
            Popup temp;
            for (int i = 0; i < IncreaseAmount; i++)
            {
                temp = GameObject.Instantiate(_popupPrefab, _parent);
                temp.gameObject.SetActive(false);
                _popupPool.Add(temp);
            }
        }
    }
}