using ezygamers.cmsv1;
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

        if (question.optionType == OptionType.LineQuestion && selectedAnswer.Equals(question.questionText.text))
        {
            return true;
        }

        return false;
    }
}
