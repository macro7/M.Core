using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web.Services.Description;

namespace WK_Framework.Utils
{
    /// <summary>
    /// WebService帮助类
    /// </summary>
    public class WebServiceHelper
    {
        const string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";

        /// <summary>
        /// 打开WebService
        /// </summary>
        /// <param name="url">WSDL服务地址</param> 
        /// <returns></returns>
        public static Assembly OpenWebService(string url)
        {
            //获取WSDL
            string tempWSDL = "?WSDL".ToUpper();
            url = url.Length > tempWSDL.Length && url.Substring(url.Length - tempWSDL.Length, tempWSDL.Length).ToUpper().Equals(tempWSDL) ? url : url + tempWSDL;
            WebClient wc = new WebClient();
            Stream stream = wc.OpenRead(url);
            ServiceDescription sd = ServiceDescription.Read(stream);
            ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
            sdi.AddServiceDescription(sd, "", "");
            CodeNamespace cn = new CodeNamespace(@namespace);
            //生成客户端代理类代码          
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(cn);
            sdi.Import(cn, ccu);
            CSharpCodeProvider icc = new CSharpCodeProvider();
            //设定编译参数                 
            CompilerParameters cplist = new CompilerParameters();
            cplist.GenerateExecutable = false;
            cplist.GenerateInMemory = true;
            cplist.ReferencedAssemblies.Add("System.dll");
            cplist.ReferencedAssemblies.Add("System.XML.dll");
            cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
            cplist.ReferencedAssemblies.Add("System.Data.dll");
            //编译代理类                 
            CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
            if (true == cr.Errors.HasErrors)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                throw new Exception(sb.ToString());
            }
            //生成代理实例，并调用方法   
            Assembly WebServiceAssembly = cr.CompiledAssembly;
            return WebServiceAssembly;
        }

        /// <summary>           
        /// 动态调用web服务         
        /// </summary>          
        /// <param name="WebServiceAssembly">WSDL服务地址</param> 
        /// <param name="classname">类名</param>           
        /// <param name="methodname">方法名</param>           
        /// <param name="args">参数</param>           
        /// <returns></returns>          
        public static object InvokeWebService(Assembly WebServiceAssembly, string classname, string methodname, object[] args)
        {
            Type t = WebServiceAssembly.GetType(@namespace + "." + classname, true, true);
            object obj = Activator.CreateInstance(t);
            MethodInfo mi = t.GetMethod(methodname);
            return mi.Invoke(obj, args);
        }

        /// <summary>           
        /// 动态调用web服务         
        /// </summary>          
        /// <param name="WebServiceAssembly">WSDL服务地址</param> 
        /// <param name="methodname">方法名</param>           
        /// <param name="args">参数</param>           
        /// <returns></returns>          
        public static object InvokeWebService(Assembly WebServiceAssembly, string methodname, object[] args)
        {
            Type t = WebServiceAssembly.GetType();
            object obj = Activator.CreateInstance(t);
            MethodInfo mi = t.GetMethod(methodname);
            return mi.Invoke(obj, args);
        }
        ///// <summary>
        ///// 销毁静态打开的Webservice
        ///// </summary>
        ///// <returns></returns>
        //public static bool DisposeStaticWebService(Assembly WebServiceAssembly) {
        //    WebServiceAssembly = null;
        //    return true;
        //}

        /// <summary>           
        /// 动态调用web服务         
        /// </summary>          
        /// <param name="url">WSDL服务地址</param> 
        /// <param name="methodname">方法名</param>           
        /// <param name="args">参数</param>           
        /// <returns></returns>          
        public static object InvokeWebService(string url, string methodname, object[] args)
        {
            return WebServiceHelper.InvokeWebService(url, null, methodname, args);
        }

        /// <summary>          
        /// 动态调用web服务 
        /// </summary>          
        /// <param name="url">WSDL服务地址</param>
        /// <param name="classname">类名</param>  
        /// <param name="methodname">方法名</param>  
        /// <param name="args">参数</param> 
        /// <returns></returns>
        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            if ((classname == null) || (classname == ""))
            {
                classname = WebServiceHelper.GetWsClassName(url);
            }
            //获取WSDL
            string tempWSDL = "?WSDL".ToUpper();
            url = url.Length > tempWSDL.Length && url.Substring(url.Length - tempWSDL.Length, tempWSDL.Length).ToUpper().Equals(tempWSDL) ? url : url + tempWSDL;
            WebClient wc = new WebClient();
            Stream stream = wc.OpenRead(url);
            ServiceDescription sd = ServiceDescription.Read(stream);
            ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
            sdi.AddServiceDescription(sd, "", "");
            CodeNamespace cn = new CodeNamespace(@namespace);
            //生成客户端代理类代码          
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(cn);
            sdi.Import(cn, ccu);
            CSharpCodeProvider icc = new CSharpCodeProvider();
            //设定编译参数                 
            CompilerParameters cplist = new CompilerParameters();
            cplist.GenerateExecutable = false;
            cplist.GenerateInMemory = true;
            cplist.ReferencedAssemblies.Add("System.dll");
            cplist.ReferencedAssemblies.Add("System.XML.dll");
            cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
            cplist.ReferencedAssemblies.Add("System.Data.dll");
            //编译代理类                 
            CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
            if (true == cr.Errors.HasErrors)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                throw new Exception(sb.ToString());
            }
            //生成代理实例，并调用方法   
            System.Reflection.Assembly assembly = cr.CompiledAssembly;
            Type t = assembly.GetType(@namespace + "." + classname, true, true);
            object obj = Activator.CreateInstance(t);
            MethodInfo mi = t.GetMethod(methodname);
            return mi.Invoke(obj, args);
            // PropertyInfo propertyInfo = type.GetProperty(propertyname);     
            //return propertyInfo.GetValue(obj, null); 
        }

        private static string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }
    }
}
