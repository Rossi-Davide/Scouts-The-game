using UnityEngine;
using UnityEngine.EventSystems;

public class ClickedObjects : MonoBehaviour
{
	#region Singleton
	public static ClickedObjects instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("ClickedObjects non è un singleton");
		}
		instance = this;
	}
	#endregion
	private void Update()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
		RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
		if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
		{
			Debug.Log("Clicked on UI");
		}
		if (Input.touchCount >= 1)
		{
			if (!PanZoom.instance.panningOrZooming && !Joystick.instance.isUsingJoystick && ActionButtons.instance.selected != null && !ClickedOnUI && hit.collider == null)
			{
				ActionButtons.instance.ChangeSelectedObject(null);
			}
		}
	}

	public bool ClickedOnUI
	{
		get
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				if (EventSystem.current.IsPointerOverGameObject(i))
					return true;
			}
			return false;
		}
	}
}
