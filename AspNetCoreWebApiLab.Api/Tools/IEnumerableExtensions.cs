using System;
using System.Dynamic;
using System.Reflection;
using System.Collections.Generic;

namespace AspNetCoreWebApiLab.Api.Tools
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<T>(this IEnumerable<T> source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentException(nameof(source));
            }

            var expandoObjectList = new List<ExpandoObject>();
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(",");

                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();
                    var flags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
                    var propertyInfo = typeof(T).GetProperty(propertyName, flags);

                    if (propertyInfo == null)
                    {
                        throw new ApplicationException($"Property {propertyName} wasn't found on {typeof(T)}");
                    }

                    propertyInfoList.Add(propertyInfo);
                }

                foreach (T sourceObject in source)
                {
                    var dataShapedObject = new ExpandoObject();

                    foreach (var propertyInfo in propertyInfoList)
                    {
                        var propertyValue = propertyInfo.GetValue(sourceObject);
                        ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                    }

                    expandoObjectList.Add(dataShapedObject);
                }
                
            }

            return expandoObjectList;
        }
    }
}
