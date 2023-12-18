using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class UIAction : MonoBehaviour
	{
		public TMP_Text ActionNameText => actionNameText;
		public Button ActionButton => actionButton;
		public SelectableUI SelectableUI { get; set; }
		public Action<UIAction> ActionButtonPressed { get; set; }

		[SerializeField] protected TMP_Text actionNameText;
		[SerializeField] protected Image uiIcon;
		[SerializeField] protected Button actionButton;

		protected virtual void Awake()
		{
			actionButton.onClick.AddListener(() => ActionButtonPressed?.Invoke(this));
		}
	}
}
