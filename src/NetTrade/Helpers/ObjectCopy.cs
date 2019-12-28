using System;
using System.Collections.Generic;
using System.Text;

namespace NetTrade.Helpers
{
    public static class ObjectCopy
    {
        public static void ShallowCopy<T>(T oldObject, T newObject)
        {
            var properties = oldObject.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (!property.CanRead || !property.CanWrite)
                {
                    continue;
                }

                var value = property.GetValue(oldObject);

                property.SetValue(newObject, value);
            }
        }
    }
}
