﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SpendingTracker.GenericSubDomain.Common;

public static class EnumHelper<T> where T : struct, Enum
{
    public static IList<T> GetValues(Enum value)
    {
        return value
            .GetType()
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(fi => (T)Enum.Parse(value.GetType(), fi.Name, false))
            .ToList();
    }

    public static T Parse(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static IList<string> GetNames(Enum value)
    {
        return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
    }

    public static IList<string> GetDisplayValues(Enum value)
    {
        return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
    }

    private static string LookupResource(Type resourceManagerProvider, string resourceKey)
    {
        var resourceKeyProperty = resourceManagerProvider.GetProperty(resourceKey,
            BindingFlags.Static | BindingFlags.Public, null, typeof(string),
            Type.EmptyTypes, null);
        if (resourceKeyProperty != null)
        {
            return (string)resourceKeyProperty.GetMethod.Invoke(null, null);
        }

        return resourceKey; // Fallback with the key name
    }

    public static string GetDisplayValue(T value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());

        var descriptionAttributes = fieldInfo.GetCustomAttributes(
            typeof(DisplayAttribute), false) as DisplayAttribute[];

        if (descriptionAttributes[0].ResourceType != null)
            return LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

        if (descriptionAttributes == null) return string.Empty;
        return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
    }
}