using Demo.Demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Demo.Attribute
{
    public class WebHandleErrorAttribute:System.Web.Mvc.HandleErrorAttribute
    {

        public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            string actionParameterInfo;
            try
            {
                actionParameterInfo = GetActionParametersInfoStr(filterContext);
            }
            catch
            {
                actionParameterInfo = "参数信息：不能处理参数绑定";
            }
            var routeValueDic = filterContext.RouteData.Values;
            Logger.Error("控制器：" + routeValueDic["controller"] + "/" + routeValueDic["action"] + "\r\n【参数信息】\r\n" + actionParameterInfo+"\r\n【错误详情】\r\n"+filterContext.Exception.Message);

            filterContext.ExceptionHandled = true;


            if (filterContext.RouteData.DataTokens["area"] != null)
            {
                if (filterContext.RouteData.DataTokens["area"].ToString() == "Admin")
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        JsonResult json = new JsonResult();
                        json.Data =
                            new { Message = "出错啦" };
                        filterContext.Result = json;
                    }
                    else
                    {
                       // filterContext.Result = redirect;
                    }
                }
            }
            else
            {
                if (filterContext.RequestContext.HttpContext.Request.Browser.IsMobileDevice)
                {
                    //filterContext.Result = redirecthomepage;
                }
                else
                {
                    //filterContext.Result = redirect;
                }
            }
           // base.OnException(filterContext);
        }
        /// <summary>
        /// 获得错误action的参数信息 与值
        /// 2016-09-22 新增 李强
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>

        public string GetActionParametersInfoStr(ExceptionContext filterContext)
        {
            var sb = new StringBuilder();
            var bind = new MyArrayBind();
            var c = new ModelBindingContext { ValueProvider = filterContext.Controller.ValueProvider };
            var type = filterContext.Controller.GetType();
            var methods = type.GetMethods();
            var actionMethod =
                methods.Where(b => b.Name.ToLower().Equals(filterContext.RouteData.Values["action"].ToString().ToLower())).ToList();
            MethodInfo processMethed = null;
            if (actionMethod.Count >= 2)
            {
                processMethed = filterContext.HttpContext.Request.HttpMethod.ToLower().Equals("get") ? actionMethod.FirstOrDefault(d => d.GetCustomAttribute<HttpGetAttribute>() != null) : actionMethod.FirstOrDefault(d => d.GetCustomAttribute<HttpPostAttribute>() != null);
            }
            else
            {
                processMethed = actionMethod[0];
            }
            bind.bindingContext = c;
            if (processMethed != null)
            {
                var parameters = processMethed.GetParameters();
                foreach (var parameterInfo in parameters)
                {

                    var obj = bind.BindingArray(parameterInfo.ParameterType, parameterInfo.Name);
                    string parameterValue = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    sb.Append(string.Format("参数名:{0}, 参数类型:{1}, 参数值:{2}\r\n", parameterInfo.Name, parameterInfo.ParameterType.ToString(), parameterValue));
                }
            }
            return sb.ToString();



        }
    }
}