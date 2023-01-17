using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KyoshiStudios.Animation {

    [RequireComponent(typeof(SpriteRenderer))]
    public class SpritePlayer : MonoBehaviour {
        [SerializeField] private List<SpriteAnimation> animations;
        [Tooltip("If it should play the first animation in the list when it is created")]
        [SerializeField] private bool playOnStart = true;

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

        void Start() {
            if(animations.Count > 0 && playOnStart) {
                currentAimation = animations[0];
                PlayAnimation(animations[0]);
            }
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
                GetComponent<SpriteRenderer>().sprite = sprite;
                yield return new WaitForSeconds(1 / animation.fps);
            }

            if(animation.looping)
                PlayAnimation(animation);
            else
                currentLoop = null;

            OnComplete?.Invoke();
            OnCurrentLoopComplete?.Invoke();
            OnCurrentLoopComplete?.RemoveAllListeners();
        }

        private SpriteAnimation GetAnimation(string name) {
            foreach(SpriteAnimation animation in animations) {
                if(animation.id == name) {
                    return animation;
                }
            }
            return null;
        }
    }
}