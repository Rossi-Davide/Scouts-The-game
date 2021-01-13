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
    CampManager campManager = CampManager.instance;

    public int timeNeeded;
    public int energyGiven, materialsGiven, pointsGiven; // can be less than 0
    public int timeBeforeRedo;

    //[Header("Parameters (DONT MODIFY)")]
    [HideInInspector]
    public int EditableTimeNeeded { get { return editableTimeNeeded * campManager.possibleDurations[campManager.camp.settings.durationIndex].actionDurationFactor; } set { editableTimeNeeded = value; } }
    [HideInInspector]
    public int EditableEnergyGiven { get { return editableEnergyGiven * (editableEnergyGiven > 0 ? campManager.possibleDifficulties[campManager.camp.settings.difficultyIndex].prizesFactor : campManager.possibleDifficulties[campManager.camp.settings.difficultyIndex].actionPricesFactor); } set { editableEnergyGiven = value; } }
    [HideInInspector]
    public int EditableMaterialsGiven { get { return editableMaterialsGiven * (editableMaterialsGiven > 0 ? campManager.possibleDifficulties[campManager.camp.settings.difficultyIndex].prizesFactor : campManager.possibleDifficulties[campManager.camp.settings.difficultyIndex].actionPricesFactor); } set { editableMaterialsGiven = value; } }
    [HideInInspector]
    public int EditablePointsGiven { get { return editablePointsGiven * (editablePointsGiven > 0 ? campManager.possibleDifficulties[campManager.camp.settings.difficultyIndex].prizesFactor : campManager.possibleDifficulties[campManager.camp.settings.difficultyIndex].actionPricesFactor); } set { editablePointsGiven = value; } }
    [HideInInspector]
    public int EditableTimeBeforeRedo { get { return editableTimeBeforeRedo * campManager.possibleDurations[campManager.camp.settings.durationIndex].actionWaitTimeFactor; } set { editableTimeBeforeRedo = value; } }

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
