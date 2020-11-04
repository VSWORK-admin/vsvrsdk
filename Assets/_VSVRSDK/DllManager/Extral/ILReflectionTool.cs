using System;
using System.Reflection;

public static class ILReflectionTool
{
    public static object GetPrivateField(this object instance, string fieldname)
    {
        BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
        Type type = instance.GetType();
        FieldInfo field = type.GetField(fieldname, flag);
        if (field == null && type.BaseType != null)
        {
            field = type.BaseType.GetField(fieldname, flag);
            if (field == null && type.BaseType.BaseType != null)
                field = type.BaseType.BaseType.GetField(fieldname, flag);
        }
        return field.GetValue(instance);
    }
    public static void SetPrivateField(this object instance, string fieldname, object value)
    {
        BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
        Type type = instance.GetType();
        FieldInfo field = type.GetField(fieldname, flag);
        if (field == null && type.BaseType != null)
        {
            field = type.BaseType.GetField(fieldname, flag);
            if (field == null && type.BaseType.BaseType != null)
                field = type.BaseType.BaseType.GetField(fieldname, flag);
        }
        field.SetValue(instance, value);
    }
    public static object GetPrivateProperty(this object instance, string propertyname)
    {
        BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
        Type type = instance.GetType();
        PropertyInfo field = type.GetProperty(propertyname, flag);
        if (field == null && type.BaseType != null)
        {
            field = type.BaseType.GetProperty(propertyname, flag);
            if (field == null && type.BaseType.BaseType != null)
                field = type.BaseType.BaseType.GetProperty(propertyname, flag);
        }
        return field.GetValue(instance, null);
    }
    public static void SetPrivateProperty(this object instance, string propertyname, object value)
    {
        BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
        Type type = instance.GetType();
        PropertyInfo field = type.GetProperty(propertyname, flag);
        if (field == null && type.BaseType != null)
        {
            field = type.BaseType.GetProperty(propertyname, flag);
            if (field == null && type.BaseType.BaseType != null)
                field = type.BaseType.BaseType.GetProperty(propertyname, flag);
        }
        field.SetValue(instance, value, null);
    }
    public static object GetStaticField(Type type, string fieldname)
    {
        BindingFlags flag = BindingFlags.Static | BindingFlags.NonPublic;
        FieldInfo field = type.GetField(fieldname, flag);
        if (field == null && type.BaseType != null)
        {
            field = type.BaseType.GetField(fieldname, flag);
            if (field == null && type.BaseType.BaseType != null)
                field = type.BaseType.BaseType.GetField(fieldname, flag);
        }
        return field.GetValue(null);
    }
    public static void SetStaticField(Type type, string fieldname,object value)
    {
        BindingFlags flag = BindingFlags.Static | BindingFlags.NonPublic;
        FieldInfo field = type.GetField(fieldname, flag);
        if (field == null && type.BaseType != null)
        {
            field = type.BaseType.GetField(fieldname, flag);
            if (field == null && type.BaseType.BaseType != null)
                field = type.BaseType.BaseType.GetField(fieldname, flag);
        }
        field.SetValue(null, value);
    }
    public static object CallPrivateMethod(this object instance, string name, params object[] param)
    {
        BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
        Type type = instance.GetType();
        MethodInfo method = type.GetMethod(name, flag);
        if (method == null && type.BaseType != null)
        {
            method = type.BaseType.GetMethod(name, flag);
            if (method == null && type.BaseType.BaseType != null)
                method = type.BaseType.BaseType.GetMethod(name, flag);
        }
        return method.Invoke(instance, param);
    }
}