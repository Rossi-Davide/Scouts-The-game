using TMPro;
using UnityEngine;

public class ShopObjectBase : MonoBehaviour
{
    protected bool showingInfo;
	protected ShopObjectType type;
    protected TextMeshProUGUI objectName, typeText, price;
    protected GameObject energyLogo, materialsLogo, pointsLogo, infoText, icon, infoButton;

	protected virtual void Awake()
	{
		InitializeVariables();
		RefreshInfo();
	}

    public virtual void RefreshInfo()
	{
        
    }

    protected virtual void InitializeVariables()
	{
		energyLogo = transform.Find("BuyButton/EnergyLogo").gameObject;
		materialsLogo = transform.Find("BuyButton/MaterialsLogo").gameObject;
		pointsLogo = transform.Find("BuyButton/PointsLogo").gameObject;
		infoText = transform.Find("Info").gameObject;
		icon = transform.Find("ItemLogo").gameObject;
		objectName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
		typeText = transform.Find("Type").GetComponent<TextMeshProUGUI>();
		price = transform.Find("BuyButton/Price").GetComponent<TextMeshProUGUI>();
		infoButton = transform.Find("InfoButton").gameObject;
	}
	protected virtual void OnEnable()
	{
		RefreshInfo();
	}

	public void OnClick()
	{
		if (showingInfo)
		{
			ToggleInfo();
		}
	}

	public virtual void ToggleInfo()
	{
		showingInfo = !showingInfo;
		infoText.SetActive(showingInfo);
		price.transform.parent.gameObject.SetActive(!showingInfo);
		icon.SetActive(!showingInfo);
		infoButton.SetActive(!showingInfo);
	}

	public virtual void Select()
	{

	}

}


public enum ShopObjectType
{
    Item,
    Costruzione,
    Decorazione,
}