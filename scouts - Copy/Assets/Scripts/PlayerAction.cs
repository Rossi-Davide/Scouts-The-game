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

    //[Header("Parameters (DONT MODIFY)")]
    //public int EditableTimeNeeded { get { return EditableTimeNeeded * CampManager.instance.possibleDurations[CampManager.instance.camp.settings.durationIndex].actionDurationFactor; } set { EditableTimeNeeded = value; } }
    //public int EditableEnergyGiven { get { return EditableEnergyGiven * (EditableEnergyGiven > 0 ? CampManager.instance.possibleDifficulties[CampManager.instance.camp.settings.difficultyIndex].prizesFactor : CampManager.instance.possibleDifficulties[CampManager.instance.camp.settings.difficultyIndex].actionPricesFactor); } set { EditableEnergyGiven = value; } }
    //public int EditableMaterialsGiven { get { return EditableMaterialsGiven * (EditableMaterialsGiven > 0 ? CampManager.instance.possibleDifficulties[CampManager.instance.camp.settings.difficultyIndex].prizesFactor : CampManager.instance.possibleDifficulties[CampManager.instance.camp.settings.difficultyIndex].actionPricesFactor); } set { EditableMaterialsGiven = value; } }
    //public int EditablePointsGiven { get { return EditablePointsGiven * (EditablePointsGiven > 0 ? CampManager.instance.possibleDifficulties[CampManager.instance.camp.settings.difficultyIndex].prizesFactor : CampManager.instance.possibleDifficulties[CampManager.instance.camp.settings.difficultyIndex].actionPricesFactor); } set { EditablePointsGiven = value; } }
    //public int EditableTimeBeforeRedo { get { return EditableTimeBeforeRedo * CampManager.instance.possibleDurations[CampManager.instance.camp.settings.durationIndex].actionWaitTimeFactor; } set { EditableTimeBeforeRedo = value; } }

    public int EditableTimeNeeded;
    public int EditableEnergyGiven;
    public int EditableMaterialsGiven;
    public int EditablePointsGiven;
    public int EditableTimeBeforeRedo;

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
