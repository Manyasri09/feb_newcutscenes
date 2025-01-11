using ezygamers.cmsv1;
using System;
using System.Diagnostics;

public class AnswerChecker
{
    //This method checks if the selected answer is correct
    public static bool CheckAnswer(QuestionBaseSO question, string selectedAnswer)
    {
        //If the question type is learning, return true
        if (question.optionType == OptionType.Learning)
        {
            return true;
        }

        //If the selected answer is equal to the correct option ID, return true
        if (question.correctOptionID.Equals(selectedAnswer))
        {
            return true;
        }


        //Debug.Log(selectedAnswer);
        //Debug.Log(question.questionText.text);
        //Print the question text and selected answer to the console
        Console.WriteLine(question.questionText.text);
        Console.WriteLine(selectedAnswer);
        //If the question type is line question and the selected answer is equal to the question text, return true
        if (question.optionType == OptionType.LineQuestion && selectedAnswer.Equals(question.questionText.text))
        {
            return true;
        }

        //Otherwise, return false
        return false;
    }
}
