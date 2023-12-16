using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.WebApp.Contracts;

namespace SpendingTracker.WebApp
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static MvcOptions UseDateOnlyTimeOnlyStringConverters(this MvcOptions options)
        {
            TypeDescriptor.AddAttributes(typeof(DateOnly), new TypeConverterAttribute(typeof(DateOnlyTypeConverter)));
            return options;
        }
    }
}
