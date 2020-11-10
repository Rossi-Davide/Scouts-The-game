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
	protected TextMeshProUGUI buttonsText;
	public ActionButton[] buttons;
	public float maxDistanceFromPlayer;

	[HideInInspector]
	public GameObject wpCanvas, buttonCanvas;

	protected TimeAction action; //like local variable
	public bool isInstantiating; //true if object creates instance of clicklistener prefab
	public bool checkPositionEachFrame; //true if object is often moving so it needs to update click listener position frequently
	public bool spawnInRandomPosition;
	public bool manageAnimationsAutomatically; //true for AIs, for example, because they need to change their animations once per frame so they dont need the standard methods.
	public Vector3[] possiblePositions;
	public Button clickListener;

	protected bool hasBeenClicked;

	[HideInInspector]
	public Vector3 nameTextOffset = new Vector3(0, 0.55f, 0), loadingBarOffset = new Vector3(0, 0.15f, 0);
	[HideInInspector]
	public TimeLeftBar loadingBar;
	[HideInInspector]
	public TextMeshProUGUI nameText, subNameText;
	Vector3 subNameRelativeOffset = new Vector3(0, -0.30f, 0);
	protected SaveSystem saveSystem;

	protected virtual void ReceiveSavedData(LoadPriority p)
	{
		if (p == LoadPriority.Low)
		{
			transform.position = (Vector3)saveSystem.RequestData(DataCategory.InGameObject, DataKey.position);
			gameObject.SetActive((bool)saveSystem.RequestData(DataCategory.InGameObject, DataKey.active));
			objectName = (string)saveSystem.RequestData(DataCategory.InGameObject, DataKey.objectName);
			objectSubName = (string)saveSystem.RequestData(DataCategory.InGameObject, DataKey.objectSubName);
			for (int b = 0; b < buttons.Length; b++)
			{
				buttons[b].canDo = (bool)saveSystem.RequestData(DataCategory.InGameObject, DataKey.buttons, DataParameter.canDo, b);
				buttons[b].isWaiting = (bool)saveSystem.RequestData(DataCategory.InGameObject, DataKey.buttons, DataParameter.isWaiting, b);
				buttons[b].timeLeft = (int)saveSystem.RequestData(DataCategory.InGameObject, DataKey.buttons, DataParameter.timeLeft, b);
			}
			MoveUI();
		}
	}


	protected virtual void FixedUpdate()
	{
		if (checkPositionEachFrame)
		{
			MoveUI();
		}
	}

	protected Animator animator;
	public BuildingState[] states;
	protected void ChangeAnimations()
	{
		int max = -1;
		string animation = "";
		foreach (var b in buttons)
		{
			if (b.generalAction.state != null && b.generalAction.state.priority > max)
			{
				max = b.generalAction.state.priority;
				animation = b.generalAction.state.animationSubstring;
			}
		}
		foreach (var s in states)
		{
			if (s.priority > max)
			{
				max = s.priority;
				animation = s.animationSubstring;
			}
		}
		animation += GetAnimationByLevel();
		animator.Play(objectName + animation);
	}

	protected virtual string GetAnimationByLevel()
	{
		return "";
	}

	protected virtual void Start()
	{
		saveSystem = SaveSystem.instance;
		saveSystem.OnReadyToLoad += ReceiveSavedData;
		GameManager.instance.OnCampStart += WhenCampStarts;
		animator = GetComponent<Animator>();
		InvokeRepeating(nameof(RefreshButtonsState), 1f, .2f);
		if (manageAnimationsAutomatically)
			InvokeRepeating(nameof(ChangeAnimations), 1f, .3f);

		for (int b = 0; b < buttons.Length; b++)
		{
			CalculatePriceOrPrize(buttons[b]);
			buttons[b].buttonNum = b + 1;
			buttons[b].obj = GameManager.instance.actionButtons[b];
			buttons[b].canDo = true;
		} //change price or prize string in buttons

		wpCanvas = GameManager.instance.wpCanvas;
		buttonCanvas = GameManager.instance.buttonCanvas;
		buttonsText = GameManager.instance.buttonsText;

		if (isInstantiating) // maybe check if listener is null
		{
			clickListener = Instantiate(clickListener, transform.position, Quaternion.identity, buttonCanvas.transform);
		}
		clickListener.onClick.AddListener(OnClick);

		nameText = Instantiate(GameManager.instance.nameTextPrefab, transform.position + nameTextOffset, Quaternion.identity, wpCanvas.transform).GetComponent<TextMeshProUGUI>();
		subNameText = Instantiate(GameManager.instance.subNameTextPrefab, nameText.transform.position + subNameRelativeOffset, Quaternion.identity, wpCanvas.transform).GetComponent<TextMeshProUGUI>();
		loadingBar = Instantiate(GameManager.instance.loadingBarPrefab, transform.position + loadingBarOffset, Quaternion.identity, wpCanvas.transform).GetComponent<TimeLeftBar>();
		nameText.text = objectName;
		subNameText.text = objectSubName;
	}

	void WhenCampStarts()
	{
		if (spawnInRandomPosition)
		{
			transform.position = possiblePositions[UnityEngine.Random.Range(0, possiblePositions.Length - 1)];
			MoveUI();
		}
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
		string s = "";
		if (b.generalAction.materialsGiven > 0 || b.generalAction.energyGiven > 0 || b.generalAction.pointsGiven > 0)
			s = "+";
		else
			s = "-";
		if (b.generalAction.materialsGiven >= b.generalAction.energyGiven && b.generalAction.materialsGiven >= b.generalAction.energyGiven)
		{
			s += b.generalAction.materialsGiven.ToString();
			b.priceOrPrizeType = Counter.Materiali;
		}
		if (b.generalAction.energyGiven >= b.generalAction.materialsGiven && b.generalAction.energyGiven >= b.generalAction.pointsGiven)
		{
			s += b.generalAction.energyGiven.ToString();
			b.priceOrPrizeType = Counter.Energia;
		}
		if (b.generalAction.pointsGiven >= b.generalAction.energyGiven && b.generalAction.pointsGiven >= b.generalAction.materialsGiven)
		{
			s += b.generalAction.pointsGiven.ToString();
			b.priceOrPrizeType = Counter.Punti;
		}
		if (b.generalAction.materialsGiven == 0 && b.generalAction.pointsGiven == 0 && b.generalAction.pointsGiven == 0)
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
		nameText.gameObject.SetActive(true);
		subNameText.gameObject.SetActive(true);
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
		nameText.gameObject.SetActive(false);
		subNameText.gameObject.SetActive(false);
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
				else
				{
					var onEnd = DoAction(b);
					GameManager.instance.ActionDone(b.generalAction);
					ActuallyAddAction(n - 1, onEnd);
					buttons[n - 1].generalAction.ChangeCountersOnStart();
					b.canDo = false;
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
		if (ActionManager.instance.CheckIfNotTooManyActions() || !buttons[buttonIndex].generalAction.showInActionList)
		{
			return true;
		}
		else
		{
			return false;
		}

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
			buttonsText.text = objectSubName != null ? objectName + $" ({objectSubName})" : objectName;
			foreach (var b in buttons)
			{
				b.obj.transform.Find("TimeLeftCounter").gameObject.SetActive(b.isWaiting);
				b.obj.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = b.buttonText;
				b.obj.transform.Find("InfoButton").gameObject.SetActive(b.generalAction.hasInfoPanel);
				RefreshTimeLeft(b);
				if (FindNotVerified(b.generalAction.conditions) == null && CheckActionItems(b) == null && ActionManager.instance.CheckIfNotTooManyActions() && b.canDo)
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
			default: throw new NotImplementedException(t.ToString());
		}
	}
	public virtual void MoveUI()
	{
		loadingBar.transform.position = transform.position + loadingBarOffset;
		nameText.transform.position = transform.position + nameTextOffset;
		subNameText.transform.position = nameText.transform.position + subNameRelativeOffset;
		clickListener.transform.position = transform.position;
	}
	public virtual void ToggleUI(bool active)
	{
		loadingBar.gameObject.SetActive(active);
		nameText.gameObject.SetActive(active);
		subNameText.gameObject.SetActive(active);
		clickListener.gameObject.SetActive(active);
	}



	#region Wait To Use Again
	public void StartWaitToUseAgain(ActionButton b)
	{
		b.obj.transform.Find("TimeLeftCounter").gameObject.SetActive(true);
		b.isWaiting = true;
		b.timeLeft = b.generalAction.timeBeforeRedo;
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


}



[Serializable]
public class ActionButton
{
	public string buttonText;
	[HideInInspector]
	public int buttonNum;
	public GameColor color;
	[HideInInspector]
	public GameObject obj;
	[HideInInspector]
	public string priceOrPrizeAmount;
	[HideInInspector]
	public Counter priceOrPrizeType;
	[HideInInspector]
	public int timeLeft;
	[HideInInspector]
	public bool isWaiting;
	[HideInInspector]
	public bool canDo;
	public PlayerAction generalAction;
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
	ConditionCookBundle,
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