using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezygamers.cmsv1
{
    
    public class LevelConfigSO : ScriptableObject
    {
        // Public field to store the level number
        public int LevelNumber;
        // Public field to store the number of sublevels
        public int noOfSubLevel;

        // Private backing field for questions
        [SerializeField]
        private List<QuestionBaseSO> questions = new List<QuestionBaseSO>();

        // Property to access questions
        public List<QuestionBaseSO> Questions
        {
            get => questions;
            set
            {
                if (value != null)
                {
                    questions = value;
                }
                else
                {
                    Debug.LogError("Attempted to set questions to null!");
                }
            }
        }

        // Optionally, a property to get a specific question
        public QuestionBaseSO this[int index]
        {
            get
            {
                if (index < 0 || index >= questions.Count)
                {
                    Debug.LogError($"Invalid question index: {index}");
                    return null;
                }
                return questions[index];
            }
        }
    }

    

}
