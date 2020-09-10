using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	[HideInInspector]
	public CapieCambu selectedCapoOrCambu;

	#region Singleton
	public static DialogueManager instance;
	private void Awake()
	{
		if (instance != null)
		{
			throw new System.Exception("Dialogue manager non è un singleton");
		}
		instance = this;
	}
	#endregion
	public void NextSentence(int num)
	{
		if (selectedCapoOrCambu != null)
		{
			selectedCapoOrCambu.NextSentence(num);
		}
	}

	public void ClosePanel()
	{
		if (selectedCapoOrCambu != null)
		{
			selectedCapoOrCambu.CancelDialogue();
		}
	}
}
