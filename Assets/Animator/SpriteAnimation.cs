using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyoshiStudios.Animation {

    [CreateAssetMenu(fileName = "New Sprite Animation", menuName = "Sprite Animation")]
    public class SpriteAnimation : ScriptableObject {
        [Tooltip("The sprites in this animation")]
        public List<Sprite> sprites = new List<Sprite>();
        [Tooltip("The time between each frame")]
        public float fps = 12;
        [Tooltip("A unique name used to identify this animation")]
        public string id = "New Animation";
        [Tooltip("If the animation should loop")]
        public bool looping = true;
    }
}