using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class UnitSelectionComponent : MonoBehaviour
{
	private List<Transform> selectedObjects = new List<Transform>();

	private void unitSelectionSystem()
	{
		if (Input.GetMouseButtonDown(0))
		{
			foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
				if (selectableObject.isSelected())
				{
					Renderer rend = selectableObject.GetComponent<Renderer>();
					rend.material.color = Color.white;
					selectableObject.setSelection(false);
				}
		}
	}

	public List<Transform> getSelectedObjects()
	{
		List<Transform> result = selectedObjects;
		return result;
	}

	public void clearSelections()
	{
		this.selectedObjects.Clear();
	}

	void Update()
	{
		this.unitSelectionSystem();
	}
}
