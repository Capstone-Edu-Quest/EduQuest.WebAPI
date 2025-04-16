namespace EduQuest_Domain.Models.Notification;

public class NotificationDto
{
    public string userId { get; set; }
    public string Content {  get; set; }
    public string Receiver {  get; set; }
    public string Url {  get; set; }
    public Dictionary<string, string>? Values { get; set; }
}
