using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Threading;

public abstract class PlayerBuildingBase : InGameObject
{
	[HideInInspector] [System.NonSerialized]
	public GameObject healthBar;
	[HideInInspector] [System.NonSerialized]
	public Vector3 healthBarOffset = new Vector3(0, -0.2f, 0);
	[HideInInspector] [System.NonSerialized]
	public int health;
	protected bool isSafe, isDestroyed;
	public PlayerBuilding building;

	int timeLeftBeforeHealthLoss; 
	protected override void Start()
	{
		base.Start();
		if (customDataFileName != null && customDataFileName != "")
		{
			SetPBStatus(SaveSystem.instance.LoadData<PBStatus>(customDataFileName, false));
		}
		objectName = building.name;
		objectSubName = "Livello " + (building.level + 1);
		healthBar = Instantiate(GameManager.instance.healthBarPrefab, transform.position + healthBarOffset, Quaternion.identity, wpCanvas.transform);
		health = building.healthInfos[building.level].maxHealth;
		healthBar.GetComponent<Slider>().maxValue = building.healthInfos[building.level].maxHealth;
		healthBar.GetComponent<Slider>().value = health;
		InvokeRepeating(nameof(LoseHealthWhenRaining), 1f, 1f);

	}
	protected override void SaveData()
	{
		SaveSystem.instance.SaveData(SendPBStatus(), customDataFileName, false);
	}


	protected override string GetAnimationByLevel()
	{
		return "Livello" + building.level;
	}

	public override void Select()
	{
		healthBar.SetActive(true);
		base.Select();
	}
	public override void Deselect()
	{
		if (!GameManager.instance.isRaining)
		{
			healthBar.SetActive(false);
		}
		base.Deselect();
	}

	protected virtual void LoseHealthWhenRaining()
	{
		if (GameManager.instance.isRaining && !isSafe && !isDestroyed)
		{
			timeLeftBeforeHealthLoss--;
			if (timeLeftBeforeHealthLoss == 0)
			{
				timeLeftBeforeHealthLoss = building.healthInfos[building.level].healthLossInterval;
				healthBar.SetActive(true);
				health--;
			}

			if (health <= 0)
			{
				health = 0;
				isDestroyed = true;
				GetComponent<Animator>().Play(objectName + "Destroyed");
				RefreshButtonsState();
			}
			healthBar.GetComponent<Slider>().value = health;
			healthBar.transform.Find("HealthValue").GetComponent<TextMeshProUGUI>().text = health.ToString();
		}
		else if (!hasBeenClicked)
		{
			healthBar.SetActive(false);
		}
	}
	protected void RefreshIsSafe()
	{
		if (!GameManager.instance.isRaining)
		{
			isSafe = false;
			if (ActionButtons.instance.selected == this)
			{
				healthBar.SetActive(false);
			}
		}
	}

	protected override void RefreshButtonsState()
	{
		base.RefreshButtonsState();
		if (ActionButtons.instance.selected == this)
		{
			buttonsText.text = isDestroyed ? objectName + " (Distrutto)" : objectName;
			nameText.GetComponent<TextMeshProUGUI>().text = isDestroyed ? objectName + " (Distrutto)" : objectName;
		}
	}
	protected override bool GetConditionValue(ConditionType t)
	{
		switch (t)
		{
			case ConditionType.ConditionIsSafe: return isSafe;
			case ConditionType.ConditionIsDestroyed: return isDestroyed;
			default: return base.GetConditionValue(t);
		}
	}

	protected void MettiAlSicuro()
	{
		GameManager.instance.ChangeCounter(Counter.Energia, -2);
		isSafe = true;
		RefreshButtonsState();
	}

	protected void Ripara()
	{
		GameManager.instance.ChangeCounter(Counter.Energia, -5);
		isDestroyed = false;
		health = building.healthInfos[building.level].maxHealth;
		healthBar.GetComponent<Slider>().value = health;
		healthBar.transform.Find("HealthValue").GetComponent<TextMeshProUGUI>().text = health.ToString();
		GetComponent<Animator>().Play(objectName + 2);
		RefreshButtonsState();
	}


	public override IEnumerator MoveUI()
	{
		base.MoveUI();
		yield return new WaitForEndOfFrame();
		healthBar.transform.position = transform.position + healthBarOffset;
	}

	public class PBStatus : Status
	{
		public int health;
		public bool isSafe;
		public bool isDestroyed;
		public ObjectBase.Status building;
	}
	public virtual PBStatus SendPBStatus()
	{
		var b = new ActionButton.Status[buttons.Length];
		for (int i = 0; i < b.Length; i++)
		{
			b[i] = buttons[i].SendStatus();
		}
		return new PBStatus
		{
			position = transform.position,
			active = gameObject.activeSelf,
			actionButtonInfos = b,
			health = health,
			isSafe = isSafe,
			isDestroyed = isDestroyed,
			building = building.SendStatus(),
		};
	}
	public virtual void SetPBStatus(PBStatus status)
	{
		if (status != null)
		{
			health = status.health;
			isDestroyed = status.isDestroyed;
			isSafe = status.isSafe;
			building.SetStatus(status.building);
		}
	}





}


