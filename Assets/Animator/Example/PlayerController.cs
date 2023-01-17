using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KyoshiStudios.Animation;

[RequireComponent(typeof(SpritePlayer), typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    private SpritePlayer player;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start() {
        player = GetComponent<SpritePlayer>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        //Control attacking
        if(Input.GetKeyDown(KeyCode.Space)) {
            player.PlayAnimation("Attack");
            player.OnCurrentLoopComplete.AddListener(() => {
                player.PlayAnimation("Idle");
            });
        }

        //Control the player moving
        if(Input.GetKey(KeyCode.LeftArrow)) {
            sprite.flipX = true;
            rb.velocity = new Vector2(-5, rb.velocity.y);
        } else if(Input.GetKey(KeyCode.RightArrow)) {
            sprite.flipX = false;
            rb.velocity = new Vector2(5, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        //Change the animation depending on the speed of the player
        if(player.GetCurrentAnimation().id != "Attack") { //If the player is currently attacking, don't change the animation
            if(rb.velocity.magnitude > 0.2f) {
                if(player.GetCurrentAnimation().id != "Walk") { //Check to make sure you aren't already walking
                    player.PlayAnimation("Walk");
                }
            } else {
                if(player.GetCurrentAnimation().id != "Idle") { //Check to make sure you aren't already idle
                    player.PlayAnimation("Idle");
                }
            }
        }
    }
}