using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SideKick : ScriptableObject
{
    public string fullname;
    public Sprite sprite;
    public GameObject CharacterPrefab;
    public Vector3 PositionInHub;
    public Sprite CharacterPortrait;
    public Sprite CharacterArtMenu;
    public string SpecialSkill;
    public List<GameProgressionBonus> PlayerBonus = new List<GameProgressionBonus>();
}
