using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class SelectableUnitComponent : MonoBehaviour
{
	[SerializeField] private bool selection = false;

	public void setSelection(bool s)
	{
		this.selection = s;
	}

	public bool isSelected()
	{
		return selection;
	}
}
