using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ActivityDesc : GameSocialEvent
{

    public SocialStat statIncrease;
    public int statIncreaseValue;
    public int TimeSpent;
    /*[TextArea(3, 5)]
    public string description;
    public string shortDescription;*/
    public List<MiniGame> Challenges = new List<MiniGame>();
}
