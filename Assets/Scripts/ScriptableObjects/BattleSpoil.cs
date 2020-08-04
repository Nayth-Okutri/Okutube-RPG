using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemPossibleSpoils
{
    public Item item;
    public int weight;
    public bool RareItem;
}


[CreateAssetMenu]
public class BattleSpoil : ScriptableObject
{
    //RARE ITMS ?? ADD BOOL ? to increase loot rate ?
    public ItemPossibleSpoils[] SpoilList;
    private Dictionary<Item, int> weights = new Dictionary<Item, int>();

    private void OnEnable()
    {
        foreach (var i in SpoilList)
        {
            weights[i.item] = i.weight;
        }
    }

    public void ApplyLootRateBonus(int bonus)
    {
        foreach (var s in SpoilList)
        {
            if (s.RareItem)
            {
                foreach (KeyValuePair<Item, int> i in weights)
                {
                    if (i.Key.fullname == s.item.fullname) weights[i.Key] += bonus;
                }
            }

        }
    }


    public Item RollItem()
    {

        return WeightedRandomizer.From(weights).TakeOne();
    }
}

public static class WeightedRandomizer
{
    public static WeightedRandomizer<R> From<R>(Dictionary<R, int> spawnRate)
    {
        return new WeightedRandomizer<R>(spawnRate);
    }
}

public class WeightedRandomizer<T>
{
    private static Random _random = new Random();
    private Dictionary<T, int> _weights;

    /// <summary>
    /// Instead of calling this constructor directly,
    /// consider calling a static method on the WeightedRandomizer (non-generic) class
    /// for a more readable method call, i.e.:
    /// 
    /// <code>
    /// var selected = WeightedRandomizer.From(weights).TakeOne();
    /// </code>
    /// 
    /// </summary>
    /// <param name="weights"></param>
    /// 

    public WeightedRandomizer(Dictionary<T, int> weights)
    {
        _weights = weights;
    }

    /// <summary>
    /// Randomizes one item
    /// </summary>
    /// <param name="spawnRate">An ordered list withe the current spawn rates. The list will be updated so that selected items will have a smaller chance of being repeated.</param>
    /// <returns>The randomized item.</returns>
    public T TakeOne()
    {
        // Sorts the spawn rate list
        var sortedSpawnRate = Sort(_weights);

        // Sums all spawn rates
        int sum = 0;
        foreach (var spawn in _weights)
        {
            sum += spawn.Value;
        }

        // Randomizes a number from Zero to Sum
        int roll = Random.Range(0, sum);

        // Finds chosen item based on spawn rate
        T selected = sortedSpawnRate[sortedSpawnRate.Count - 1].Key;
        foreach (var spawn in sortedSpawnRate)
        {
            if (roll < spawn.Value)
            {
                selected = spawn.Key;
                break;
            }
            roll -= spawn.Value;
        }

        // Returns the selected item
        return selected;
    }

    private List<KeyValuePair<T, int>> Sort(Dictionary<T, int> weights)
    {
        var list = new List<KeyValuePair<T, int>>(weights);

        // Sorts the Spawn Rate List for randomization later
        list.Sort(
            delegate (KeyValuePair<T, int> firstPair,
                     KeyValuePair<T, int> nextPair)
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
         );

        return list;
    }
}


