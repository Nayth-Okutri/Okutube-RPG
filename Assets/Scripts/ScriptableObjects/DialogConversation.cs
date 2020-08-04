using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Line
{
    public CharacterPortrait character;
    [TextArea(2, 5)]
    public string text;
    
}
     

[CreateAssetMenu]
public class DialogConversation : ScriptableObject
{
    public CharacterPortrait LeftPortrait;
    public CharacterPortrait RightPortrait;
    public Line[] lines;
    public Quizz question;

    public bool hasQuest()
    {
        if (question != null) return question.hasQuest();
        else return false;
    }

}
