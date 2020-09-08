using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.EventSystems;

public abstract class BuildingsActionsAbstract : ObjectWithActions
{
	[HideInInspector]
	public GameObject healthBar;
	[HideInInspector]
	public Vector3 healthBarOffset = new Vector3(0, -0.2f, 0);
	public int health, maxHealth, healthLossAmount, healthLossFrequency;
	protected bool isSafe, isDestroyed;

	public Vector3 clickListenerOffset;

	protected override void Start()
	{
		healthBar = Instantiate(wpCanvas.transform.Find("HealthBar").gameObject, transform.position + healthBarOffset, Quaternion.identity, wpCanvas.transform);
		healthBar.transform.SetParent(wpCanvas.transform, false);
		health = maxHealth;
		healthBar.GetComponent<Slider>().maxValue = maxHealth;
		healthBar.GetComponent<Slider>().value = health;
		InvokeRepeating("LoseHealthWhenRaining", 0f, healthLossFrequency);
		GameManager.instance.OnRain += RefreshIsSafe;
		base.Start();
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
			healthBar.SetActive(true);
			health -= healthLossAmount;

			if (health <= 0)
			{
				health = 0;
				isDestroyed = true;
				GetComponent<Animator>().Play(name + "Destroyed");
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
			buttonsText.text = isDestroyed ? name + " (Distrutto)" : name;
			nameText.GetComponent<TextMeshProUGUI>().text = isDestroyed ? name + " (Distrutto)" : name;
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
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, -2);
		isSafe = true;
		RefreshButtonsState();
	}

	protected void Ripara()
	{
		GameManager.instance.ChangeCounter(GameManager.Counter.Energia, -5);
		isDestroyed = false;
		health = maxHealth;
		healthBar.GetComponent<Slider>().value = health;
		healthBar.transform.Find("HealthValue").GetComponent<TextMeshProUGUI>().text = health.ToString();
		GetComponent<Animator>().Play(name + 2);
		RefreshButtonsState();
	}
}


