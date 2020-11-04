using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class ExtralData_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::ExtralData);

            field = type.GetField("Target", flag);
            app.RegisterCLRFieldGetter(field, get_Target_0);
            app.RegisterCLRFieldSetter(field, set_Target_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_Target_0, AssignFromStack_Target_0);


        }



        static object get_Target_0(ref object o)
        {
            return ((global::ExtralData)o).Target;
        }

        static StackObject* CopyToStack_Target_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::ExtralData)o).Target;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Target_0(ref object o, object v)
        {
            ((global::ExtralData)o).Target = (UnityEngine.Transform)v;
        }

        static StackObject* AssignFromStack_Target_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Transform @Target = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::ExtralData)o).Target = @Target;
            return ptr_of_this_method;
        }



    }
}
