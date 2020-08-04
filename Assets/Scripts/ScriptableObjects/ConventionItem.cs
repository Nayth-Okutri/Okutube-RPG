using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ConventionItem : ScriptableObject
{
    public string Fullname;
    public ConventionItemType ItemType;
    public float Completion;
    public Sprite icon;
    //Avantages inconvénients
    public string Bonus;
}
