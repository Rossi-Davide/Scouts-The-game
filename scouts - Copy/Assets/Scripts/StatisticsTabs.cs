using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatisticsTabs : MonoBehaviour
{
	[HideInInspector] [System.NonSerialized]
	public int selectedTab = 1;
	public TextMeshProUGUI nomeSq, description, materials, points;
	public void OnClick(int tabNum)
	{
		selectedTab = tabNum;
		Animator[] animator = transform.GetComponentsInChildren<Animator>();
		foreach (var a in animator)
		{
			if (a.gameObject.name == "Tab" + selectedTab)
			{
				a.Play("Open");
			}
			else
			{
				a.Play("Close");
			}
		}
		RefreshSqInfo();
	}
	public void RefreshSqInfo()
	{
		nomeSq.text = SquadrigliaManager.instance.GetSquadrigliaName(selectedTab);
		description.text = SquadrigliaManager.instance.GetSquadrigliaDescription(selectedTab);
		materials.text = SquadrigliaManager.instance.GetSquadrigliaMaterials(selectedTab).ToString();
		points.text = SquadrigliaManager.instance.GetSquadrigliaPoints(selectedTab).ToString();
	}
}
