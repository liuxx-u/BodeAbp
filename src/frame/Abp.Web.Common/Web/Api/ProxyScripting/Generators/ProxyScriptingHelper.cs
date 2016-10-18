﻿using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Web.Api.Modeling;

namespace Abp.Web.Api.ProxyScripting.Generators
{
    internal static class ProxyScriptingHelper
    {
        public const string DefaultHttpVerb = "POST";

        public static string GenerateUrlWithParameters(ActionApiDescriptionModel action)
        {
            //TODO: Can be optimized using StringBuilder?
            var url = ReplacePathVariables(action.Url, action.Parameters);
            url = AddQueryStringParameters(url, action.Parameters);
            return url;
        }

        public static string GenerateHeaders(ActionApiDescriptionModel action, int indent = 0)
        {
            var parameters = action
                .Parameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Header)
                .ToArray();

            if (!parameters.Any())
            {
                return null;
            }

            return ProxyScriptingJsFuncHelper.CreateJsObjectLiteral(parameters, indent);
        }

        public static string GenerateBody(ActionApiDescriptionModel action)
        {
            var parameters = action
                .Parameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Body)
                .ToArray();

            if (parameters.Length <= 0)
            {
                return null;
            }

            if (parameters.Length > 1)
            {
                throw new AbpException(
                    $"Only one complex type allowed as argument to a controller action that's binding source is 'Body'. But {action.Name} ({action.Url}) contains more than one!"
                    );
            }

            return ProxyScriptingJsFuncHelper.GetParamNameInJsFunc(parameters[0]);
        }

        public static string GenerateFormPostData(ActionApiDescriptionModel action, int indent = 0)
        {
            var parameters = action
                .Parameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Form)
                .ToArray();

            if (!parameters.Any())
            {
                return null;
            }

            return ProxyScriptingJsFuncHelper.CreateJsObjectLiteral(parameters, indent);
        }

        private static string ReplacePathVariables(string url, IList<ParameterApiDescriptionModel> actionParameters)
        {
            var pathParameters = actionParameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Path)
                .ToArray();

            if (!pathParameters.Any())
            {
                return url;
            }

            foreach (var pathParameter in pathParameters)
            {
                url = url.Replace($"{{{pathParameter.Name}}}", $"' + {ProxyScriptingJsFuncHelper.GetParamNameInJsFunc(pathParameter)} + '");
            }

            return url;
        }

        private static string AddQueryStringParameters(string url, IList<ParameterApiDescriptionModel> actionParameters)
        {
            var queryStringParameters = actionParameters
                .Where(p => p.BindingSourceId.IsIn(ParameterBindingSources.ModelBinding, ParameterBindingSources.Query))
                .ToArray();

            if (!queryStringParameters.Any())
            {
                return url;
            }

            var qsBuilderParams = queryStringParameters
                .Select(p => $"{{ name: '{p.Name.ToCamelCase()}', value: {ProxyScriptingJsFuncHelper.GetParamNameInJsFunc(p)} }}")
                .JoinAsString(", ");

            return url + $"' + abp.utils.buildQueryString([{qsBuilderParams}]) + '";
        }

        public static string GetConventionalVerbForMethodName(string methodName)
        {
            if (methodName.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase))
            {
                return "GET";
            }

            if (methodName.StartsWith("Put", StringComparison.InvariantCultureIgnoreCase) ||
                methodName.StartsWith("Update", StringComparison.InvariantCultureIgnoreCase))
            {
                return "PUT";
            }

            if (methodName.StartsWith("Delete", StringComparison.InvariantCultureIgnoreCase) ||
                methodName.StartsWith("Remove", StringComparison.InvariantCultureIgnoreCase))
            {
                return "DELETE";
            }

            if (methodName.StartsWith("Patch", StringComparison.InvariantCultureIgnoreCase))
            {
                return "PATCH";
            }

            if (methodName.StartsWith("Post", StringComparison.InvariantCultureIgnoreCase) ||
                methodName.StartsWith("Create", StringComparison.InvariantCultureIgnoreCase) ||
                methodName.StartsWith("Insert", StringComparison.InvariantCultureIgnoreCase))
            {
                return "POST";
            }

            //Default
            return DefaultHttpVerb;
        }
    }
}
