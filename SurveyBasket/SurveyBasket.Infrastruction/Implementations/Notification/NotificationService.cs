﻿

namespace SurveyBasket.Infrastruction.Implementations.Notification;
public class NotificationService
    (
        ApplicationDbContext context ,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        IEmailSender emailSender
    ) : INotificationService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task SendNewPollSNotification(int? pollId = null)
    {
        IEnumerable<Poll> polls = [];

        //Select Single Poll
        if (pollId.HasValue)
        {
            var poll = await _context.Polls
                .SingleOrDefaultAsync(p => p.Id == pollId && p.IsPublished);

            polls = [poll!];
        }
        //Select All Polls
        else
        {
            polls = await _context.Polls
                .Where(p => p.IsPublished && p.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
                .AsNoTracking()
                .ToListAsync();
        }



        var users = await _userManager.Users.ToListAsync();

        var origin =  _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        foreach ( var poll in polls )
        {
            foreach (var user in users)
            {
                var placeholders = new Dictionary<string, string>
                {

                    {"{{name}}",user.FirstName},
                    {"{{pollTitle}}",poll.Title},
                    {"{{endDate}}",poll.EndsAt.ToString()},
                    {"{{url}}",$"{origin}/polls/start/{poll.Id}"}
                };

                var body = EmailBodyBuilder.GenerateEmailBody("PollNotification", placeholders);

                await _emailSender.SendEmailAsync(user.Email!, $"📣 Survey Basket: New Poll - {poll.Title}", body);
            }
        }
        
    }
}
