using UnityEngine;
using UnityEngine.UI;
using FZFUI.UI.Buttons;
using TMPro;
using UnityEngine;
using Nobi.UiRoundedCorners;

namespace UI
{
    public partial class ResourceItemView
    {
        [SerializeField] private Image ResourceIcon;
		[SerializeField] private BasicButton ResourceInfoButton;
		[SerializeField] private TextMeshProUGUI ResourceAmount;
		[SerializeField] private RectTransform RectTransform;
		[SerializeField] private CanvasRenderer CanvasRenderer;
		[SerializeField] private Image Image;
		[SerializeField] private VerticalLayoutGroup VerticalLayoutGroup;
		[SerializeField] private ImageWithRoundedCorners ImageWithRoundedCorners;
    }
}
