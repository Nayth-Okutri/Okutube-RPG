using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BulletSource : MonoBehaviour
{
    [SerializeField]
    private int bulletAmount = 10;
    [SerializeField]
    private float startAngle = 90f, endAngle = 270f;
    private Vector2 bulletMoveDirection;
    float timer;
    int direction = 1;
    Rigidbody2D rigidbody2D;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Fire", 0f, 2f);
        rigidbody2D = GetComponent<Rigidbody2D>();

    }

    private void Fire()
    {
        float angleStep = (endAngle - startAngle) / bulletAmount;
        float angle = startAngle;
        for(int i=0;i<bulletAmount;i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = ObjectPooler.Instance.GetPooledObject("bullet");
            if (bul != null)
            {
                bul.transform.position = transform.position;
                bul.transform.rotation = transform.rotation;
                bul.SetActive(true);
                bul.GetComponent<Bullet>().SetMoveDirection(bulDir);
                angle += angleStep;
            }
        }

    }


    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = Random.Range(1.0f, 4.0f);
        }
      

    }
    void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;

       
            position.x = position.x + Time.deltaTime  * direction;
            position.y = position.y + Time.deltaTime  * -direction;
            /*animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", -direction);*/
       

        rigidbody2D.MovePosition(position);
    }
}
