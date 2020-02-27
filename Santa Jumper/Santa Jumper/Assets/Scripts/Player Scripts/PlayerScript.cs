using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D myBody;

    private Button jumpBtn;

    private bool hasJumped, platformBound;

    public delegate void MoveCamera();
    public static event MoveCamera move;

    private GameObject parent;

    private Text scoreText;
    private int score = 0;

    public Sprite standing_santa;
    public Sprite jumping_santa;


    // Start is called before the first frame update
    void Awake()
    {
        jumpBtn = GameObject.Find("JumpButton").GetComponent<Button>();
        jumpBtn.onClick.AddListener(() => Jump());

        myBody = GetComponent<Rigidbody2D>();

        scoreText = GameObject.Find("Score Text").GetComponent<Text>();
        scoreText.text = score.ToString();
    }

    void Update()
    {
        if(hasJumped && myBody.velocity.y == 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = standing_santa;
            if (!platformBound)
            {
                hasJumped = false;

                score++;
                scoreText.text = score.ToString();

                transform.SetParent(parent.transform);

                GameplayController.instance.CreatePlatform();

                if(move != null)
                {
                    move();
                }
            }
            else if (parent != null)
            {
                transform.SetParent(parent.transform);
                
            }
        } 
    }

    public void Jump()
    {
        if(myBody.velocity.y == 0)
        {
            myBody.velocity = new Vector2(0, 10);
            transform.SetParent(null);
            hasJumped = true;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = jumping_santa;
        }
    }


    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Platform")
        {
            parent = target.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D target)
    {
        if(target.gameObject.tag == "Platform")
        {
            parent = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag == "MainCamera")
        {
            platformBound = true;
        }
    }

    private void OnTriggerExit2D(Collider2D target)
    {
        if (target.tag == "MainCamera")
        {
            platformBound = false;
        }
    }


}//class
