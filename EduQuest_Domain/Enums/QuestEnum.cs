

namespace EduQuest_Domain.Enums;

public class QuestEnum
{
    public enum QuestType
    {
        STAGE = 1, // Complete X Stages
        STAGE_TIME = 2, // Complete X Stages in Y minutes

        MATERIAL = 3,// Complete X materials (Quiz, Video,٠٠٠)
        MATERIAL_TIME = 4,// Complete X materials in Y minutes

        QUIZ = 5,// Complete X Quizzes
        QUIZ_TIME = 6,// Complete X Quizzes in Y minutes

        COURSE = 7,// Complete X Courses
        COURSE_TIME = 8,// Complete X Courses in Y minutes

        LEARNING_TIME = 9,// Spend X minutes learning
        LEARNING_TIME_TIME = 10,// Spend X minutes learning in Y minutes 

        STREAK = 11 // Study X days
    }
    public enum RewardType
    {
        Gold = 1,
        Exp = 2,
        Item = 3,
        Coupon = 4,
        Booster = 5,
    }
    public enum ResetType
    {
        Daily = 1,
        OneTime = 2
    }
}
