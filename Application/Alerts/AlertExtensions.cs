using System;
using SharedLibraryCore;
using SharedLibraryCore.Alerts;
using SharedLibraryCore.Database.Models;

namespace IW4MAdmin.Application.Alerts;

/// <summary>
/// extension method helper class to allow building of alerts
/// </summary>
public static class AlertExtensions
{
    /// <summary>
    /// builds basic alert for user with provided category
    /// </summary>
    /// <param name="client">client to build the alert for</param>
    /// <param name="type">alert category</param>
    /// <returns></returns>
    public static Alert.AlertState BuildAlert(this EFClient client, Alert.AlertCategory? type = null)
    {
        return new Alert.AlertState
        {
            RecipientId = client.ClientId,
            Category = type ?? Alert.AlertCategory.Information
        };
    }

    /// <summary>
    /// sets the category for an existing alert
    /// </summary>
    /// <param name="state">existing alert</param>
    /// <param name="category">new category</param>
    /// <returns></returns>
    public static Alert.AlertState WithCategory(this Alert.AlertState state, Alert.AlertCategory category)
    {
        state.Category = category;
        return state;
    }

    /// <summary>
    /// sets the alert type for an existing alert
    /// </summary>
    /// <param name="state">existing alert</param>
    /// <param name="type">new type</param>
    /// <returns></returns>
    public static Alert.AlertState OfType(this Alert.AlertState state, string type)
    {
        state.Type = type;
        return state;
    }

    /// <summary>
    /// sets the message for an existing alert
    /// </summary>
    /// <param name="state">existing alert</param>
    /// <param name="message">new message</param>
    /// <returns></returns>
    public static Alert.AlertState WithMessage(this Alert.AlertState state, string message)
    {
        state.Message = message;
        return state;
    }

    /// <summary>
    /// sets the expiration duration for an existing alert
    /// </summary>
    /// <param name="state">existing alert</param>
    /// <param name="expiration">duration before expiration</param>
    /// <returns></returns>
    public static Alert.AlertState ExpiresIn(this Alert.AlertState state, TimeSpan expiration)
    {
        state.ExpiresAt = DateTime.Now.Add(expiration);
        return state;
    }
    
    /// <summary>
    /// sets the source for an existing alert
    /// </summary>
    /// <param name="state">existing alert</param>
    /// <param name="source">new source</param>
    /// <returns></returns>
    public static Alert.AlertState FromSource(this Alert.AlertState state, string source)
    {
        state.Source = source;
        return state;
    }

    /// <summary>
    /// sets the alert source to the provided client
    /// </summary>
    /// <param name="state">existing alert</param>
    /// <param name="client">new client</param>
    /// <returns></returns>
    public static Alert.AlertState FromClient(this Alert.AlertState state, EFClient client)
    {
        state.Source = client.Name.StripColors();
        state.SourceId = client.ClientId;
        return state;
    }
}
