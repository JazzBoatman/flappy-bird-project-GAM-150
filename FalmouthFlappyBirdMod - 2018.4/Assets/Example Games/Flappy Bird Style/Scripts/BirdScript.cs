using UnityEngine;
using System.Collections;

public class BirdScript : MonoBehaviour 
{
	public float upForce;			//upward force of the "flap"
	public float forwardSpeed;		//forward movement speed
	public bool isDead = false;		//has the player collided with a wall?
    public float moveSpeed;
    public float speedIncreaseAmount;
    private float currentSpeedModifier;
    private Vector3 direction;
    private Rigidbody2D rigidBody;

	Animator anim;					//reference to the animator component
	bool flap = false;				//has the player triggered a "flap"?


	void Start()
	{
		//get reference to the animator component
		anim = GetComponent<Animator> ();
        //set the bird moving forward
        rigidBody = GetComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>().velocity = new Vector2 (forwardSpeed, 0);
        currentSpeedModifier = 1f;
    }


	void Update()
	{
        if (Input.GetKey(KeyCode.W) && !isDead)
        {
            direction = new Vector2(0, 1);
        }

        else if (Input.GetKey(KeyCode.S) && !isDead)
        {
            direction = new Vector2(0, -1);
        }

        else
        {
            direction = Vector2.zero;
        }
	}

	void FixedUpdate()
	{
		//if a "flap" is triggered...
		if (flap) 
		{
			flap = false;

			//...tell the animator about it and then...
			anim.SetTrigger("Flap");
			//...zero out the birds current y velocity before...
			//..giving the bird some upward force
		}
        rigidBody.velocity = direction * moveSpeed;
        currentSpeedModifier += speedIncreaseAmount;

        if (!isDead)
        {
            direction.x = 1;
            rigidBody.velocity = direction * moveSpeed * currentSpeedModifier;
        }
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		//if the bird collides with something set it to dead...
		isDead = true;
		//...tell the animator about it...
		anim.SetTrigger ("Die");
		//...and tell the game control about it
		GameControlScript.current.BirdDied ();
	}
}
