using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public abstract class InGameObject : MonoBehaviour
{
	public string objectName;
	public string objectSubName; //the subtitle which appears on screen
	public string animationPrefix;
	protected TextMeshProUGUI buttonsText;
	public ActionButton[] buttons;
	public float maxDistanceFromPlayer;

	[HideInInspector]
	[System.NonSerialized]
	public GameObject wpCanvas, buttonCanvas;

	protected TimeAction action;
	public bool isInstantiating; //true if object creates instance of clicklistener prefab
	public bool checkPositionEachFrame; //true if object is often moving so it needs to update click listener position frequently
	public bool spawnInRandomPosition;
	public bool manageAnimationsAutomatically; //false for AIs, for example, because they need to change their animations once per frame so they dont need the standard methods.
	public string customDataFileName;
	public Vector3[] possiblePositions;
	public Button clickListener;

	protected bool hasBeenClicked;
	public bool isNameAlwaysActive;

	public Vector3 nameTextOffset, loadingBarOffset, clickListenerOffset;
	[HideInInspector]
	[System.NonSerialized]
	public TimeLeftBar loadingBar;
	[HideInInspector]
	[System.NonSerialized]
	public TextMeshProUGUI nameText, subNameText;
	Vector3 subNameRelativeOffset = new Vector3(0, -0.30f, 0);

	protected Animator animator;
	public BuildingState[] states;

	#region Animations
	protected void ChangeAnimations()
	{
		var maxPriority = -1;
		bool variesWithLevel = true;
		var animation = "";
		foreach (var b in buttons)
		{
			if (b.generalAction.state != null && b.generalAction.state.priority > maxPriority && b.generalAction.state.active)
			{
				maxPriority = b.generalAction.state.priority;
				animation = b.generalAction.state.animationSubstring;
				variesWithLevel = b.generalAction.state.variesWithLevel;
			}
		}
		foreach (var s in states)
		{
			if (s.priority > maxPriority && s.active)
			{
				maxPriority = s.priority;
				animation = s.animationSubstring;
				variesWithLevel = s.variesWithLevel;
			}
		}
		if (variesWithLevel)
			animation = GetAnimationByLevel() + animation;
		animator.Play(animationPrefix + animation);
		Debug.Log($"Attempting to play animation '{animationPrefix + animation}' for game object {objectName}");
	}

	protected void RefreshStates()
	{
		foreach (var s in states)
		{
			s.active = FindNotVerified(s.conditions) == null;
		}
		foreach (var b in buttons)
		{
			if (b.generalAction.state != null)
			{
				b.generalAction.state.active = ActionManager.instance.currentActions.Exists(el => el.action == b.generalAction && el.building == this) || ActionManager.instance.currentHiddenActions.Exists(el => el.action == b.generalAction && el.building == this);
			}
		}
	}

	protected virtual string GetAnimationByLevel()
	{
		return "";
	}
	#endregion

	protected virtual void Start()
	{
		animator = GetComponent<Animator>();
		GameManager.instance.OnActionDo += RefreshPreviousActions;

		wpCanvas = GameManager.instance.wpCanvas;
		buttonCanvas = GameManager.instance.buttonCanvas;
		buttonsText = GameManager.instance.buttonsText;

		if (isInstantiating) // maybe check if listener is null
		{
			clickListener = Instantiate(clickListener, transform.position + clickListenerOffset, Quaternion.identity, buttonCanvas.transform);
		}
		nameText = Instantiate(GameManager.instance.nameTextPrefab, transform.position + nameTextOffset, Quaternion.identity, wpCanvas.transform).GetComponent<TextMeshProUGUI>();
		subNameText = Instantiate(GameManager.instance.subNameTextPrefab, nameText.transform.position + subNameRelativeOffset, Quaternion.identity, wpCanvas.transform).GetComponent<TextMeshProUGUI>();
		loadingBar = Instantiate(GameManager.instance.loadingBarPrefab, transform.position + loadingBarOffset, Quaternion.identity, wpCanvas.transform).GetComponent<TimeLeftBar>();
		clickListener.onClick.AddListener(OnClick);
		InvokeRepeating(nameof(RefreshButtonsState), 1f, .2f);

		if (manageAnimationsAutomatically)
		{
			InvokeRepeating(nameof(ChangeAnimations), 1f, .3f);
			InvokeRepeating(nameof(RefreshStates), .9f, .3f);
		}

		for (int b = 0; b < buttons.Length; b++)
		{
			CalculatePriceOrPrize(buttons[b]);
			buttons[b].buttonNum = b + 1;
			buttons[b].obj = GameManager.instance.actionButtons[b];
			buttons[b].canDo = true;
		} //change price or prize string in buttons


		if (checkPositionEachFrame)
			InvokeRepeating(nameof(MoveUI), 0.1f, 0.05f);
		if (spawnInRandomPosition)
			transform.position = possiblePositions[UnityEngine.Random.Range(0, possiblePositions.Length - 1)];

		nameText.text = objectName;
		subNameText.text = objectSubName;

		if (customDataFileName != null && customDataFileName != "")
		{
			SetStatus(SaveSystem.instance.LoadData<Status>(customDataFileName, false));
			SaveSystem.instance.OnReadyToSaveData += SaveData;
		}
		if (GetComponent<PlayerBuildingBase>() == null)
			MoveUI();


		isNameAlwaysActive = GetComponent<CapieCambu>() != null || (GetComponent<Squadrigliere>() != null && GetComponent<Squadrigliere>().sq == Player.instance.squadriglia);
		ToggleNameAndSubName(isNameAlwaysActive);
	}


	void RefreshPreviousActions(PlayerAction a)
	{
		foreach (var b in buttons)
		{
			if (b.previousAction != null && b.previousAction == a)
			{
				b.hasDonePreviousAction = true;
			}
		}
	}

	protected virtual void SaveData()
	{
		SaveSystem.instance.SaveData(SendStatus(), customDataFileName, false);
	}

	void OnEnable()
	{
		StartCoroutine(ObjectEnabled());
	}
	IEnumerator ObjectEnabled()
	{
		yield return new WaitForEndOfFrame();
		GameManager.instance.BuildingChanged();
	}
	void CalculatePriceOrPrize(ActionButton b)
	{
		string s;
		if (b.generalAction.EditableMaterialsGiven > 0 || b.generalAction.EditableEnergyGiven > 0 || b.generalAction.EditablePointsGiven > 0)
			s = "+";
		else
			s = "-";
		if (b.generalAction.EditableMaterialsGiven >= b.generalAction.EditableEnergyGiven && b.generalAction.EditableMaterialsGiven >= b.generalAction.EditableEnergyGiven)
		{
			s += b.generalAction.EditableMaterialsGiven.ToString();
			b.priceOrPrizeType = Counter.Materiali;
		}
		if (b.generalAction.EditableEnergyGiven >= b.generalAction.EditableMaterialsGiven && b.generalAction.EditableEnergyGiven >= b.generalAction.EditablePointsGiven)
		{
			s += b.generalAction.EditableEnergyGiven.ToString();
			b.priceOrPrizeType = Counter.Energia;
		}
		if (b.generalAction.EditablePointsGiven >= b.generalAction.EditableEnergyGiven && b.generalAction.EditablePointsGiven >= b.generalAction.EditableMaterialsGiven)
		{
			s += b.generalAction.EditablePointsGiven.ToString();
			b.priceOrPrizeType = Counter.Punti;
		}
		if (b.generalAction.EditableMaterialsGiven == 0 && b.generalAction.EditablePointsGiven == 0 && b.generalAction.EditablePointsGiven == 0)
		{
			s = "";
			b.priceOrPrizeType = Counter.None;
		}
		b.priceOrPrizeAmount = s;
	}

	protected Item[] CheckActionItems(ActionButton b)
	{
		Item[] items = new Item[b.generalAction.neededItems.Length];
		for (int i = 0; i < items.Length; i++)
		{
			var y = b.generalAction.neededItems[i];
			if (!InventoryManager.instance.Contains(y) && !ChestManager.instance.Contains(y))
			{
				items[i] = y;
			}
		}
		return null;
	}


	public void OnClick()
	{
		if (hasBeenClicked)
		{
			ActionButtons.instance.ChangeSelectedObject(null);
			hasBeenClicked = false;
		}
		else
		{
			GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("click");

			ActionButtons.instance.ChangeSelectedObject(this);
			hasBeenClicked = true;
		}
	}


	public virtual void Select()
	{
		buttonsText.enabled = true;
		ToggleNameAndSubName(true);
		foreach (var b in buttons)
		{
			b.obj.SetActive(true);
			b.obj.transform.Find("PriceOrPrizeValue").GetComponent<TextMeshProUGUI>().text = b.priceOrPrizeAmount;
			b.obj.transform.Find("EnergyLogo").gameObject.SetActive(b.priceOrPrizeType == Counter.Energia);
			b.obj.transform.Find("MaterialsLogo").gameObject.SetActive(b.priceOrPrizeType == Counter.Materiali);
			b.obj.transform.Find("PointsLogo").gameObject.SetActive(b.priceOrPrizeType == Counter.Punti);
			b.obj.transform.Find("TimeLeftCounter").gameObject.SetActive(b.isWaiting);
		}
		RefreshButtonsState();
	}

	public virtual void Deselect()
	{
		buttonsText.enabled = false;
		ToggleNameAndSubName(false);
		foreach (var b in buttons)
		{
			b.obj.SetActive(false);
		}
	}



	public virtual void ClickedButton(int n)
	{
		if (n - 1 >= 0 && n - 1 < buttons.Length)
		{
			var b = buttons[n - 1];
			var c = FindNotVerified(b.generalAction.conditions);
			if (c == null)
			{
				var i = CheckActionItems(b);
				if (!CheckActionManager(n - 1))
				{
					GameManager.instance.WarningOrMessage("Non puoi eseguire più di 5 azioni contemporaneamente!", true);
				}
				else if (i != null)
				{
					GameManager.instance.WarningOrMessage($"Prima di svolgere l'azione {b.generalAction.name}, devi acquistare {i}", true);
				}
				else if (!b.canDo)
				{
					GameManager.instance.WarningOrMessage("Hai appena fatto o stai ancora facendo questa azione!", true);
				}
				else if (b.previousAction != null && !b.hasDonePreviousAction)
				{
					GameManager.instance.WarningOrMessage($"Puoi svolgere questa azione solo dopo avere svolto l'azione {b.previousAction.name}!", true);
				}
				else
				{
					var onEnd = DoAction(b);
					if (b.generalAction.state != null)
						b.generalAction.state.active = true;
					GameManager.instance.ActionDone(b.generalAction);
					ActuallyAddAction(n - 1, onEnd);
					buttons[n - 1].generalAction.ChangeCountersOnStart();
					b.canDo = false;
					b.hasDonePreviousAction = false;
				}
			}
			else
			{
				GameManager.instance.WarningOrMessage(c.warning, true);
			}
		}

	}

	protected virtual bool CheckActionManager(int buttonIndex)
	{
		return ActionManager.instance.CheckIfNotTooManyActions() || !buttons[buttonIndex].generalAction.showInActionList;
	}

	protected virtual void ActuallyAddAction(int buttonIndex, Action onEnd)
	{
		action = new TimeAction(buttons[buttonIndex].generalAction, this, buttonIndex + 1, loadingBar, onEnd);
		ActionManager.instance.AddAction(action, buttons[buttonIndex].generalAction.showInActionList);
	}

	protected virtual void RefreshButtonsState()
	{
		if (ActionButtons.instance.selected == this)
		{
			buttonsText.text = objectSubName != "" ? objectName + $" ({objectSubName})" : objectName;
			foreach (var b in buttons)
			{
				b.obj.transform.Find("TimeLeftCounter").gameObject.SetActive(b.isWaiting);
				b.obj.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = b.buttonText;
				b.obj.transform.Find("InfoButton").gameObject.SetActive(b.generalAction.hasInfoPanel);
				RefreshTimeLeft(b);
				if (FindNotVerified(b.generalAction.conditions) == null && CheckActionItems(b) == null && ActionManager.instance.CheckIfNotTooManyActions() && b.canDo && (b.previousAction == null || b.hasDonePreviousAction))
				{
					b.obj.GetComponent<Animator>().Play(b.color + "_Enabled");
				}
				else
				{
					b.obj.GetComponent<Animator>().Play(b.color + "_Disabled");
				}
			}
		}
	}



	protected abstract Action DoAction(ActionButton b);


	/// <summary> Restituisce null se sono tutte verificate. </summary>
	protected Condition FindNotVerified(Condition[] conditions) => conditions.FirstOrDefault(c => c.desiredValue != GetConditionValue(c.type));

	protected virtual bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionCanDoActionOnBuilding: return ActionManager.instance.CanDoAction(objectName);
			case ConditionType.ConditionIsRaining: return GameManager.instance.isRaining;
			case ConditionType.ConditionIsDaytime: return GameManager.instance.isDay;
			case ConditionType.ConditionIsPlayerCloseEnough: if (maxDistanceFromPlayer == 0) { return true; } else { return Vector2.Distance(transform.position, Player.instance.transform.position) <= maxDistanceFromPlayer; };
			case ConditionType.ConditionAreThereAnyRunningEvents: return AIsManager.instance.AreThereAnyRunningEvents != null;
			default: throw new NotImplementedException(t.ToString());
		}
	}
	public virtual void MoveUI()
	{
		loadingBar.transform.position = transform.position + loadingBarOffset;
		nameText.transform.position = transform.position + nameTextOffset;
		subNameText.transform.position = nameText.transform.position + subNameRelativeOffset;
		clickListener.transform.position = transform.position + clickListenerOffset;
	}
	public void ToggleClickListener(bool active)
	{
		clickListener.gameObject.SetActive(active);
	}
	public void ToggleNameAndSubName(bool active)
	{
		if ((active && isNameAlwaysActive) || !isNameAlwaysActive)
		{
			nameText.gameObject.SetActive(active);
			subNameText.gameObject.SetActive(active);
		}
	}
	public IEnumerator ToggleLoadingBar(bool active)
	{
		yield return new WaitForEndOfFrame();
		loadingBar.gameObject.SetActive(active);
	}
	public virtual IEnumerator ToggleHealthBar(bool active)
	{
		yield return new WaitForEndOfFrame();
	}



	#region Wait To Use Again
	public void StartWaitToUseAgain(ActionButton b)
	{
		b.obj.transform.Find("TimeLeftCounter").gameObject.SetActive(true);
		b.isWaiting = true;
		b.timeLeft = b.generalAction.EditableTimeBeforeRedo;
		b.isWaiting = true;
	}

	public void CountDownTime(ActionButton b)
	{
		if (b.timeLeft <= 0)
		{
			b.obj.transform.Find("TimeLeftCounter").gameObject.SetActive(false);
			b.isWaiting = false;
			b.canDo = true;
			return;
		}
		RefreshTimeLeft(b);
		b.timeLeft--;
	}



	protected void RefreshTimeLeft(ActionButton b)
	{
		if (ActionButtons.instance.selected == this)
		{
			b.obj.transform.Find("TimeLeftCounter").GetComponent<TextMeshProUGUI>().text = GameManager.IntToMinuteSeconds(b.timeLeft);
		}
	}
	#endregion

	#region status

	[System.Serializable]
	public class Status
	{
		public Vector3 position;
		public bool active;
		public ActionButton.Status[] actionButtonInfos;
	}
	public virtual Status SendStatus()
	{
		var b = new ActionButton.Status[buttons.Length];
		for (int i = 0; i < b.Length; i++)
		{
			b[i] = buttons[i].SendStatus();
		}
		return new Status
		{
			position = transform.position,
			active = gameObject.activeSelf,
			actionButtonInfos = b,
		};
	}
	public virtual void SetStatus(Status status)
	{
		if (status != null)
		{
			transform.position = status.position;
			gameObject.SetActive(status.active);
			for (int i = 0; i < status.actionButtonInfos.Length; i++)
			{
				buttons[i].SetStatus(status.actionButtonInfos[i]);
			}
		}
	}
	#endregion
}



[Serializable]
public class ActionButton
{
	public string buttonText;
	[HideInInspector]
	[NonSerialized]
	public int buttonNum;
	public GameColor color;
	[HideInInspector]
	[NonSerialized]
	public GameObject obj;
	[HideInInspector]
	[NonSerialized]
	public string priceOrPrizeAmount;
	[HideInInspector]
	[NonSerialized]
	public Counter priceOrPrizeType;
	[HideInInspector]
	[NonSerialized]
	public int timeLeft;
	[HideInInspector]
	[NonSerialized]
	public bool isWaiting;
	[HideInInspector]
	[NonSerialized]
	public bool canDo;
	public PlayerAction generalAction;
	public PlayerAction previousAction;
	public bool hasDonePreviousAction;


	[Serializable]
	public class Status
	{
		public bool canDo;
		public bool isWaiting;
		public int timeLeft;
		public bool hasDonePreviousAction;
	}
	public Status SendStatus()
	{
		return new Status
		{
			canDo = canDo,
			isWaiting = isWaiting,
			timeLeft = timeLeft,
			hasDonePreviousAction = hasDonePreviousAction,
		};
	}
	public void SetStatus(Status status)
	{
		canDo = status.canDo;
		isWaiting = status.isWaiting;
		timeLeft = status.timeLeft;
		hasDonePreviousAction = status.hasDonePreviousAction;
	}
}

public enum ConditionType
{
	ConditionIsSafe,
	ConditionIsDestroyed,
	ConditionIsPlayerCloseEnough,
	ConditionIsDaytime,
	ConditionIsRaining,
	ConditionCanDoActionOnBuilding,
	ConditionHasAnythingToSayAI,
	ConditionEDellaStessaSquadriglia,
	ConditionCanUnlockNegozioDelFurfante,
	ConditionHasEnoughMaterials,
	ConditionAreThereAnyRunningEvents,
}

[Serializable]
public class Condition
{
	public ConditionType type;
	public bool desiredValue;
	public string warning;

	public Condition(ConditionType type, bool desiredValue, string warning)
	{
		this.type = type;
		this.desiredValue = desiredValue;
		this.warning = warning;
	}
}