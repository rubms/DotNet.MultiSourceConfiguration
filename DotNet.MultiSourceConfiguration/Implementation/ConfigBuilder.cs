using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace DotNet.MultiSourceConfiguration.Implementation
{
    class ConfigBuilder
    {
        private readonly AssemblyBuilder assemBuilder;
        private readonly AssemblyName assembly;
        private readonly Type baseType;

        private readonly ModuleBuilder moduleBuilder;
        private readonly MethodInfo getValueMethod;

        public static readonly ConfigBuilder Instance = new ConfigBuilder();

        private ConfigBuilder()
        {
            assembly = new AssemblyName("DynamicConfigAssembly-" + Guid.NewGuid().ToString());
            assemBuilder = Thread.GetDomain().DefineDynamicAssembly(assembly, AssemblyBuilderAccess.Run);
            moduleBuilder = assemBuilder.DefineDynamicModule("DynamicConfigModule");
            baseType = typeof(ConfigInterfaceImplBase);
            getValueMethod = baseType.GetMethod("GetValue", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public ConfigInterfaceImplBase BuildInterface<T>()
        {
            lock (this)
            {
                Type interfaceType = typeof (T);
                TypeBuilder typeBuilder = moduleBuilder.DefineType(typeof(T).Name + "Impl" + Guid.NewGuid().ToString(), TypeAttributes.Class, baseType);
                
                typeBuilder.AddInterfaceImplementation(interfaceType);
                foreach (PropertyInfo propertyInfo in interfaceType.GetProperties())
                {
                    var attr = GetFieldAttribute(propertyInfo);

                    MethodBuilder getterBuilder =
                        typeBuilder.DefineMethod(String.Format("get_{0}", propertyInfo.Name),
                                                 MethodAttributes.Private | MethodAttributes.Virtual);
                    getterBuilder.SetReturnType(propertyInfo.PropertyType);

                    CreateGetMethodBody(getterBuilder, attr.Property, propertyInfo.PropertyType);
                    typeBuilder.DefineMethodOverride(getterBuilder, propertyInfo.GetGetMethod());
                }
                return (ConfigInterfaceImplBase) Activator.CreateInstance(typeBuilder.CreateType());
            }
        }

        private void CreateGetMethodBody(MethodBuilder getterBuilder, string property, Type propertyType)
        {
            ILGenerator ilGen = getterBuilder.GetILGenerator();
            var generic = getValueMethod.MakeGenericMethod(propertyType);
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldstr, property);
            ilGen.EmitCall(OpCodes.Callvirt, generic, null);
            ilGen.Emit(OpCodes.Ret);
        }

        private static PropertyAttribute GetFieldAttribute(PropertyInfo propertyInfo)
        {
            object[] attributes = propertyInfo.GetCustomAttributes(typeof(PropertyAttribute), true);
            if (!attributes.Any())
                throw new InvalidOperationException("FieldName attribute is required on each property");
            PropertyAttribute attr = attributes.Cast<PropertyAttribute>().First();
            return attr;
        }
    }
}