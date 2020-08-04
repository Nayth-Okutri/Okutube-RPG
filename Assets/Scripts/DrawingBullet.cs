using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingBullet : MonoBehaviour
{

    // Start is called before the first frame update
    public Rigidbody2D rigidbody2d;
    private float speed;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        int maxIndex = DrawingMiniGame.DrawingListOfIcons.Count - 1;
        int GetIndex = Random.Range(0, maxIndex);
        if (maxIndex > 0) gameObject.GetComponent<Image>().sprite = DrawingMiniGame.DrawingListOfIcons[GetIndex];

        speed = Random.Range(1, 4);
       
    }

    // Update is called once per frame
    void  FixedUpdate()
    {
       
        Vector2 position = rigidbody2d.position;
        position.x = position.x + Screen.width /10 * speed* Time.deltaTime;
        rigidbody2d.MovePosition(position);

        if (position.x > Screen.width)
            Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "DOT")
        {
            DrawingMiniGame.canPressButton = true;
            
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "DOT")
        {
            DrawingMiniGame.canPressButton = false;

        }
    }
}
