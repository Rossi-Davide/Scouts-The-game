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
    //public BuildingState state;

    public int timeNeeded;
    public int energyGiven, materialsGiven, pointsGiven; // can be less than 0
    public int timeBeforeRedo;

    //[Header("Parameters (DONT MODIFY)")]
    [HideInInspector]
    public int EditableTimeNeeded { get { return CampManager.instance.MultiplyByDurationFactor(editableTimeNeeded, DurationFactor.actionDurationFactor); } set { editableTimeNeeded = value; } }
    [HideInInspector]
    public int EditableEnergyGiven { get { return editableEnergyGiven > 0 ? CampManager.instance.MultiplyByDurationFactor(editableEnergyGiven, DurationFactor.prizesFactor) : CampManager.instance.MultiplyByDurationFactor(editableEnergyGiven, DurationFactor.actionPricesFactor); } set { editableEnergyGiven = value; } }
    [HideInInspector]
    public int EditableMaterialsGiven { get { return editableMaterialsGiven > 0 ? CampManager.instance.MultiplyByDurationFactor(editableMaterialsGiven, DurationFactor.prizesFactor) : CampManager.instance.MultiplyByDurationFactor(editableMaterialsGiven, DurationFactor.actionPricesFactor); } set { editableMaterialsGiven = value; } }
    [HideInInspector]
    public int EditablePointsGiven { get { return editablePointsGiven > 0 ? CampManager.instance.MultiplyByDurationFactor(editablePointsGiven, DurationFactor.prizesFactor) : CampManager.instance.MultiplyByDurationFactor(editablePointsGiven, DurationFactor.actionPricesFactor); } set { editablePointsGiven = value; } }
    [HideInInspector]
    public int EditableTimeBeforeRedo { get { return CampManager.instance.MultiplyByDurationFactor(editableTimeBeforeRedo, DurationFactor.actionWaitTimeFactor); } set { editableTimeBeforeRedo = value; } }

    int editableTimeNeeded;
    int editableEnergyGiven;
    int editableMaterialsGiven;
    int editablePointsGiven;
    int editableTimeBeforeRedo;

    public void ResetEditableInfo()
    {
        EditableTimeBeforeRedo = timeBeforeRedo;
        EditableEnergyGiven = energyGiven;
        EditableMaterialsGiven = materialsGiven;
        EditablePointsGiven = pointsGiven;
        EditableTimeNeeded = timeNeeded;
        //Debug.Log($"modified values: {EditableEnergyGiven}, {EditableMaterialsGiven}, {EditablePointsGiven}; real values: {editableEnergyGiven}, {editableMaterialsGiven}, {editablePointsGiven}");
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
        if (EditableEnergyGiven < 0)
            GameManager.instance.ChangeCounter(Counter.Energia, EditableEnergyGiven);
        if (EditableMaterialsGiven < 0)
            GameManager.instance.ChangeCounter(Counter.Materiali, EditableMaterialsGiven);
        if (EditablePointsGiven < 0)
            GameManager.instance.ChangeCounter(Counter.Punti, EditablePointsGiven);
    }
    public void ChangeCountersOnEnd()
	{
        if (EditableEnergyGiven > 0)
            GameManager.instance.ChangeCounter(Counter.Energia, EditableEnergyGiven);
        if (EditableMaterialsGiven > 0)
            GameManager.instance.ChangeCounter(Counter.Materiali, EditableMaterialsGiven);
        if (EditablePointsGiven > 0)
            GameManager.instance.ChangeCounter(Counter.Punti, EditablePointsGiven);
    }

}
