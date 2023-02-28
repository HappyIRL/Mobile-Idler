
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	class TypedUIAction : UIAction
	{
		[SerializeField] private Image typeIcon;
		[SerializeField] private Button typedActionButton;

		public Image TypeIcon
		{
			get => typeIcon; set => typeIcon = value;
		}

		public System.Action<TypedUIAction> TypedActionButtonPressed;
		public int DisplayedOptionIndex { get; set; } = 0;
		public Button TypedActionButton => typedActionButton;

		protected override void Awake()
		{
			base.Awake();
			typedActionButton.onClick.AddListener(() => TypedActionButtonPressed?.Invoke(this));
		}
	}
}
