using UnityEngine;

namespace Sources.Presenters.HelperViews
{
    public class ToggleableView : MonoBehaviour
    {
        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}