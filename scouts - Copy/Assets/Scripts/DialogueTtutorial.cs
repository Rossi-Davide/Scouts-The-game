using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class DialogueTtutorial
{
    public TutorialSentence[] sentences;
}
[System.Serializable]
public class TutorialSentence
{
    [TextArea(3, 10)]
    public string sentence;
    public Capo capo;
    public VideoClip clip;
    public Sprite photo;
}
public enum Capo
{
    bongia,
    tommaso,
    cesco,
    fra,
}