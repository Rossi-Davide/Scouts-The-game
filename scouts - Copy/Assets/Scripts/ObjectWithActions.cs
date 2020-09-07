using UnityEngine;
using TMPro;

public abstract class ObjectWithActions : InGameObject
{
	public Canvas wpCanvas;
	protected Vector3 nameTextOffset = new Vector3(0, 0.55f, 0), loadingBarOffset = new Vector3(0, 0.15f, 0);
    protected GameObject loadingBar, nameText;

	protected ObjectAction action;

    protected override void Start()
    {
		nameText = Instantiate(wpCanvas.transform.Find("Name").gameObject, transform.position + nameTextOffset, Quaternion.identity, wpCanvas.transform);
		loadingBar = Instantiate(wpCanvas.transform.Find("LoadingBar").gameObject, transform.position + loadingBarOffset, Quaternion.identity, wpCanvas.transform);
		nameText.GetComponent<TextMeshProUGUI>().text = name;
		base.Start();
    }

	public override void Select()
	{
		nameText.SetActive(true);
		base.Select();
	}
	public override void Deselect()
	{
		nameText.SetActive(false);
		base.Deselect();
	}
	public override void ClickedButton(int n)
	{
		if (n - 1 >= 0 && n - 1 < buttons.Length)
		{
			var b = buttons[n - 1];
			var c = FindNotVerified(b.conditions);
			if (c == null)
			{
				if (CheckActionManager(n))
					DoAction(b);
				else
					GameManager.instance.WarningMessage("Non puoi eseguire più di 5 azioni contemporaneamente!");

			}
			else
			{
				GameManager.instance.WarningMessage(c.warning);
			}
		}

	}

	public Build.Objects thisObject;
	protected bool CheckActionManager(int buttonNum)
	{
		action = new ObjectAction(GetActionName(buttonNum), thisObject, GetTime(buttonNum));
		return ActionManager.instance.AddAction(action);
	}
	protected virtual int GetTime(int buttonNum)
	{
		return 0;
	}
	protected virtual string GetActionName(int buttonNum)
	{
		return "";
	}


	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCanDoActionOnBuilding: return ActionManager.instance.CanDoAction(thisObject);
			default: return base.GetConditionValue(t);
		}
	}
}

