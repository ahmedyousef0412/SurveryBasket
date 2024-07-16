

namespace SurveyBasket.Application.Services.Notifications;
public  interface INotificationService
{
    Task SendNewPollSNotification(int? pollId = null);
}
