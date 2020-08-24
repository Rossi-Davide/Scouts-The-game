using UnityEngine;

public class ActionButtons : MonoBehaviour
{
	public ObjectWithActions selected;
	public static ActionButtons instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("ActionButtons non è un singleton");
		}
		instance = this;
	}

	public void ChangeSelectedObject(ObjectWithActions b)
	{

		if (b != null)
		{
			if (selected == null)
			{
				selected = b;
				selected.Select();
			}
			else
			{
				selected.Deselect();
				selected = b;
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
		if (selected != null)
		{
			selected.ClickedButton(n);
		}
	}
}
