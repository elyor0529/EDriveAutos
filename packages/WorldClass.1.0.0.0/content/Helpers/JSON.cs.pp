using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace $rootnamespace$.Helpers
{
   public static class Ef2Json
   {
       private static readonly Type[] BuiltInTypes = new[]{
           typeof(bool),
           typeof(byte),
           typeof(sbyte),
           typeof(char),
           typeof(decimal),
           typeof(double),
           typeof(float),
           typeof(int),
           typeof(uint),
           typeof(long),
           typeof(ulong),
           typeof(short),
           typeof(ushort),
           typeof(string),
           typeof(DateTime),
           typeof(Guid),
           typeof(bool?),
           typeof(byte?),
           typeof(sbyte?),
           typeof(char?),
           typeof(decimal?),
           typeof(double?),
           typeof(float?),
           typeof(int?),
           typeof(uint?),
           typeof(long?),
           typeof(ulong?),
           typeof(short?),
           typeof(ushort?),
           typeof(DateTime?),
           typeof(Guid?)
       };

       public static IDictionary<string, object> Serialize(this object targetObject, int currentDepth = 1, List<int> alreadyProccededObjects = null, int maxDepth = 2)
       {            
           if (alreadyProccededObjects == null) { alreadyProccededObjects = new List<int>(); }
           alreadyProccededObjects.Add(targetObject.GetHashCode());

           Type type = targetObject.GetType();

           var properties = from p in type.GetProperties()
                            where p.CanWrite &&
                            BuiltInTypes.Contains(p.PropertyType)
                            select p;

           var result = properties.ToDictionary(
           property => property.Name,
           property => property.GetValue(targetObject, null) ?? ""
           );

           if (maxDepth > currentDepth)
           {
               var complexProperties = from p in type.GetProperties()
                                       where p.CanWrite &&
                                       p.CanRead &&
                                       !BuiltInTypes.Contains(p.PropertyType) &&
                                       !alreadyProccededObjects.Contains(p.GetValue(targetObject, null) == null ? 0 : p.GetValue(targetObject, null).GetHashCode())
                                       select p;

               foreach (var property in complexProperties)
               {
                   if (IsList(property))
                   {
                       var list = (from object c in (IEnumerable)property.GetValue(targetObject, null) where c != null
                                   select c.Serialize(currentDepth + 1, alreadyProccededObjects)).ToList();
                       result.Add(property.Name, list);
                   }
                   else
                   {           
                       var newObj = property.GetValue(targetObject, null).Serialize(currentDepth + 1, alreadyProccededObjects);
                       result.Add(property.Name, newObj);
                   }
               }
           }
           return result;
       }

       public static bool IsList(PropertyInfo pi)
       {            
           foreach (Type interfaceType in pi.PropertyType.GetInterfaces())
           {
               if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IList<>))
               {                    
                   return true;
               }
               if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(ICollection<>))
               {                 
                   return true;
               }
           }            
           return false;
       }
   }    
}