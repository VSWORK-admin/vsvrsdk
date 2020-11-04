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
    unsafe class HFExtralData_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::HFExtralData);

            field = type.GetField("ExtralDatas", flag);
            app.RegisterCLRFieldGetter(field, get_ExtralDatas_0);
            app.RegisterCLRFieldSetter(field, set_ExtralDatas_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_ExtralDatas_0, AssignFromStack_ExtralDatas_0);


        }



        static object get_ExtralDatas_0(ref object o)
        {
            return ((global::HFExtralData)o).ExtralDatas;
        }

        static StackObject* CopyToStack_ExtralDatas_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::HFExtralData)o).ExtralDatas;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_ExtralDatas_0(ref object o, object v)
        {
            ((global::HFExtralData)o).ExtralDatas = (global::ExtralData[])v;
        }

        static StackObject* AssignFromStack_ExtralDatas_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::ExtralData[] @ExtralDatas = (global::ExtralData[])typeof(global::ExtralData[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::HFExtralData)o).ExtralDatas = @ExtralDatas;
            return ptr_of_this_method;
        }



    }
}
