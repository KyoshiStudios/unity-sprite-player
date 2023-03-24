using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KyoshiStudios.Animation;

namespace KyoshiStudios.Animation {

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

            SpriteAnimation idle = player.GetAnimation("Idle");
            SpriteAnimation walk = player.GetAnimation("Walk");
            SpriteAnimation attack = player.GetAnimation("Attack");

            Transition attackTransition = new Transition(attack, "attack");
            Transition walkTransition = new Transition(walk, "isWalking");
            Transition idleTransition = new Transition(idle, "isIdle");

            //might add an option to automate this using all animations

            //Add them in order of priority
            idle.AddTransition(attackTransition);
            idle.AddTransition(walkTransition);

            walk.AddTransition(attackTransition);
            walk.AddTransition(idleTransition);

            attack.AddTransition(idleTransition);
            attack.AddTransition(walkTransition);
        }

        // Update is called once per frame
        void Update() {
            //Control attacking
            if(Input.GetKeyDown(KeyCode.Space)) {
                player.SetCondition("attack", true);
            }
            if(Input.GetKeyUp(KeyCode.Space)) {
                player.SetCondition("attack", false);
            }

            bool isWalking = true;
            //Control the player moving
            if(Input.GetKey(KeyCode.LeftArrow)) {
                sprite.flipX = true;
                rb.velocity = new Vector2(-5, rb.velocity.y);
            } else if(Input.GetKey(KeyCode.RightArrow)) {
                sprite.flipX = false;
                rb.velocity = new Vector2(5, rb.velocity.y);
            } else {
                rb.velocity = new Vector2(0, rb.velocity.y);
                isWalking = false;
            }

            player.SetCondition("isWalking", isWalking);
            player.SetCondition("isIdle", !isWalking);

            /*
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
            }*/
        }
    }
}