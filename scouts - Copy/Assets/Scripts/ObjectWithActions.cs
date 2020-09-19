using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class ObjectWithActions : InGameObject
{
	[HideInInspector]
	public Canvas wpCanvas;
	[HideInInspector]
	public Vector3 nameTextOffset = new Vector3(0, 0.55f, 0), loadingBarOffset = new Vector3(0, 0.15f, 0);
	[HideInInspector]
    public GameObject loadingBar, nameText;

	protected TimeAction action;

	public Button clickListener;

	protected override void Start()
    {
		wpCanvas = GameManager.instance.wpCanvas;
		nameText = Instantiate(wpCanvas.transform.Find("Name").gameObject, transform.position, Quaternion.identity, wpCanvas.transform);
		loadingBar = Instantiate(wpCanvas.transform.Find("LoadingBar").gameObject, transform.position + loadingBarOffset, Quaternion.identity, wpCanvas.transform);
		nameText.GetComponent<TextMeshProUGUI>().text = name;
		if (clickListener != null)
			clickListener.onClick.AddListener(OnClick);
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
	protected override bool CheckActionManager(int buttonIndex)
	{
		action = new TimeAction(buttons[buttonIndex].generalAction.name, name, buttons[buttonIndex].generalAction.timeNeeded);
		if (ActionManager.instance.CheckIfTooManyActions())
		{
			ActionManager.instance.AddAction(action);
			return true;
		}
		else
		{
			return false;
		}
		
	}


	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCanDoActionOnBuilding: return ActionManager.instance.CanDoAction(name);
			default: return base.GetConditionValue(t);
		}
	}
}

