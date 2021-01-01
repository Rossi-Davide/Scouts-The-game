using UnityEngine;

[CreateAssetMenu(fileName = "New Player Action", menuName = "PlayerAction")]
public class PlayerAction : ScriptableObject
{
    public new string name;
    public string description;
    public Item[] neededItems;
    public Condition[] conditions;
    public bool hasInfoPanel;
    public bool showInActionList;
    public BuildingState state;

    public int timeNeeded;
    public int energyGiven, materialsGiven, pointsGiven; // can be less than 0
    public int timeBeforeRedo;
    [Header("Parameters (DONT MODIFY)")]
    public int editableTimeNeeded;
    public int editableEnergyGiven, editableMaterialsGiven, editablePointsGiven; // can be less than 0
    public int editableTimeBeforeRedo;

    public void ResetEditableInfo()
    {
        editableTimeBeforeRedo = timeBeforeRedo;
        editableEnergyGiven = energyGiven;
        editableMaterialsGiven = materialsGiven;
        editablePointsGiven = pointsGiven;
        editableTimeNeeded = timeNeeded;
    }


    public enum ActionParams
	{
        None,
        timeNeeded,
        energyGiven,
        materialsGiven,
        pointsGiven,
        timeBeforeRedo,
	}


    public void ChangeCountersOnStart()
	{
        if (editableEnergyGiven < 0)
            GameManager.instance.ChangeCounter(Counter.Energia, editableEnergyGiven);
        if (editableMaterialsGiven < 0)
            GameManager.instance.ChangeCounter(Counter.Materiali, editableMaterialsGiven);
        if (editablePointsGiven < 0)
            GameManager.instance.ChangeCounter(Counter.Punti, editablePointsGiven);
    }
    public void ChangeCountersOnEnd()
	{
        if (editableEnergyGiven > 0)
            GameManager.instance.ChangeCounter(Counter.Energia, editableEnergyGiven);
        if (editableMaterialsGiven > 0)
            GameManager.instance.ChangeCounter(Counter.Materiali, editableMaterialsGiven);
        if (editablePointsGiven > 0)
            GameManager.instance.ChangeCounter(Counter.Punti, editablePointsGiven);
    }

}
