using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
	class FrontendUI : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text playerMoneyUiText;
		public TMP_Text PlayerMoneyUiText => playerMoneyUiText;

		[SerializeField] private TMP_Text selectableUIActionsHeader;
		public TMP_Text SelectableUiActionsHeader => selectableUIActionsHeader;

		[SerializeField] private Transform selectableUIActionsContainer;
		public Transform SelectableUIActionsContainer => selectableUIActionsContainer;

		[SerializeField] private GameObject uiActionPrefab;
		public GameObject UIActionPrefab => uiActionPrefab;

		[SerializeField] private GameObject typedUIActionPrefab;
		public GameObject TypedUIActionPrefab => typedUIActionPrefab;

		[SerializeField] private GameObject floorSelectUIActionPrefab;
		public GameObject FloorSelectUIActionPrefab => floorSelectUIActionPrefab;

	}
}
