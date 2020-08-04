using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MobController : MonoBehaviour
{
    public float speed;
    public float changeTime = 3.0f;
    Rigidbody2D rigidbody2D;
    float timer =2f;
    int direction = 1;
    Animator animator;
    private float tChange = 0.0f; // force new direction in the first Update
    public GameObject Reward;
    private MobState mobState;
    private enum MobState
    {
        Idle,
        InCombat,
    }
    public bool Moving=false;
    public bool BattleOnTouch = false;
    [SerializeField]
    private GameEvent OnGoalCompleted;
    private bool Inactive=true;
    [SerializeField]
    private BattleSpoil loot;




    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mobState = MobState.Idle;
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            if (Inactive) Inactive = false;
            direction = -direction;
            timer = Random.Range(4.0f, 8.0f); 
        }
    }

    void FixedUpdate()
    {
        if (Moving)
        {
            Vector2 position = rigidbody2D.position;
            position.x = position.x + Time.deltaTime * speed * -direction;
            position.y = position.y + Time.deltaTime * speed * direction;
            if (animator != null)
            {
                animator.SetFloat("Move X", 0);
                animator.SetFloat("Move Y", direction);
            }
            rigidbody2D.MovePosition(position);
        }
        
    }

    public void CombatCompleted()
    {
        if (mobState == MobState.InCombat)
        {


            gameObject.SetActive(false);
            TheWorld.Instance.CurrentDayMobCount--;
            float randomNumber = Random.Range(0.0f, 10.0f);
            if (randomNumber > 1.0f)
            {
                Instantiate(Reward, gameObject.transform.position, gameObject.transform.rotation);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if ((!Inactive) && (mobState == MobState.Idle))
        {
            NayCharController player = other.gameObject.GetComponent<NayCharController>();
            if (player != null)
            {
                if (BattleOnTouch)
                {
                    ProgressionSystem.Instance.LootInPlay = loot;
                    mobState = MobState.InCombat;
                    //Pour pouvoir le tuer par la suite avec le catch event
                    DontDestroyOnLoad(gameObject);
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = -200;
                    gameObject.GetComponent<DepthSortByY>().enabled = false;
                    SceneTransition.Instance.LoadSceneForATime("BattleScene", gameObject.transform.position);
                }
                else if (OnGoalCompleted != null) OnGoalCompleted.Raise();
            }
        }
       
    }
       
      

}
