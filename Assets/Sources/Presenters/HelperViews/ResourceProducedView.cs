using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presenters.HelperViews
{
    public class ResourceProducedView : MonoBehaviour
    {
        [SerializeField] private Image Icon;
        [SerializeField] private TextMeshProUGUI ProducedAmount;

        public void Init(Sprite icon, string amount)
        {
            Icon.sprite = icon;
            ProducedAmount.text = amount;
        }
    }
}