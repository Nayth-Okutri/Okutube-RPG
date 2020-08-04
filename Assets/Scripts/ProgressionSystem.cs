using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionSystem : MonoBehaviour
{
    
    public List<SocialStat> SocialStats = new List<SocialStat>();

    public Dictionary<string, int> PlayerSocialStats;
    public int Level;
    private List<SideKick> sideKicks = new List<SideKick>(); 
    public static ProgressionSystem Instance;
    public int Inspiration;
    private int HitPoint;
    [SerializeField]
    private GameEvent OnCannotAddSidekick;
    [SerializeField]
    GameEvent OnRecruitSidekick;

    private List<GameProgressionBonus> BonusGained = new List<GameProgressionBonus>();
    public BattleSpoil LootInPlay;



    private void applyAllBonusOnLoot()
    {
        if (LootInPlay != null)
        {
            foreach (var b in BonusGained)
            {
                if (b.bonusLootRate != 0)
                {
                    // Limit de bonus ? omg
                    LootInPlay.ApplyLootRateBonus(b.bonusLootRate);
                }

            }
        }
           
    }


    public Item RollItemForCurrentLoot()
    {
        Item _result;
        if (LootInPlay!= null)
        {
            applyAllBonusOnLoot();
            _result= LootInPlay.RollItem();
            return _result;
        }
        
        return null;

    }
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            
        }

        PlayerSocialStats = new Dictionary<string, int>();
        foreach (SocialStat stat in SocialStats)
            PlayerSocialStats.Add(stat.name, 1);
        HitPoint = 50;
        Inspiration = 10;
       
    }

    public void AddSideKick(SideKick s )
    {

        if (sideKicks.Count == 2) OnCannotAddSidekick.Raise();
        else if ((sideKicks.Count < 2) && (!sideKicks.Contains(s)))
        {
            sideKicks.Add(s);
            //ADD BonusGained / lifespan ?
            foreach (var b in s.PlayerBonus)
            {
                BonusGained.Add(b);
            }
            OnRecruitSidekick.Raise();
        }
    }

    public int getSidekicksCount()
    {
        return sideKicks.Count;
    }
    public Sprite GetSidekickPortraitAt(int i)
    {
        return sideKicks[i].CharacterPortrait;
    }
    public string GetSidekickNametAt(int i)
    {
        return sideKicks[i].fullname;

    }
    public string GetSidekickSkillAt(int i)
    {
        return sideKicks[i].SpecialSkill;

    }
    public Sprite GetSidekickMenuartAt(int i)
    {
        
        return sideKicks[i].CharacterArtMenu;

    }
    public void RemoveSideKickAt(int i)
    {
        if ((sideKicks != null) && (sideKicks.Count >= i+1)) sideKicks.RemoveAt(i);
    }
    public void RemoveSideKick(SideKick s)
    {
        if ((sideKicks != null) && (sideKicks.Contains(s))) sideKicks.Remove(s);
    }
    public void InscreaseSocialStat(string key, int value)
    {
        //take into account bonus BonusGained
        if (PlayerSocialStats.ContainsKey(key))
        {
            PlayerSocialStats[key] += value;
        }
        else
        {
            PlayerSocialStats.Add(key, value);
        }

    }
    public string ProduceHPText()
    {
        return "HP : " + HitPoint.ToString();

    }
    public int GetHealthStat()
    {
        return HitPoint;

    }
    public int GetStrengthStat()
    {
        return GetEffectiveSocialStatValue("Courage")*10;

    }
    public int GetDefenceStat()
    {
        return GetEffectiveSocialStatValue("Motivation");

    }
    public int GetSpeedStat()
    {
        return GetEffectiveSocialStatValue("Motivation");

    }
    public void SetHealthStat(int i )
    {
        HitPoint=i;
    }
    public int GetEffectiveSocialStatValue(string name)
    {
        int _result = GetSocialStatValue(name);
        foreach (var b in BonusGained)
        {
            if (b.bonusElement.stat.name == name) _result += b.bonusElement.BonusAmount;
        }
        return _result;

    }

    public int GetSocialStatValue(string name)
    {
        int c;
        PlayerSocialStats.TryGetValue(name, out c);
        return c;
        //Add BONUS
    }
    public string ProduceSocialStatsText()
    {
        string Result="";
        foreach (KeyValuePair<string, int> PlayerSocialStat in PlayerSocialStats)
        {
            Result += "\n" +
                PlayerSocialStat.Key + " : " +  PlayerSocialStat.Value;
        }
        return Result;
    }
   
}
