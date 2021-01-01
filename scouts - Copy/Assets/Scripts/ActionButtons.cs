using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionButtons : MonoBehaviour
{
	public InGameObject selected;
	[System.NonSerialized]
	public bool clicking = false;

	
	#region Singleton
	public static ActionButtons instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("ActionButtons non è un singleton");
		}
		instance = this;
	}
	#endregion

	public void ChangeSelectedObject(InGameObject b)
	{
		//clicking = true;
		if (b != null)
		{
			if (selected == null)
			{
				GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

				selected = b;
				selected.Select();
			}
			else
			{
				selected.Deselect();
				selected = b;
				GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

				selected.Select();
			}
		}
		else
		{
			if (selected != null)
			{
				selected.Deselect();
				selected = null;
			}
		}
	}

	public void Click(int n)
	{
		GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");
		if (selected != null)
		{
			selected.ClickedButton(n);
		}
	}

	private void Start()
	{
		InvokeRepeating(nameof(Check), 0.1f, 0.5f);
	}

	private void Check()
	{
		Debug.Log(clicking);
		if (Input.touchCount > 0 && !clicking&&!IsPointerOverCollider)
		{
			ChangeSelectedObject(null);
			
		}
	}

	

	public bool IsPointerOverCollider
	{
		get {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
			RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
			return (hit.collider!=null);
		}
	}
}
