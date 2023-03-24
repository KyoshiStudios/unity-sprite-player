using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KyoshiStudios.Animation {

    [RequireComponent(typeof(SpriteRenderer))]
    public class SpritePlayer : MonoBehaviour {
        [Tooltip("If it should play the first animation in the list when it is created")]
        [SerializeField] private bool playOnStart = true;
        [Tooltip("The animations that this player can play")]
        [SerializeField] private List<SpriteAnimation> animations;

        /// <summary>
        /// Fired every time an animation loop is completed
        /// </summary>
        public UnityEvent OnComplete = new UnityEvent();
        /// <summary>
        /// Fired when the current animation is done, all listeners are removed afterwards
        /// </summary>
        public UnityEvent OnCurrentLoopComplete = new UnityEvent();

        private SpriteAnimation currentAimation;
        private Coroutine currentLoop = null;

        private SpriteRenderer spriteRenderer;

        private List<string> activeConditions = new List<string>();

        void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();

            if(animations.Count > 0 && playOnStart) {
                currentAimation = animations[0];
                PlayAnimation(animations[0]);
            }
        }

        public void SetCondition(string condition, bool value) {
            if(value && !activeConditions.Contains(condition))
                activeConditions.Add(condition);
            else if(!value && activeConditions.Contains(condition))
                activeConditions.Remove(condition);
        }

        public void PlayAnimation(string name) {
            SpriteAnimation animation = GetAnimation(name);
            if(animation == null) {
                Debug.LogError("Animation not found: " + name);
                return;
            }

            PlayAnimation(animation);
        }

        public void PlayAnimation(SpriteAnimation animation) {
            if(currentLoop != null) {
                StopCoroutine(currentLoop);
            }

            currentAimation = animation;
            currentLoop = StartCoroutine(PlayAnimationRoutine(animation));
        }

        public SpriteAnimation GetCurrentAnimation() {
            return currentAimation;
        }

        private IEnumerator PlayAnimationRoutine(SpriteAnimation animation) {
            foreach(Sprite sprite in animation.sprites) {
                spriteRenderer.sprite = sprite;

                float waitedTime = 0;
                while(waitedTime < 1 / animation.fps) {
                    waitedTime += Time.deltaTime;

                    if(!animation.finishAnimation) {
                        SpriteAnimation checkAnimation = animation.GetNextAnimation(activeConditions);
                        if(checkAnimation != animation && checkAnimation != null)
                            break;
                    }

                    yield return null;
                }
            }

            SpriteAnimation nextAnimation = animation.GetNextAnimation(activeConditions);
            if(nextAnimation == null)
                currentLoop = null;
            else
                PlayAnimation(nextAnimation);

            OnComplete?.Invoke();
            OnCurrentLoopComplete?.Invoke();
            OnCurrentLoopComplete?.RemoveAllListeners();
        }

        public SpriteAnimation GetAnimation(string name) {
            foreach(SpriteAnimation animation in animations) {
                if(animation.id == name) {
                    return animation;
                }
            }
            return null;
        }
    }
}