using UnityEngine;
using System.Collections;
using TMPro;
using System.Linq;
using UnityEngine.UI;


public abstract class InGameObject : MonoBehaviour
{
    public new string name;
    public TextMeshProUGUI buttonsText;
    public ActionButton[] buttons;
    public float maxDistanceFromPlayer;
    protected bool hasBeenClicked;

	protected virtual void Start()
	{
		InvokeRepeating("RefreshButtonsState", 0f, .4f);
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
			var c = FindNotVerified(b.conditions);
			if (c == null)
			{
				DoAction(b);
			}
			else
			{
				GameManager.instance.WarningMessage(c.warning);
			}
		}

	}


	protected virtual void RefreshButtonsState()
	{
		if (ActionButtons.instance.selected == this)
		{
			buttonsText.text = name;
			foreach (var b in buttons)
			{
				b.obj.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = b.buttonText;
				RefreshTimeLeft(b);
				if (FindNotVerified(b.conditions) == null)
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
			case ConditionType.ConditionHasBeenClicked: return hasBeenClicked;
			case ConditionType.ConditionIsRaining: return GameManager.instance.isRaining;
			case ConditionType.ConditionIsDaytime: return GameManager.instance.isDay;
			case ConditionType.ConditionIsPlayerCloseEnough: if (maxDistanceFromPlayer == 0) { return true; } else { return Vector2.Distance(transform.position, Player.instance.transform.position) <= maxDistanceFromPlayer; };
			default: throw new System.NotImplementedException(t.ToString());
		}
	}



	#region Wait To Use Again
	protected virtual IEnumerator WaitToUseAgain(ActionButton b, System.Action onEnd)
	{
		b.obj.transform.Find("TimeLeftCounter").gameObject.SetActive(true);
		b.isWaiting = true;
		b.timeLeft = b.timeBeforeUse;
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
	public int buttonNum;
	public GameManager.Color color;
	public GameObject obj;
	public string priceOrPrizeAmount; //add "+" or "-" at the beginning
	public GameManager.Counter priceOrPrizeType;
	[HideInInspector]
	public int timeLeft;
	[HideInInspector]
	public bool isWaiting;
	public int timeBeforeUse;

	public Condition[] conditions;
}

public enum ConditionType
{
	ConditionHasBeenClicked,
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