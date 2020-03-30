using Microsoft.AspNetCore.Mvc.Controllers;
using System.Linq;
using static Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescriptionGroup;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.AspNetCore.Routing;
public class MonitoringService: IMonitoringService
{
    private (string httpMethod, TemplateMatcher matcher)[] _matchers;

    public MonitoringService(IApiDescriptionGroupCollectionProvider provider)
    {
        _matchers =
            provider
                .ApiDescriptionGroups
                .Items
                .SelectMany(group => group.Items)
                .Where(x =>
                    x.ActionDescriptor is ControllerActionDescriptor
                    && ((ControllerActionDescriptor)x.ActionDescriptor).MethodInfo.CustomAttributes.Any(attr => attr.AttributeType == typeof(MonitorAttribute)))
                .Select(desc => {
                    var routeTemplate = TemplateParser.Parse(desc.RelativePath);
                    var routeValues = new RouteValueDictionary (routeTemplate
                        .Parameters
                        .ToDictionary(x => x.Name, y => y.DefaultValue));
                    var matcher = new TemplateMatcher(routeTemplate, routeValues);
                    return (desc.HttpMethod, matcher);
                })
                .ToArray();
    }

    public bool Monitor(string httpMethod, PathString path)
    {
        return _matchers.Any(m => m.httpMethod == httpMethod && m.matcher.TryMatch(path, new RouteValueDictionary()));
    }
}
