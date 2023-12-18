using CasinoIdler;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class LinkedUIAction : UIAction
	{
		public IAction Action { get; set; }
	}
}
