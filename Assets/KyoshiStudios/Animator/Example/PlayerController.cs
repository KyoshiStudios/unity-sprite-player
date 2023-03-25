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
        }
    }
}