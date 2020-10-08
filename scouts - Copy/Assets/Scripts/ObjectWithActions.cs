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
	public TimeLeftBar loadingBar;
	[HideInInspector]
	public GameObject nameText;

	protected TimeAction action;
	public bool isInstantiating;
	public Button clickListener;

	protected override void Start()
    {
		wpCanvas = GameManager.instance.wpCanvas;
		nameText = Instantiate(wpCanvas.transform.Find("Name").gameObject, transform.position + nameTextOffset, Quaternion.identity, wpCanvas.transform);
		loadingBar = Instantiate(wpCanvas.transform.Find("LoadingBar").gameObject, transform.position + loadingBarOffset, Quaternion.identity, wpCanvas.transform).GetComponent<TimeLeftBar>();
		nameText.GetComponent<TextMeshProUGUI>().text = objectName;
		if (clickListener != null)
		{
			if (isInstantiating)
			{
				clickListener = Instantiate(clickListener, transform.position, Quaternion.identity, wpCanvas.transform);
			}
			clickListener.onClick.AddListener(OnClick);
		}
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
		if (ActionManager.instance.CheckIfTooManyActions())
		{
			return true;
		}
		else
		{
			return false;
		}
		
	}
	protected override void ActuallyAddAction(int buttonIndex, System.Action onEnd)
	{
		action = new TimeAction(buttons[buttonIndex].generalAction.name, objectName, buttons[buttonIndex].generalAction.timeNeeded, loadingBar, onEnd);
		ActionManager.instance.AddAction(action);
	}


	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCanDoActionOnBuilding: return ActionManager.instance.CanDoAction(objectName);
			default: return base.GetConditionValue(t);
		}
	}
}

