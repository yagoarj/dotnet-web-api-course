﻿using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;

namespace IWantApp.Endpoints;

public static class ProblemDetailsExtensions
{
    public static Dictionary<string, string[]> ConvertToProblemDetails(this IReadOnlyCollection<Notification> notifications)
    {
        return notifications
                .GroupBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Select(m => m.Message).ToArray());
    }

    public static Dictionary<string, string[]> ConvertToProblemDetails(this IEnumerable<IdentityError> errors)
    {
        var dictionary = new Dictionary<string, string[]>();

        dictionary.Add("Error", errors.Select(e => e.Description).ToArray());

        return dictionary;
    }
}
