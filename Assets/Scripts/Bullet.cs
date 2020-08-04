using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    [SerializeField]
    private GameEvent onPlayerHit;
    private void OnEnable()
    {
        Invoke("Destroy",3f);
    }
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir;
    }
    public void Destroy()
    {
        gameObject.SetActive(false);

    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Nay")
        {
       
            //Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            //other.gameObject.SetActive(false);
            gameObject.SetActive(false);
            onPlayerHit.Raise();
        }
    }
}
