using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	class TypedLinkedUIAction : LinkedUIAction
	{
		[SerializeField] private Image typeIcon;
		[SerializeField] private Button typedActionButton;

		public Image TypeIcon => typeIcon;

		public System.Action<TypedLinkedUIAction> TypedActionButtonPressed;
		public int DisplayedOptionIndex { get; set; } = 0;

		protected override void Awake()
		{
			base.Awake();
			typedActionButton.onClick.AddListener(() => TypedActionButtonPressed?.Invoke(this));
		}
	}
}
