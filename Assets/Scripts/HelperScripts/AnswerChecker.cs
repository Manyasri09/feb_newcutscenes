using ezygamers.cmsv1;
using System;
using System.Diagnostics;

public class AnswerChecker
{
    public static bool CheckAnswer(QuestionBaseSO question, string selectedAnswer)
    {
        if (question.optionType == OptionType.Learning)
        {
            return true;
        }

        if (question.correctOptionID.Equals(selectedAnswer))
        {
            return true;
        }


        //Debug.Log(selectedAnswer);
        //Debug.Log(question.questionText.text);
        Console.WriteLine(question.questionText.text);
        Console.WriteLine(selectedAnswer);
        if (question.optionType == OptionType.LineQuestion && selectedAnswer.Equals(question.questionText.text))
        {
            return true;
        }

        return false;
    }
}
