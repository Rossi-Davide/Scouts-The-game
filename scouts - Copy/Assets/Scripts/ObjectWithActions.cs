using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class ObjectWithActions : InGameObject
{
	public Canvas wpCanvas;
	[HideInInspector]
	public Vector3 nameTextOffset = new Vector3(0, 0.55f, 0), loadingBarOffset = new Vector3(0, 0.15f, 0);
	[HideInInspector]
    public GameObject loadingBar, nameText;

	protected ObjectAction action;

	public Button clickListener;

	protected override void Start()
    {
		nameText = Instantiate(wpCanvas.transform.Find("Name").gameObject, transform.position + nameTextOffset, Quaternion.identity, wpCanvas.transform);
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

	public Build.Objects thisObject;
	protected override bool CheckActionManager(int buttonIndex)
	{
		action = new ObjectAction(buttons[buttonIndex].generalAction.name, thisObject, buttons[buttonIndex].generalAction.timeBeforeRedo);
		return ActionManager.instance.AddAction(action);
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

