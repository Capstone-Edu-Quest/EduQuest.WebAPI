namespace EduQuest_Application.Abstractions.Email
{
    public interface IEmailService
    {
        Task SendEmailVerifyAsync(string subject, string recipientEmail, string username, string otp, string path, string logoPath);
        Task SendEmailWarningLearningPathOverDueAsync(string subject, string recipientEmail,
            string learningPath, string url, string course, string path, string logoPath);
    }
}
