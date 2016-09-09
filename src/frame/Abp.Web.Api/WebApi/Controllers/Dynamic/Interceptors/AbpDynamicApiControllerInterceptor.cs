using System.Linq;
using System.Reflection;
using Abp.Dependency;
using Abp.Extensions;
using Abp.WebApi.Configuration;
using Abp.WebApi.Controllers.Dynamic.Builders;
using Castle.DynamicProxy;

namespace Abp.WebApi.Controllers.Dynamic.Interceptors
{
    /// <summary>
    /// Interceptor dynamic controllers.
    /// It handles method calls to a dynmaic generated api controller and 
    /// calls underlying proxied object.
    /// </summary>
    /// <typeparam name="T">Type of the proxied object</typeparam>
    public class AbpDynamicApiControllerInterceptor<T> : IInterceptor
    {
        /// <summary>
        /// Real object instance to call it's methods when dynamic controller's methods are called.
        /// </summary>
        private readonly T _proxiedObject;

        /// <summary>
        /// Creates a new AbpDynamicApiControllerInterceptor object.
        /// </summary>
        /// <param name="proxiedObject">Real object instance to call it's methods when dynamic controller's methods are called</param>
        public AbpDynamicApiControllerInterceptor(T proxiedObject)
        {
            _proxiedObject = proxiedObject;
        }

        /// <summary>
        /// Intercepts method calls of dynamic api controller
        /// </summary>
        /// <param name="invocation">Method invocation information</param>
        public void Intercept(IInvocation invocation)
        {
            //If method call is for generic type (T)...
            if (DynamicApiControllerActionHelper.IsMethodOfType(invocation.Method, typeof(T)))
            {
                //Call real object's method
                try
                {
                    var _config = IocManager.Instance.Resolve<IAbpWebApiModuleConfiguration>();
                    //if (_config.UseRpc)
                    //{
                    //    var serviceProxyGenerater = IocManager.Instance.Resolve<IServiceProxyGenerater>();
                    //    var serviceProxyFactory = IocManager.Instance.Resolve<IServiceProxyFactory>();
                    //    var proxyService = serviceProxyGenerater.GenerateProxys(null).Single(invocation.Method.DeclaringType.IsAssignableFrom);
                    //    var instance = serviceProxyFactory.CreateProxy(proxyService);
                    //    invocation.ReturnValue = proxyService.GetMethods().Single(p => p.Name == invocation.Method.Name).Invoke(instance, invocation.Arguments);
                    //}
                    //else
                    //{
                        invocation.ReturnValue = invocation.Method.Invoke(_proxiedObject, invocation.Arguments);
                    //}
                }
                catch (TargetInvocationException targetInvocation)
                {
                    if (targetInvocation.InnerException != null)
                    {
                        targetInvocation.InnerException.ReThrow();
                    }

                    throw;
                }
            }
            else
            {
                //Call api controller's methods as usual.
                invocation.Proceed();
            }
        }
    }
}