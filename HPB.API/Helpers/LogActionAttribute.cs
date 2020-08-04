using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPB.API.Helpers
{
    public class LogActionAttribute : ActionFilterAttribute
    {
        private readonly ILoggerFactory _DepLoggerFactory;
        private ILogger _objLog4Net = null;
        string logLevel = string.Empty;
        //LogCritical
        //LogError
        //LogInformation
        //LogWarning
        public LogActionAttribute()
        {
           
        }
        public LogActionAttribute(ILoggerFactory DepLoggerFactory)
        {
            _DepLoggerFactory = DepLoggerFactory;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var parameter = "";
            var actionName = "";
            var controllerName = "";
            foreach (var actionParameter in filterContext.RouteData.Values)
            {
                if (!string.IsNullOrEmpty(parameter))
                    parameter += " , ";
                if (actionParameter.Key == "action")
                    actionName = actionParameter.Value.ToString();
                else if (actionParameter.Key == "controller")
                    controllerName = actionParameter.Value.ToString();
                else
                    parameter = actionParameter.Key.ToString() + " : " + actionParameter.Value.ToString();
            }
            _objLog4Net = _DepLoggerFactory.CreateLogger(controllerName);
            _objLog4Net.LogInformation(string.Concat("Begin ", controllerName, "'s ", actionName, " method at ( ", DateTime.Now, " )"));
            if (!string.IsNullOrEmpty(parameter))
                _objLog4Net.LogCritical(string.Concat("Entered ", actionName, " method . Param : " , parameter));
            else
                _objLog4Net.LogCritical(string.Concat("Entered ", actionName, " method "));
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var actionName = filterContext.RouteData.Values["action"].ToString();
            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var result = filterContext.Result;
            // Do something with Result.
            if (filterContext.Canceled == true)
            {
                    _objLog4Net.LogWarning(string.Concat("Canceled " , actionName, " method"));
            }

            if (filterContext.Exception != null)
            {
                    _objLog4Net.LogError(string.Concat("Error ", actionName, " method : ", filterContext.Exception.Message));
                filterContext.Exception = null;
            }
        _objLog4Net.LogInformation(string.Concat("End ", controllerName, "'s ", actionName, " method  at ( ", DateTime.Now, " )"));
        base.OnActionExecuted(filterContext);
        }
      
    }
}
