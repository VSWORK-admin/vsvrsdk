using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class DllCLRBindings
    {


        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            UnityEngine_Debug_Binding.Register(app);
            UnityEngine_Component_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            HFExtralData_Binding.Register(app);
            ExtralData_Binding.Register(app);
            UnityEngine_GameObject_Binding.Register(app);
            ILReflectionTool_Binding.Register(app);
            System_Int32_Binding.Register(app);
            System_String_Binding.Register(app);
            UnityEngine_UI_Text_Binding.Register(app);
            System_Diagnostics_Stopwatch_Binding.Register(app);
            System_Int64_Binding.Register(app);
            System_Double_Binding.Register(app);
            UnityEngine_Random_Binding.Register(app);
            UnityEngine_Vector3_Binding.Register(app);
            UnityEngine_Quaternion_Binding.Register(app);
            UnityEngine_Transform_Binding.Register(app);
            System_DateTime_Binding.Register(app);
            System_Random_Binding.Register(app);
            System_Text_StringBuilder_Binding.Register(app);
            System_Char_Binding.Register(app);
            System_Object_Binding.Register(app);
            System_Int32_Array2_Binding.Register(app);
            System_Math_Binding.Register(app);
            UnityEngine_Time_Binding.Register(app);
            System_Single_Binding.Register(app);
            System_Boolean_Binding.Register(app);
            UnityEngine_Vector2_Binding.Register(app);

            ILRuntime.CLR.TypeSystem.CLRType __clrType = null;
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
        }
    }
}
