using Microsoft.AspNetCore.Http;
using System;
public class MonitorAttribute : Attribute { }

public interface IMonitoringService
{
    bool Monitor(string httpMethod, PathString path);
}
