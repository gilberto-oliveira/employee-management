using System.Reflection;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace EmployeeManagement.Api.Features.Patch;

public class PatchIgnoreCaseAdapter : IObjectAdapter
{
    public void Add(Operation operation, object objectToApplyTo)
    {
        Replace(operation, objectToApplyTo);
    }

    public void Copy(Operation operation, object objectToApplyTo)
    {
        var sourcePropertyInfo = GetPropertyInfo(objectToApplyTo, operation.from);
        var destinationPropertyInfo = GetPropertyInfo(objectToApplyTo, operation.path);

        if (sourcePropertyInfo != null && destinationPropertyInfo != null)
        {
            var sourceValue = sourcePropertyInfo.GetValue(objectToApplyTo);
            destinationPropertyInfo.SetValue(objectToApplyTo, sourceValue);
        }
    }

    public void Move(Operation operation, object objectToApplyTo)
    {
        var sourcePropertyInfo = GetPropertyInfo(objectToApplyTo, operation.from);
        var destinationPropertyInfo = GetPropertyInfo(objectToApplyTo, operation.path);

        if (sourcePropertyInfo != null && destinationPropertyInfo != null)
        {
            var sourceValue = sourcePropertyInfo.GetValue(objectToApplyTo);
            destinationPropertyInfo.SetValue(objectToApplyTo, sourceValue);
            sourcePropertyInfo.SetValue(objectToApplyTo, null); // set the source property to null after moving its value
        }
    }

    public void Remove(Operation operation, object objectToApplyTo)
    {
        var propertyInfo = GetPropertyInfo(objectToApplyTo, operation.path);
        propertyInfo?.SetValue(objectToApplyTo, null);
    }

    public void Replace(Operation operation, object objectToApplyTo)
    {
        var propertyInfo = GetPropertyInfo(objectToApplyTo, operation.path);
        if (propertyInfo == null) return;
        if (propertyInfo.PropertyType == typeof(DateOnly) && operation.value is not null)
        {
            propertyInfo.SetValue(objectToApplyTo, DateOnly.Parse(operation.value.ToString()!, System.Globalization.CultureInfo.InvariantCulture));
            return;
        }
        if (propertyInfo.PropertyType == typeof(DateTime) && operation.value is not null)
        {
            propertyInfo.SetValue(objectToApplyTo, DateTime.Parse(operation.value.ToString()!, System.Globalization.CultureInfo.InvariantCulture));
            return;
        }

        propertyInfo.SetValue(objectToApplyTo, operation.value);
    }

    private static PropertyInfo? GetPropertyInfo(object objectToApplyTo, string path)
    {
        return Array.Find(objectToApplyTo.GetType().GetProperties(), p => string.Compare(p.Name, path, true) == 0);
    }
}