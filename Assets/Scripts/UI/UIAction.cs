using CasinoIdler;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	class UIAction : MonoBehaviour
	{
		[SerializeField] private TMP_Text actionNameText;
		[SerializeField] private Image uiIcon;
		[SerializeField] protected Button actionButton;

		public TMP_Text ActionNameText => actionNameText;
		public Image UIIcon => uiIcon;
		public Button ActionButton => actionButton;
		public IAction Action { get; set; }
		public SelectableUI SelectableUI { get; set; }

		public System.Action<UIAction> ActionButtonPressed;

		protected virtual void Awake()
		{
			actionButton.onClick.AddListener(() => ActionButtonPressed?.Invoke(this));
		}
	}
}
