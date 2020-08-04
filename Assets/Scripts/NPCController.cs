using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCController : MonoBehaviour
{
    [SerializeField]
    private DialogManager theDialogManager;
    public DialogConversation NPCconversation;
    public QuestDesc activeQuest = null;
    
    public GameEvent OnQuestCompleted;
    public AudioClip HelloClip;
    AudioSource audioSource;
    public bool Moving;
    Rigidbody2D rigidbody2D;
    [SerializeField]
    GameObject RedPointer;
    float timer;
    int direction = 1;
    [SerializeField]
    public SideKick AssociatedSidekick;
    public float speed=1f;
    private float timerSoundFX=0f;
    Animator animator;
    private bool HasMoveAnimation = false;
    void OnEnable()
    {
       
    }

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        GameObject theObject =  GameObject.Find("DialogSystem");
        if (theObject!=null) theDialogManager = theObject.GetComponent<DialogManager>();
        audioSource = GetComponent<AudioSource>();
        if ((HasSocialEventAttached()) &&(RedPointer !=null))
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            var e = Instantiate(RedPointer, transform, false);
            e.transform.localPosition = new Vector3(0.1f, spriteRenderer.bounds.size.y, 0f);
            //StartCoroutine(DrawRedPointer(Random.Range(0,2)));
            //gameObject.transform.position +
        }
        animator = GetComponent<Animator>();
        if (animator != null)  HasMoveAnimation = animator.ContainsParam("Move X");
    }
    IEnumerator DrawRedPointer(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        var e = Instantiate(RedPointer, transform, false);
        e.transform.localPosition = new Vector3(0.1f, spriteRenderer.bounds.size.y, 0f);

    }
    private bool HasSocialEventAttached()
    {
        if (NPCconversation!=null)
        {
            if (NPCconversation.hasQuest()) return true;
        }
        Activity someActivity = GetComponent<Activity>();
        if (someActivity != null) return true;
        return false;
    }
    private QuestDesc GetActiveQuest(Quizz theQuizz)
    {
       
        foreach (Choice choice in theQuizz.Choices)
        {
         
            if ((choice.TrigerredQuest != null) && (QuestSystem.Instance.IsCurrentQuest(choice.TrigerredQuest)))
            {
                return choice.TrigerredQuest;
            }
        }

        return null;
    }
    public void SetActiveQuest(int QuestId)
    {
      
        if ((NPCconversation != null) && (NPCconversation.question != null))
        {
            activeQuest = GetActiveQuest(NPCconversation.question);
           
        }
    }
            
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = Random.Range(4.0f, 8.0f);
        }
        if (timerSoundFX >= 0)
        {
            timerSoundFX -= Time.deltaTime; ;
        }
    }

    void FixedUpdate()
    {
        if ((Moving) && (HasMoveAnimation))
        {
            Vector2 position = rigidbody2D.position;
            position.x = position.x + Time.deltaTime *speed* -direction;
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            //animator.SetFloat("Move Y", direction);
            rigidbody2D.MovePosition(position);
        }
    }

    public void RecruitSidekicks()
    {
        //EN DUR POUR LINSTANT
        if (AssociatedSidekick != null)
        {
            ProgressionSystem.Instance.AddSideKick(AssociatedSidekick);
        }
    }

    public void DisplayDialog()
    {
        if (timerSoundFX <= 0f)
        {
            PlaySound();
            timerSoundFX = 5f;
        }
        if (activeQuest == null)
        {
            if (NPCconversation != null)
            {
                theDialogManager.conversation = NPCconversation;
                theDialogManager.timerDisplay = 5f;
                theDialogManager.StartConversation();
            }
        }
        else if (QuestSystem.Instance.CheckRequirements(activeQuest))
        {
            foreach (var i in activeQuest.completionRequirements.itemRequirement)
            {
                Inventory.Instance.RemoveInventoryItem(i.item, i.itemCount);
            }
           
            theDialogManager.conversation = activeQuest.QuestCompletedConversation;
            theDialogManager.timerDisplay = 5f;
            theDialogManager.AdvanceConversation();
            activeQuest.Completed = true;
            //Communication chelou entre trucs qui se connaissent pas...
            QuestSystem.Instance.theCurrentQuest = activeQuest;
            activeQuest = null;
            OnQuestCompleted.Raise();
        }
        else if (activeQuest != null)
        {
            theDialogManager.DisplayQuestRequirements(activeQuest);
            theDialogManager.conversation = activeQuest.QuestInprogressConversation;
        }

    }
    public void PlaySound()
    {

        if ((audioSource!=null)&& (HelloClip!=null))
        {
            audioSource.volume = 0.3f;
            audioSource.PlayOneShot(HelloClip);
           
        }
             
    }
}
