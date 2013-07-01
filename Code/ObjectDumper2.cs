using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Security.Principal;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace TwilioEmulator.Code
{
    public static class ObjectDumper2
    {
        public static void Dump(object value, string name, TextWriter writer, bool CompactMode)
        {
            //if (ObjectDumper2.IsNullOrWhiteSpace(name))
            //    throw new ArgumentNullException("name");
            if (writer == null)
                throw new ArgumentNullException("writer");
            ObjectIDGenerator idGenerator = new ObjectIDGenerator();
            ObjectDumper2.InternalDump(0, name, value, writer, idGenerator, true, CompactMode);
        }


        private static void InternalDump(int indentationLevel, string name, object value, TextWriter writer, ObjectIDGenerator idGenerator, bool recursiveDump, bool CompactMode)
        {
            string str1 = new string(' ', indentationLevel * 3);
            if (value == null)
            {
                if (CompactMode)
                    return;
                writer.WriteLine("{0}{1} = <null>", (object)str1, (object)name);
            }
            else
            {
                Type type = value.GetType();
                string str2 = string.Empty;
                string str3 = string.Empty;
                if (!type.IsValueType)
                {
                    bool firstTime;
                    long id = idGenerator.GetId(value, out firstTime);
                    if (!firstTime)
                        str2 = string.Format((IFormatProvider)CultureInfo.InvariantCulture, " (see #{0})", new object[1]
            {
              (object) id
            });
                    else
                        str3 = string.Format((IFormatProvider)CultureInfo.InvariantCulture, "#{0}: ", new object[1]
            {
              (object) id
            });
                }
                bool flag = value is string;
                string fullName = value.GetType().FullName;
                string str4 = value.ToString();
                Exception exception = value as Exception;
                if (exception != null)
                    str4 = exception.GetType().Name + ": " + exception.Message;
                string str5;
                if (str4 == fullName)
                {
                    str5 = string.Empty;
                }
                else
                {
                    string str6 = str4.Replace("\t", "\\t").Replace("\n", "\\n").Replace("\r", "\\r");
                    int length = str6.Length;
                    //if (length > 80)
                    //    str6 = str6.Substring(0, 80);
                    if (flag)
                        str6 = string.Format((IFormatProvider)CultureInfo.InvariantCulture, "\"{0}\"", new object[1]
            {
              (object) str6
            });
            //        if (length > 80)
            //            str6 = string.Concat(new object[4]
            //{
            //  (object) str6,
            //  (object) " (+",
            //  (object) (length - 80),
            //  (object) " chars)"
            //});
                    str5 = " = " + str6;
                }
                writer.WriteLine("{0}{1}{2}{3} [{4}]{5}", (object)str1, (object)str3, (object)name, (object)str5, (object)value.GetType(), (object)str2);
                if (str2.Length > 0 || flag || (type.IsValueType && type.FullName == "System." + type.Name || type.FullName == "System.Reflection." + type.Name || (value is SecurityIdentifier || !recursiveDump)))
                    return;
                if (value is NameValueCollection)
                {
                    var v = (NameValueCollection)value;
                    v.AllKeys.ToList().ForEach(x =>
                        {
                            InternalDump(indentationLevel + 2, x, v[x], writer, idGenerator, true, CompactMode);
                        });
                }
                else
                {
                    WriteObject(indentationLevel, value, writer, idGenerator, CompactMode, str1, type);
                }
                writer.WriteLine("{0}}}", str1);
            }
        }

        private static void WriteObject(int indentationLevel, object value, TextWriter writer, ObjectIDGenerator idGenerator, bool CompactMode, string str1, Type type)
        {
            PropertyInfo[] propertyInfoArray = Enumerable.ToArray<PropertyInfo>(Enumerable.Where<PropertyInfo>((IEnumerable<PropertyInfo>)type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), (Func<PropertyInfo, bool>)(property => property.GetIndexParameters().Length == 0 && property.CanRead)));
            FieldInfo[] fieldInfoArray = Enumerable.ToArray<FieldInfo>((IEnumerable<FieldInfo>)type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            if (propertyInfoArray.Length == 0 && fieldInfoArray.Length == 0)
               return;
            writer.WriteLine("{0}{{", str1);
            if (propertyInfoArray.Length > 0)
            {
                writer.WriteLine("{0}   properties {{", str1);
                foreach (PropertyInfo propertyInfo in propertyInfoArray)
                {
                    try
                    {
                        object obj = propertyInfo.GetValue(value, (object[])null);
                        ObjectDumper2.InternalDump(indentationLevel + 2, propertyInfo.Name, obj, writer, idGenerator, true, CompactMode);
                    }
                    catch (TargetInvocationException ex)
                    {
                        ObjectDumper2.InternalDump(indentationLevel + 2, propertyInfo.Name, (object)ex, writer, idGenerator, false, CompactMode);
                    }
                    catch (ArgumentException ex)
                    {
                        ObjectDumper2.InternalDump(indentationLevel + 2, propertyInfo.Name, (object)ex, writer, idGenerator, false, CompactMode);
                    }
                    catch (RemotingException ex)
                    {
                        ObjectDumper2.InternalDump(indentationLevel + 2, propertyInfo.Name, (object)ex, writer, idGenerator, false, CompactMode);
                    }
                }
                writer.WriteLine("{0}   }}", str1);
            }
            if (fieldInfoArray.Length > 0 && !CompactMode)
            {
                writer.WriteLine("{0}   fields {{", str1);
                foreach (FieldInfo fieldInfo in fieldInfoArray)
                {
                    try
                    {
                        object obj = fieldInfo.GetValue(value);
                        ObjectDumper2.InternalDump(indentationLevel + 2, fieldInfo.Name, obj, writer, idGenerator, true, CompactMode);
                    }
                    catch (TargetInvocationException ex)
                    {
                        ObjectDumper2.InternalDump(indentationLevel + 2, fieldInfo.Name, (object)ex, writer, idGenerator, false, CompactMode);
                    }
                }
                writer.WriteLine("{0}   }}", str1);

            }
        }

        public static bool IsNullOrWhiteSpace(string value)
        {
            return value == null || value.Trim().Length == 0;
        }
    }
}
