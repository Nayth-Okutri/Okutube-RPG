using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public struct InitialPositions
{
    public int SceneIndex;
    public Vector3 Position;
}
[CreateAssetMenu]
public class Cutscene : ScriptableObject
{
    public GameObject animationPrefab;
    private Animator animation;
    public AudioClip animationAudio;
    public float AnimationDuration;
    [SerializeField]
    public InitialPositions[] scenesInitialPositions;
    public Dictionary<int, Vector3> InitialPositions = new Dictionary<int, Vector3>();
    private GameObject e;
    private bool HasHiddenParam=false;
    private void OnEnable()
    {
        if (animationPrefab != null)
            animation = animationPrefab.GetComponent<Animator>();
        foreach (var i in scenesInitialPositions)
            InitialPositions.Add(i.SceneIndex, i.Position);
        if (animation != null) HasHiddenParam = animation.ContainsParam("IsHidden");
    }


    public void StartCutscene(int SceneIndex)
    {
        e=Instantiate(animationPrefab);
        e.SetActive(true);
        e.transform.position = InitialPositions[SceneIndex];
        AudioSource audioSource = e.GetComponent<AudioSource>();
        Animator IntroSequence = e.GetComponent<Animator>();
        if (IntroSequence != null) IntroSequence.SetBool("IsHidden", false);
        if (audioSource != null)  audioSource.PlayOneShot(animationAudio);

    }
    public void EndCutscene()
    {
        Animator IntroSequence = e.GetComponent<Animator>();
        if (IntroSequence != null) IntroSequence.SetBool("IsHidden", true);
        e.SetActive(false);
    }
}
