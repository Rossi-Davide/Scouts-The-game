using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections;
using UnityEngine.UI;
using System;

public abstract class InGameObject : MonoBehaviour
{
	public string objectName;
	public string objectSubName; //the subtitle which appears on screen
	[NonSerialized]
	public string id;
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

	public Vector3 nameTextOffset, clickListenerOffset;
	[HideInInspector]
	[System.NonSerialized]
	public TimeLeftBar loadingBar;
	[HideInInspector]
	[System.NonSerialized]
	public TextMeshProUGUI nameText, subNameText;
	Vector3 subNameRelativeOffset = new Vector3(0, -0.40f, 0);
	Vector3 loadingBarRelativeOffset = new Vector3(0, -0.70f, 0);

	protected Animator animator;
	public BuildingState[] states;
	public int defaultStateIndex;
	[System.NonSerialized]
	public int activeStateIndex = 0;

	#region Animations
	protected void PlayAnimations()
	{
		if (states != null && states.Length > 0)
		{
			var animations = states[activeStateIndex].boolNames;
			var l = GetLevel();
			if (states[activeStateIndex].variesWithLevel && l != null) animator.SetInteger("livello", l.Value);
			if (animations != null) for (int i = 0; i < animations.Length; i++) animator.SetBool(animations[i], states[activeStateIndex].boolValues[i]);
			if (objectName == "Refettorio") animator.SetBool("maschio", CampManager.instance.camp.settings.gender == Gender.Maschio);
			Debug.Log($"Attempting to play animation '{animationPrefix + states[activeStateIndex].name}' for game object {objectName}");
			//Debug.Log($"Attempting to play animation '{animationPrefix + animation}' for game object {objectName}");
		}
	}

	//public void PlayOnActionEndState(PlayerAction action)
	//{
	//	foreach (var s in states)
	//	{
	//		if (s.playAtActionEnd && s.action != null && s.action == action)
	//		{
	//			s.active = true;
	//			break;
	//		}
	//	}
	//}

	protected void RefreshActiveState()
	{
		if (manageAnimationsAutomatically)
		{
			var maxPriority = -1;
			int activeIndex = -1;
			for (int i = 0; i < states.Length; i++)
			{
				var s = states[i];
				//Debug.Log($"The state {s.animationSubstring} is {s.action != null && (ActionManager.instance.currentActions.Exists(el => el.action == s.action && el.building == this) || ActionManager.instance.currentHiddenActions.Exists(el => el.action == s.action && el.building == this))}, {(s.action == null && (s.conditions != null && s.conditions.Length > 0 && FindNotVerified(s.conditions) == null))}, {(s.action == null && (s.conditions == null || s.conditions.Length == 0))}.");
				if (
					(s.action != null && (ActionManager.instance.currentActions.Exists(el => el.action == s.action && el.building == this) || ActionManager.instance.currentHiddenActions.Exists(el => el.action == s.action && el.building == this)))
					|| (s.action == null && (s.conditions != null && s.conditions.Length > 0 && FindNotVerified(s.conditions) == null))
					|| (s.action == null && (s.conditions == null || s.conditions.Length == 0)))
				{
					if (s.priority > maxPriority)
					{
						activeIndex = i;
						maxPriority = s.priority;
					}
				}
			}
			if (activeIndex == -1) activeIndex = defaultStateIndex;
			activeStateIndex = activeIndex;
			Debug.Log("called");
			PlayAnimations();
		}
	}

	protected virtual int? GetLevel()
	{
		return null;
	}
	#endregion

	protected virtual void Start()
	{
		animator = GetComponent<Animator>();
		GameManager.instance.OnActionDo += RefreshPreviousActions;

		id = UnityEngine.Random.Range(100000000, 999999999).ToString();

		wpCanvas = GameManager.instance.wpCanvas;
		buttonCanvas = GameManager.instance.buttonCanvas;
		buttonsText = GameManager.instance.buttonsText;

		if (isInstantiating) // maybe check if listener is null
		{
			clickListener = Instantiate(clickListener, transform.position + clickListenerOffset, Quaternion.identity, buttonCanvas.transform);
		}
		nameText = Instantiate(GameManager.instance.nameTextPrefab, transform.position + nameTextOffset, Quaternion.identity, wpCanvas.transform).GetComponent<TextMeshProUGUI>();
		subNameText = Instantiate(GameManager.instance.subNameTextPrefab, nameText.transform.position + subNameRelativeOffset, Quaternion.identity, wpCanvas.transform).GetComponent<TextMeshProUGUI>();
		loadingBar = Instantiate(GameManager.instance.loadingBarPrefab, nameText.transform.position + loadingBarRelativeOffset, Quaternion.identity, wpCanvas.transform).GetComponent<TimeLeftBar>();
		clickListener.onClick.AddListener(OnClick);
		InvokeRepeating(nameof(RefreshButtonsState), 1f, .2f);

		InvokeRepeating(nameof(RefreshActiveState), .3f, .1f);

		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].buttonNum = i + 1;
			buttons[i].obj = GameManager.instance.actionButtons[i];
			buttons[i].canDo = true;
		}

		if (checkPositionEachFrame) { InvokeRepeating(nameof(MoveUI), 0.1f, 0.05f); }
		if (spawnInRandomPosition) { transform.position = possiblePositions[UnityEngine.Random.Range(0, possiblePositions.Length - 1)]; }

		nameText.text = objectName;
		subNameText.text = objectSubName;

		if (customDataFileName != null && customDataFileName != "")
		{
			SetStatus(SaveSystem.instance.LoadData<Status>(customDataFileName, false));
			SaveSystem.instance.OnReadyToSaveData += SaveData;
		}
		if (GetComponent<PlayerBuildingBase>() == null){MoveUI();}


		isNameAlwaysActive = GetComponent<CapieCambu>() != null || (GetComponent<Squadrigliere>() != null && GetComponent<Squadrigliere>().sq == Player.instance.squadriglia);
		ToggleNameAndSubName(isNameAlwaysActive);

		InvokeRepeating(nameof(CalculatePriceOrPrize), 3f, 0.2f);
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
	void CalculatePriceOrPrize()
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			var b = buttons[i];
			var a = b.generalAction;
			//Debug.Log(objectName + $" energy: {a.EditableEnergyGiven}, points: {a.EditablePointsGiven}, materials: {a.EditableMaterialsGiven}; null: {a == null}");
			var p = (b.generalAction.EditableMaterialsGiven > 0 || b.generalAction.EditableEnergyGiven > 0 || b.generalAction.EditablePointsGiven > 0) ? Math.Max(a.EditableEnergyGiven, Math.Max(a.EditablePointsGiven, a.EditableMaterialsGiven)) : Math.Min(a.EditableEnergyGiven, Math.Min(a.EditablePointsGiven, a.EditableMaterialsGiven));
			if (p == a.EditableEnergyGiven) b.priceOrPrizeType = Counter.Energia;
			if (p == a.EditableMaterialsGiven) b.priceOrPrizeType = Counter.Materiali;
			if (p == a.EditablePointsGiven) b.priceOrPrizeType = Counter.Punti;
			var s = p >= 0 ? "+" : "";
			s += p;
			b.priceOrPrizeAmount = s;
		} //change price or prize string in buttons
		
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
				int? can = b.generalAction.name == "Cucinare" ? (int?)0 : (b.generalAction.name == "Mangiare" ? (int?)1 : (b.generalAction.name == "Lavare i piatti" ? (int?)2 : (int?)null));
				var canDoCycle = can == null || GameManager.instance.GetComponent<CycleAcyion>().CheckConditionsCycle(can);

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
				else if (!canDoCycle)
				{
					GameManager.instance.WarningOrMessage($"Puoi svolgere questa azione solo dopo avere svolto l'azione {b.previousAction.name}!", true);
				}
				else if (!ActionManager.instance.CanDoAction(objectName))
				{
					GameManager.instance.WarningOrMessage("Non puoi svolgere più di un'azione contemporaneamente sullo stesso oggetto!", true);
				}
				else
				{
					var onEnd = GetOnEndAction(n - 1);
					DoActionOnStart(n - 1);
					//if (b.generalAction.state != null && !b.generalAction.state.playAtActionEnd)
					//	b.generalAction.state.active = true;
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
		action = new TimeAction(buttons[buttonIndex].generalAction.name, id, buttonIndex + 1, buttons[buttonIndex].generalAction.showInActionList);
		ActionManager.instance.AddAction(action);
	}
	protected abstract void DoActionOnStart(int buttonIndex);

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



	public abstract Action GetOnEndAction(int buttonIndex);


	/// <summary> Restituisce null se sono tutte verificate. </summary>
	protected Condition FindNotVerified(Condition[] conditions) => conditions.FirstOrDefault(c => c.desiredValue != GetConditionValue(c.type));

	protected virtual bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			//case ConditionType.ConditionCanDoActionOnBuilding: return ActionManager.instance.CanDoAction(objectName);
			case ConditionType.ConditionIsRaining: return GameManager.instance.isRaining;
			case ConditionType.ConditionIsDaytime: return GameManager.instance.isDay;
			case ConditionType.ConditionIsPlayerCloseEnough: if (maxDistanceFromPlayer == 0) { return true; } else { return Vector2.Distance(transform.position, Player.instance.transform.position) <= maxDistanceFromPlayer; };
			case ConditionType.ConditionAreThereAnyRunningEvents: return AIsManager.instance.AreThereAnyRunningEvents != null;
			default: throw new NotImplementedException(t.ToString());
		}
	}
	public virtual void MoveUI()
	{
		loadingBar.transform.position = nameText.transform.position + loadingBarRelativeOffset;
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
	public void ForceToggleName(bool active)
	{
		StartCoroutine(ActualForceToggleName(active));
	}
	public void ForceToggleNameInstant(bool active)
	{
		nameText.gameObject.SetActive(active);
		subNameText.gameObject.SetActive(active);
	}
	public IEnumerator ActualForceToggleName(bool active)
	{
		yield return new WaitForEndOfFrame();
		nameText.gameObject.SetActive(active);
		subNameText.gameObject.SetActive(active);
		//Debug.Log("Set " + (active ? "active" : "inactive"));
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
	public void ResetWait(int b)
	{
		buttons[b].timeLeft = 0;
	}

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
		public string id;
		public ActionButton.Status[] actionButtonInfos;
		//public BuildingState.Status[] buildingStatesInfo;
	}
	public virtual Status SendStatus()
	{
		var b = new ActionButton.Status[buttons.Length];
		for (int i = 0; i < b.Length; i++)
		{
			b[i] = buttons[i].SendStatus();
		}
		//var s = new BuildingState.Status[states.Length];
		//for (int i = 0; i < s.Length; i++)
		//{
		//	s[i] = states[i].SendStatus();
		//}
		//Debug.LogError ("called when started a game");

		return new Status
		{
			position = transform.position,
			active = gameObject.activeSelf,
			actionButtonInfos = b,			
			//buildingStatesInfo = s,
			id = id,
		};
	}
	public virtual void SetStatus(Status status)
	{
		if (status != null)
		{
			transform.position = status.position;
			gameObject.SetActive(status.active);
			id = status.id;
			for (int i = 0; i < status.actionButtonInfos.Length; i++)
			{
				buttons[i].SetStatus(status.actionButtonInfos[i]);
			}
			//for (int i = 0; i < status.buildingStatesInfo.Length; i++)
			//{
			//	states[i].SetStatus(status.buildingStatesInfo[i]);
			//}
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