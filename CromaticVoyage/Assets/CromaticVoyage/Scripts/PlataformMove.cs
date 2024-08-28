using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformMove : MonoBehaviour
{
    public float moveSpeed = 2f;
    public bool plataform1, plataform2;
    public bool moveRight = true, moveUp = true;

    // Update is called once per frame
    void Update()
    {
        if (plataform1)
        {
            if (transform.position.x > -5)
            {
                moveRight = false;
            }
            else if (transform.position.x < -8)
            {
                moveRight = true;
            }

            if (moveRight)
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.right * -moveSpeed * Time.deltaTime);
            }
        }
        
        if (plataform2)
        {
            if (transform.position.y > 3)
            {
                moveUp = false;
            }
            else if (transform.position.y < -1.64f)
            {
                moveUp = true;
            }

            if (moveUp)
            {
                transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.up * -moveSpeed * Time.deltaTime);
            }
        }
    }
}
