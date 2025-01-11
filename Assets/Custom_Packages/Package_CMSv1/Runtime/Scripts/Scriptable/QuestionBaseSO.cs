using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezygamers.cmsv1
{
    [CreateAssetMenu(fileName = "Question Base", menuName = "Create QuestionBase")]
    public class QuestionBaseSO : ScriptableObject
    {
        public int questionNo;
        public string questionInfo;

        [Header("Question Content")]
        public TextContent questionText; //ul
        public TextContent hindiText;  //kl
        public ImageContent learningImage; //learning image data
        public AudioContent questionAudio; //audio

        [Header("Options Data")]
        public List<OptionGeneric> options = new List<OptionGeneric>(); //options with all fields

        [Header("Correct Answer")]
        public string correctOptionID;

        [Header("Option Type")]
        public OptionType optionType;
        public DifficultyLevel difficultyLevel;

    }

    public enum OptionType
    {
        Learning,
        TwoImageOpt,
        FourImageOpt,
        TwoWordOpt,
        FourWordOpt,
        AudioOpt,
        LineQuestion
    }
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

}
