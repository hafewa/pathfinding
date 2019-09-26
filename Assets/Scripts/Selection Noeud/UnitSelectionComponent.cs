using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class UnitSelectionComponent : MonoBehaviour
{
	private bool isSelecting = false;
	private Vector3 mousePosition1;
	private List<Transform> selectedObjects = new List<Transform>();

	private void unitSelectionSystem()
	{
		if (Input.GetMouseButtonDown(0))
		{
			isSelecting = true;
			mousePosition1 = Input.mousePosition;
			foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
				if (selectableObject.isSelected())
				{
					Renderer rend = selectableObject.GetComponent<Renderer>();
					rend.material.color = Color.white;
					selectableObject.setSelection(false);
				}
		}
		if (Input.GetMouseButtonUp(0))
		{
			foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
				if (this.isInBound(selectableObject.gameObject))
					selectedObjects.Add(selectableObject.transform);
			isSelecting = false;
		}
		if (isSelecting)
		{
			foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
			{
				if (this.isInBound(selectableObject.gameObject))
				{
					if (!selectableObject.isSelected())
					{
						selectableObject.setSelection(true);
						Renderer rend = selectableObject.GetComponent<Renderer>();
						rend.material.color = Color.green;
					}
				}
				else
				{
					if (selectableObject.isSelected())
					{
						Renderer rend = selectableObject.GetComponent<Renderer>();
						rend.material.color = Color.white;
						selectableObject.setSelection(false);
					}
				}
			}
		}
	}

	private bool isInBound(GameObject gameObject)
	{
		if (!isSelecting) return false;
		var camera = Camera.main;
		var viewportBounds = Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);
		return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
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

	void OnGUI()
	{
		if (isSelecting)
		{
			var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
			Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
			Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
		}
	}

	void Update()
	{
		this.unitSelectionSystem();
	}
}
