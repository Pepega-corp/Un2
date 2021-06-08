using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using Unicon2.Formatting.Infrastructure.Services;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services;

namespace Unicon2.Formatting.Services
{
    //public class RuleContext
    //{
    //    public RuleContext(DeviceContext deviceContext, IPropertyValueService propertyValueService)
    //    {
    //        Resources = new ResourcesApi(deviceContext, propertyValueService);
    //        Utils = new UtilsApi();
    //    }

    //    public ResourcesApi Resources { get; }
    //    public UtilsApi Utils { get; }
    //}

    //public class ResourcesApi
    //{
    //    private DeviceContext _deviceContext;
    //    private readonly IPropertyValueService _propertyValueService;

    //    public ResourcesApi(DeviceContext deviceContext, IPropertyValueService propertyValueService)
    //    {
    //        _deviceContext = deviceContext;
    //        _propertyValueService = propertyValueService;
    //    }

    //    public async Task<bool> GetBitValue(string resourceName, int numberOfBit)
    //    {
    //        var resource = _deviceContext.DeviceSharedResources.SharedResourcesInContainers.FirstOrDefault(
    //            container => container.ResourceName == resourceName);
    //        var resUshorts = await _propertyValueService.GetUshortsOfProperty(resource.Resource, _deviceContext, true);

    //        return resUshorts.Item.First().GetBoolArrayFromUshort()[numberOfBit];
    //    }

    //    public async Task SetBitValue(string resourceName, int numberOfBit,bool valueOfBit)
    //    {
    //        var resource = _deviceContext.DeviceSharedResources.SharedResourcesInContainers.FirstOrDefault(
    //            container => container.ResourceName == resourceName);
    //        var resUshorts = await _propertyValueService.GetUshortsOfProperty(resource.Resource, _deviceContext, true);


    //        var boolArray = resUshorts.Item.GetBoolArrayFromUshortArray();
    //        boolArray[numberOfBit] = valueOfBit;

    //        var subPropertyUshort = boolArray.BoolArrayToUshort();


    //        _deviceContext.DeviceMemory.LocalMemoryValues[(resource.Resource as IWithAddress).Address] =
    //            subPropertyUshort;
    //        _deviceContext.DeviceEventsDispatcher.TriggerLocalAddressSubscription(
    //            (resource.Resource as IWithAddress).Address, 1);

    //    }

    //    public async Task<ushort[]> GetValueFromDevice(string resourceName)
    //    {
    //        var resource = _deviceContext.DeviceSharedResources.SharedResourcesInContainers.FirstOrDefault(
    //            container => container.ResourceName == resourceName);
    //        var resUshorts = await _propertyValueService.GetUshortsOfProperty(resource.Resource, _deviceContext, true);

    //        return resUshorts.Item;
    //    }
    //}


    //public class UtilsApi
    //{

    //}

    //public class CodeFormatterService : ICodeFormatterService
    //{
    //    private readonly IPropertyValueService _propertyValueService;

    //    public CodeFormatterService(IPropertyValueService propertyValueService)
    //    {
    //        _propertyValueService = propertyValueService;
    //    }

    //    static string code = @"
    //    using Unicon2.Formatting.Services;
    //    using System;
    //    using System.Threading.Tasks;

    //    public static class __CompiledExpr__
    //    {{
    //        public static async Task<{0}> Run({1})
    //        {{
    //            {2}
    //        }}
    //    }}
    //    ";

    //    static MethodInfo ToMethod(string expr, Type[] argTypes, string[] argNames, Type resultType)
    //    {
    //        StringBuilder argString = new StringBuilder();
    //        for (int i = 0; i < argTypes.Length; i++)
    //        {
    //            if (i != 0) argString.Append(", ");
    //            argString.AppendFormat("{0} {1}", argTypes[i].FullName, argNames[i]);
    //        }

    //        string finalCode = string.Format(code, resultType != null ? resultType.FullName : "void",
    //            argString, expr);
    //        var parameters = new CompilerParameters();
    //        parameters.ReferencedAssemblies.Add("mscorlib.dll");
    //        parameters.ReferencedAssemblies.Add(Path.GetFileName(Assembly.GetExecutingAssembly().Location));
    //        parameters.GenerateInMemory = true;
    //        var c = new CSharpCodeProvider();
    //        CompilerResults results = c.CompileAssemblyFromSource(parameters, finalCode);
    //        var asm = results.CompiledAssembly;
    //        var compiledType = asm.GetType("__CompiledExpr__");
    //        return compiledType.GetMethod("Run");
    //    }

    //    static Func<T1, T2, Task<TResult>> ToFunc<T1, T2, TResult>(string expr, string arg1Name, string arg2Name)
    //    {
    //        var method = ToMethod(expr, new Type[] {typeof(T1), typeof(T2)},
    //            new string[] {arg1Name, arg2Name}, typeof(TResult));
    //        return (T1 arg1, T2 arg2) => (Task<TResult>) method.Invoke(null, new object[] {arg1, arg2});
    //    }



    //    public Result<Func<ushort[], Task<double>>> GetFormatUshortsFunc(string codeString, DeviceContext deviceContext)
    //    {
    //        try
    //        {
    //            RuleContext context = new RuleContext(deviceContext, _propertyValueService);
    //            var f = ToFunc<ushort[], RuleContext, double>(codeString, "x",
    //                "context");
    //            return Result<Func<ushort[], Task<double>>>.Create(ushorts => f(ushorts, context), true);

    //        }
    //        catch (Exception e)
    //        {
    //            return Result<Func<ushort[], Task<double>>>.CreateWithException(e);
    //        }

    //    }

    //    public Result<Func<double, Task<ushort[]>>> GetFormatBackUshortsFunc(string codeString,
    //        DeviceContext deviceContext)
    //    {
    //        try
    //        {
    //            RuleContext context = new RuleContext(deviceContext, _propertyValueService);
    //            var f = ToFunc<double, RuleContext, ushort[]>(codeString, "x",
    //                "context");
    //            return Result<Func<double, Task<ushort[]>>>.Create(num => f(num, context), true);

    //        }
    //        catch (Exception e)
    //        {
    //            return Result<Func<double, Task<ushort[]>>>.CreateWithException(e);
    //        }
    //    }
    //}
}