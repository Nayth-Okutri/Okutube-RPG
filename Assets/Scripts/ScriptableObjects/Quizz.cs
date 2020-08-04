using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct Choice
{
    [TextArea(2, 5)]
    public string text;
    public DialogConversation conversation;
    public QuestDesc TrigerredQuest;

}

[CreateAssetMenu]
public class Quizz : ScriptableObject
{
    [TextArea(2, 5)]
    public string text;
    public Choice[] Choices;

    public bool hasQuest()
    {
        foreach(var i in Choices)
        {
            if (i.TrigerredQuest != null) return true;
            else if (i.conversation.hasQuest()) return true;
        }
        return false;
    }
}


