using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NayCharController : MonoBehaviour
{
    public float speed = 3.0f;
    public float changeTime = 3.0f;
    public float displayTime = 4.0f;
    Rigidbody2D rigidbody2d;
    float timerDisplay;
    float horizontal;
    float vertical;
    Vector2 lookDirection = new Vector2(1, 0);
    float timer;
    //int direction = 1;
    Animator animator;
    bool NeedsHighlight;
    public GameObject ArrowHighlight;
    public GameObject ActionIcon;
    public Canvas PlayerCanvas;
    public GameEvent OnActivityTouch;
    public GameEvent OnUIHide;
    private InputState inputstate;
    public Joystick joystick;
    private Vector3 InteractionIconOffset;
    private Vector3 ArrowIconOffset;
    private Vector3 ActionIconOffset;
    private enum InputState
    {
        Busy,
        Available,
    }
    public AudioClip ItemFXClip;
    public AudioClip InteractFXClip;
    AudioSource audioSource;
    float timerSoundFX;
    [SerializeField]
    private GameObject TalkIcon;
    [SerializeField]
    private GameObject RecruitIcon;
    [SerializeField]
    private GameObject ActiviteIcon;
    private bool NPCActive = false;


    // Start is called before the first frame update
    void Start()
    {
        inputstate = InputState.Available;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;
        ArrowHighlight.SetActive(false);
        ActionIcon.SetActive(false);
        NeedsHighlight = false;
        timerDisplay = -1.0f;
        timerSoundFX = 2.0f;
        DisplayHighlightArrow();
        InitializeCanvasUIOffsets();
        audioSource = GetComponent<AudioSource>();
        if (joystick != null)
        {
            //joystick.DeadZone = 0.3f;
            joystick.SnapX = true;
            joystick.SnapY = true;
        }
        TalkIcon.SetActive(false);
        RecruitIcon.SetActive(false);
        ActiviteIcon.SetActive(false);
        RectTransform rectTransform = ActionIcon.GetComponent<RectTransform>();
        Vector3 UIPosition = GetScreenPositionBasedOffScreenRes(gameObject, PlayerCanvas, ActionIconOffset);
        rectTransform.localPosition =  UIPosition;
    }
    private void InitializeCanvasUIOffsets()
    {
        int y = SceneManager.GetActiveScene().buildIndex;
        if (y == 4)
        {
            //InteractionIconOffset = new Vector3(1.5f, 0.8f, 0f);
            InteractionIconOffset = new Vector3(50f, 60f, 0f);
            ActionIconOffset  = new Vector3(1f, 0.8f, 0f);
            ArrowIconOffset = new Vector3(0f, 60f, 0f);
        }
        else
        {
            //InteractionIconOffset = new Vector3(1f, 2.5f, 0f);
            InteractionIconOffset = new Vector3(70f, 100f, 0f);
           
            ActionIconOffset = new Vector3(1f, 1.8f, 0f);
            ArrowIconOffset = new Vector3(0f, 110f, 0f);
        }
    }
    public void PlaySound(AudioClip clip)
    {
        if ((audioSource != null) && (clip != null))
            audioSource.PlayOneShot(clip);
    }
    private void OnEnable()
    {
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HideUIElement(ActionIcon);
        InitializeCanvasUIOffsets();
        inputstate= InputState.Available;
    }

    private Vector3 GetScreenPositionBasedOffScreenRes(GameObject target, Canvas PlayerCanvas, Vector3 Offset)
    {
        Vector3 offsetPos;
      
        offsetPos = target.transform.position + Offset;
        // Calculate *screen* position (note, not a canvas/recttransform position)
        //Vector2 canvasPos;
        GameObject MainCam = GameObject.Find("Main Camera");
        if (MainCam != null)
        {
            Camera cam = MainCam.GetComponent<Camera>();
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
            screenPoint = screenPoint - screenCenter;
            return screenPoint;
        }
        
        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(PlayerCanvas.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);
        return new Vector3(0f,0f,0f);
    }
    public Vector3 WorldToScreenSpace(Vector3 worldPos, Camera cam, RectTransform area)
    {
        Vector3 screenPoint = cam.WorldToScreenPoint(worldPos);
        screenPoint.z = 0;

        Vector2 screenPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, cam, out screenPos))
        {
            return screenPos;
        }

        return screenPoint;
    }
    // Update is called once per frame
    void Update()
    {

        //TIMERS
        ManageTimers();

        //ANMATION
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (joystick !=null)
        {
            if ((joystick.Horizontal > 0.3f) || (joystick.Horizontal < -0.3f)) horizontal = joystick.Horizontal;
            
            if ((joystick.Vertical > 0.3f) || (joystick.Vertical < -0.3f)) vertical = joystick.Vertical;
           
        }
        
        Vector2 move = new Vector2(horizontal, vertical);
       
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
            animator.SetFloat("Move X", lookDirection.x);
            animator.SetFloat("Move Y", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);
        }
       else
        {
            if (joystick != null)
            {
                animator.SetFloat("Move X", 0f);
                animator.SetFloat("Move Y", 0f);
                animator.SetFloat("Speed", 0f);
            }
        }

        //HIGHLIGHT
        if (timer < 0)
            HighlightInteraction();

        //ACTION
        if (inputstate == InputState.Available)
            if (Input.GetKeyDown(KeyCode.X))
            {
                InteractWith();
            }

    }

    private void ManageTimers()
    {
        if ((timerDisplay >= 0) && (NeedsHighlight))
        {
            //ArrowHighlight.GetComponent<RectTransform>().localPosition = GetScreenPositionBasedOffScreenRes(gameObject, PlayerCanvas, ArrowIconOffset);
            ArrowHighlight.GetComponent<RectTransform>().localPosition = ArrowIconOffset;
            timerDisplay -= Time.deltaTime; ;
            if (timerDisplay < 0)
            {
                HideUIElement(ArrowHighlight);
                NeedsHighlight = false;

            }
        }
        if (timerSoundFX >= 0)
        {
            timerSoundFX -= Time.deltaTime; ;
        }
        if (timer >= 0)
        {
            timer -= Time.deltaTime; ;
        }

    }
    private void HideInteractMenu()
    {
        TalkIcon.transform.GetComponent<Button>().onClick.RemoveAllListeners();
        HideUIElement(TalkIcon);
        RecruitIcon.transform.GetComponent<Button>().onClick.RemoveAllListeners();
        HideUIElement(RecruitIcon);
        ActiviteIcon.transform.GetComponent<Button>().onClick.RemoveAllListeners();
        HideUIElement(ActiviteIcon);

    }




    private void PopupIcon()
    {




    }


    private void HighlightInteraction()
    {
        RaycastHit2D UIhighlight = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.3f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
        if (UIhighlight.collider == null) UIhighlight = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.3f, lookDirection, 1.5f, LayerMask.GetMask("Interactable"));
        if (UIhighlight.collider == null) UIhighlight = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.3f, lookDirection, 1.5f, LayerMask.GetMask("Collectible"));

        if (UIhighlight.collider != null)
        {
            NPCController character = UIhighlight.collider.GetComponent<NPCController>();
            if ((character != null))
            { 
                if (!NPCActive)
                {
                    if (timerSoundFX <= 0f)
                    {
                        PlaySound(InteractFXClip);
                        timerSoundFX = 10f;
                    }
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    RectTransform rectTransform;
                    Vector2 cursor;// = GetScreenPositionBasedOffScreenRes(gameObject, PlayerCanvas, InteractionIconOffset);
                    cursor = InteractionIconOffset;
                   
                    if (character.NPCconversation != null)
                    {
                        TalkIcon.SetActive(true);
                        rectTransform = TalkIcon.GetComponent<RectTransform>();
                       // cursor = WorldToScreenSpace(gameObject.transform.position + InteractionIconOffset, Camera.main, rectTransform);
                        rectTransform.localPosition = cursor;
                        //rectTransform.position = cursor;
                        TalkIcon.transform.GetComponent<Button>().onClick.AddListener(() => { HideInteractMenu(); character.DisplayDialog(); });
                        cursor.y -= 30f;
                    }
                    if (character.AssociatedSidekick != null)
                    {
                        RecruitIcon.SetActive(true);
                        rectTransform = RecruitIcon.GetComponent<RectTransform>();
                        rectTransform.localPosition = cursor;
                        RecruitIcon.transform.GetComponent<Button>().onClick.AddListener(() => { HideInteractMenu(); character.RecruitSidekicks(); });
                        cursor.y -= 30f;
                    }
                    Activity activity = character.GetComponent<Activity>();
                    if (activity != null)
                    {

                        ActiviteIcon.SetActive(true);
                        rectTransform = ActiviteIcon.GetComponent<RectTransform>();
                        rectTransform.localPosition = cursor;
                        ActiviteIcon.transform.GetComponent<Button>().onClick.AddListener(() => { HideInteractMenu(); ActivityManager.Instance.StartActivity(activity); });
                    }
                    NPCActive = true;
                }
            }
            else
            {
                RectTransform rectTransform = ActionIcon.GetComponent<RectTransform>();
                Vector3 UIPosition = GetScreenPositionBasedOffScreenRes(gameObject, PlayerCanvas, ActionIconOffset);
                UIPosition = InteractionIconOffset;

                    //rectTransform.localPosition = UIPosition;
                    /*float t = 0f;
                    while (t < 2.0f)
                    {
                        t += Time.deltaTime / 1f;
                        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, UIPosition, t);
                        DisplayUIElement(ActionIcon);
                    }*/
                    //Debug.Log((rectTransform.localPosition - UIPosition).sqrMagnitude);
                if ((rectTransform.localPosition - UIPosition).sqrMagnitude > 3000f) rectTransform.localPosition = UIPosition;
                else
                    rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, UIPosition, Time.deltaTime * 1f);
                DisplayUIElement(ActionIcon);
                Activity activity = UIhighlight.collider.GetComponent<Activity>();
                if (activity != null)
                {
                    OnActivityTouch.sentString = activity.baseActivity.shortDescription;
                    OnActivityTouch.Raise();
                }
                if ((UIhighlight.collider.gameObject != null) && UIhighlight.collider.gameObject.tag == "SceneTransition")
                {
                    if (OnActivityTouch != null)
                    {
                        OnActivityTouch.sentString = "Change Scene vers " + SceneTransition.Instance.GetDestinationName(UIhighlight.collider.name); ;
                        OnActivityTouch.Raise();
                    }
                }
                InventoryItem inventoryItem = UIhighlight.collider.GetComponent<InventoryItem>();
                if (inventoryItem != null)
                {
                    PlaySound(ItemFXClip);
                    inventoryItem.GetItem();
                }
            }
            
        }
        else
        {
            if (OnUIHide != null) OnUIHide.Raise();
            HideUIElement(ActionIcon);
            HideInteractMenu();
            NPCActive = false;

            TalkIcon.transform.GetComponent<Button>().onClick.RemoveAllListeners();
        }

    }


    public void SetInputAvailable()
    {
        inputstate = InputState.Available;
    }
    public void InteractWith()
    {
        if (inputstate == InputState.Available)
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Interactable"));
            //if (hit.collider == null) hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Interactable"));
            if (hit.collider == null) hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Collectible"));

            if (hit.collider != null)
            {
                Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
                Activity activity = hit.collider.GetComponent<Activity>();
                InventoryItem inventoryItem = hit.collider.GetComponent<InventoryItem>();
                GameShop aShop = hit.collider.GetComponent<GameShop>();
                /*NPCController character = hit.collider.GetComponent<NPCController>();
                if (character != null)
                {
                    character.InteractWithCharacter();
                }*/
                if ((hit.collider.gameObject != null) && hit.collider.gameObject.tag == "SceneTransition")
                {
                    inputstate = InputState.Busy;
                    SceneTransition.Instance.ChangeScene(hit.collider.gameObject.name, gameObject.transform.position);
                }
                else if (activity != null)
                {
                    ActivityManager.Instance.StartActivity(activity);
                }
                else if (inventoryItem != null)
                {
                    inventoryItem.GetItem();
                }
                else if (aShop != null)
                {
                    aShop.DisplayShopMenu();
                }
            }
        }
       

    }

    void DisplayHighlightArrow()
    {
        NeedsHighlight = true;
        timerDisplay = displayTime;
        DisplayUIElement(ArrowHighlight);
    }

    void HideUIElement(GameObject UIElement)
    {
        if (UIElement != null)
        {
            UIElement.SetActive(false);
        }
    }

    void DisplayUIElement(GameObject UIElement)
    {
        if (UIElement != null)
        { 
            UIElement.SetActive(true);
        }
    }
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }
}
