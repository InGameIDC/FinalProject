using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    bool moveAllowd;
    Collider2D col;
    public GameObject SelectionEffect;

    private GameMaster gm;

    //-------------------- For mouse ------------------
    private float startPosX;
    private float startPosY;
    private bool isBeaingHeld;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if(touch.phase == TouchPhase.Began)
            {
                Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
                if(col == touchedCollider)
                {
                    moveAllowd = true;
                    Instantiate(SelectionEffect, transform.position, Quaternion.identity);
                }
            }

            if(touch.phase == TouchPhase.Moved)
            {
                if (moveAllowd)
                {
                    transform.position = new Vector2(touchPosition.x, touchPosition.y);
                }
            }

            if(touch.phase == TouchPhase.Ended)
            {
                moveAllowd = false;
            }
        }



        if (isBeaingHeld)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Veggie")
        {
            gm.GameOver();
        }
    }


    private void OnMouseDown()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        startPosX = mousePos.x - this.transform.localPosition.x;
        startPosY = mousePos.y - this.transform.localPosition.y;

        isBeaingHeld = true;
    }

    private void OnMouseUp()
    {
        isBeaingHeld = false;
    }
}
