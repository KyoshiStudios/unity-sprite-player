using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyoshiStudios.Animation {
    public class Transition {
        private List<string> conditions = new List<string>();

        public SpriteAnimation NextState { get { return nextState; } }
        private SpriteAnimation nextState;

        public Transition(SpriteAnimation nextState, string defaultCondition) {
            this.nextState = nextState;
            conditions.Add(defaultCondition);
        }

        public Transition(SpriteAnimation nextState) {
            this.nextState = nextState;
        }

        public void AddCondition(string condition) {
            conditions.Add(condition);
        }

        public void RemoveCondition(string condition) {
            if(!conditions.Contains(condition)) {
                Debug.LogError($"Tried removing a condition ({condition}) that did not exist inside this transition. \nAnimation:{nextState.id}");
                return;
            }

            conditions.Remove(condition);
        }
        
        public bool IsConditionMet(List<string> conditions) {
            foreach(string condition in this.conditions) {
                if(conditions.Contains(condition)) {
                    return true;
                }
            }

            return false;
        }
    }
}