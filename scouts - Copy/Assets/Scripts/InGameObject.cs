using UnityEngine;
using System.Collections;
using TMPro;
using System.Linq;
using UnityEngine.UI;


public abstract class InGameObject : MonoBehaviour
{
    public new string name;
    protected TextMeshProUGUI buttonsText;
    public ActionButton[] buttons;
    public float maxDistanceFromPlayer;
    protected bool hasBeenClicked;

	protected virtual void Start()
	{
		InvokeRepeating("RefreshButtonsState", 0f, .4f);
		for (int b = 0; b < buttons.Length; b++)
		{
			CalculatePriceOrPrize(buttons[b]);
			buttons[b].buttonNum = b + 1;
			buttons[b].obj = GameManager.instance.actionButtons[b];
		} //change price or prize string in buttons
		buttonsText = GameManager.instance.buttonsText;
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
			b.priceOrPrizeType = GameManager.Counter.Materiali;
		}
		if (b.generalAction.energyGiven >= b.generalAction.materialsGiven && b.generalAction.energyGiven >= b.generalAction.pointsGiven)
		{
			s += b.generalAction.energyGiven.ToString();
			b.priceOrPrizeType = GameManager.Counter.Energia;
		}
		if (b.generalAction.pointsGiven >= b.generalAction.energyGiven && b.generalAction.pointsGiven >= b.generalAction.materialsGiven)
		{
			s += b.generalAction.pointsGiven.ToString();
			b.priceOrPrizeType = GameManager.Counter.Punti;
		}
		if (b.generalAction.materialsGiven == 0 && b.generalAction.pointsGiven == 0 && b.generalAction.pointsGiven == 0)
		{
			s = "";
			b.priceOrPrizeType = GameManager.Counter.None;
		}
		b.priceOrPrizeAmount = s;
	}


	protected Item[] CheckGeneralAction(ActionButton b)
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
		foreach (var b in buttons)
		{
			b.obj.SetActive(true);
			b.obj.transform.Find("PriceOrPrizeValue").GetComponent<TextMeshProUGUI>().text = b.priceOrPrizeAmount;
			b.obj.transform.Find("EnergyLogo").gameObject.SetActive(b.priceOrPrizeType == GameManager.Counter.Energia);
			b.obj.transform.Find("MaterialsLogo").gameObject.SetActive(b.priceOrPrizeType == GameManager.Counter.Materiali);
			b.obj.transform.Find("PointsLogo").gameObject.SetActive(b.priceOrPrizeType == GameManager.Counter.Punti);
			b.obj.transform.Find("TimeLeftCounter").gameObject.SetActive(b.isWaiting);
		}
		RefreshButtonsState();
	} 

	public virtual void Deselect()
	{
		buttonsText.enabled = false;
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
				var i = CheckGeneralAction(b);
				if (CheckActionManager(n - 1) && i == null)
				{
					DoAction(b);
					GameManager.instance.ActionDone(b.generalAction);
				}
				else if (!CheckActionManager(n - 1))
				{
					GameManager.instance.WarningMessage("Non puoi eseguire più di 5 azioni contemporaneamente!");
				}
				else if (i != null)
				{
					GameManager.instance.WarningMessage($"Prima di svolgere l'azione {b.generalAction.name}, devi acquistare {i}");
				}
			}
			else
			{
				GameManager.instance.WarningMessage(c.warning);
			}
		}

	}

	protected virtual bool CheckActionManager(int buttonIndex) { return true;  }


	protected virtual void RefreshButtonsState()
	{
		if (ActionButtons.instance.selected == this)
		{
			buttonsText.text = name;
			foreach (var b in buttons)
			{
				b.obj.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = b.buttonText;
				RefreshTimeLeft(b);
				if (FindNotVerified(b.generalAction.conditions) == null && CheckGeneralAction(b) == null && ActionManager.instance.CheckIfTooManyActions())
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



	protected abstract void DoAction(ActionButton b);


	/// <summary> Restituisce null se sono tutte verificate. </summary>
	protected Condition FindNotVerified(Condition[] conditions) => conditions.FirstOrDefault(c => c.desiredValue != GetConditionValue(c.type));

	protected virtual bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionIsRaining: return GameManager.instance.isRaining;
			case ConditionType.ConditionIsDaytime: return GameManager.instance.isDay;
			case ConditionType.ConditionIsPlayerCloseEnough: if (maxDistanceFromPlayer == 0) { return true; } else { return Vector2.Distance(transform.position, Player.instance.transform.position) <= maxDistanceFromPlayer; };
			default: throw new System.NotImplementedException(t.ToString());
		}
	}



	protected void ChangeCounter(int n)
	{
		var b = buttons[n - 1];
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, b.generalAction.energyGiven);
		GameManager.instance.ChangeCounter(GameManager.Counter.Materiali, b.generalAction.materialsGiven);
		GameManager.instance.ChangeCounter(GameManager.Counter.Punti, b.generalAction.pointsGiven);
	}





	#region Wait To Use Again
	protected virtual IEnumerator WaitToUseAgain(ActionButton b, System.Action onEnd)
	{
		b.obj.transform.Find("TimeLeftCounter").gameObject.SetActive(true);
		b.isWaiting = true;
		b.timeLeft = b.generalAction.timeBeforeRedo;
		while (b.timeLeft > 0)
		{
			RefreshTimeLeft(b);
			yield return new WaitForSeconds(1);
			b.timeLeft--;
		}
		b.obj.transform.Find("TimeLeftCounter").gameObject.SetActive(false);
		b.isWaiting = false;
		onEnd();
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



[System.Serializable]
public class ActionButton
{
	public string buttonText;
	[HideInInspector]
	public int buttonNum;
	public GameManager.Color color;
	[HideInInspector]
	public GameObject obj;
	[HideInInspector]
	public string priceOrPrizeAmount;
	[HideInInspector]
	public GameManager.Counter priceOrPrizeType;
	[HideInInspector]
	public int timeLeft;
	[HideInInspector]
	public bool isWaiting;
	public PlayerAction generalAction;
}

public enum ConditionType
{
	ConditionIsSafe,
	ConditionIsDestroyed,
	ConditionIsPlayerCloseEnough,
	ConditionIsDaytime,
	ConditionIsRaining,
	ConditionCanEat,
	ConditionCanDance,
	ConditionCanCleanLatrina,
	ConditionCanCleanLavaggi,
	ConditionPuoLavarePanni,
	ConditionPuoLavarePiatti,
	ConditionPuoFareLegnaPerFuoco,
	ConditionPuoAttaccareLaCambusa,
	ConditionPuoFareAlzabandiera,
	ConditionCanCook,
	ConditionCanSleepOnAmaca,
	ConditionStaFacendoLegna,
	ConditionCanDoActionOnBuilding,
	ConditionCanTalkAI,
	ConditionHasAnythingToSayAI,
	ConditionPuoMandareAFareLegna,
	ConditionEDellaStessaSquadriglia,
	ConditionStaFacendoLegnaAI,
	ConditionCanUnlockNegozioDelFurfante,
	ConditionPuoSfidareSquadriglia,
}

[System.Serializable]
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