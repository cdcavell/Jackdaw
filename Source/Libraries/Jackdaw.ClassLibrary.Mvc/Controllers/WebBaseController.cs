﻿using Jackdaw.ClassLibrary.Common;
using Jackdaw.ClassLibrary.Common.Html;
using Jackdaw.ClassLibrary.Mvc.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Jackdaw.ClassLibrary.Mvc.Controllers
{
    /// <class>WebBaseController</class>
    /// <summary>
    /// Base controller class for web application
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.1 | 12/14/2021 | Initial Development |~ 
    /// </revision>
    [Controller]
    [Authorize]
    public abstract partial class WebBaseController<T> : Controller where T : WebBaseController<T>
    {
        /// <value>ILogger</value>
        protected readonly ILogger _logger;
        /// <value>IWebHostEnvironment</value>
        protected readonly IWebHostEnvironment _webHostEnvironment;
        /// <value>IWebHostEnvironment</value>
        protected readonly IHttpContextAccessor _httpContextAccessor;
        /// <value>IStringLocalizer</value>
        protected readonly IStringLocalizer<T> _localizer;
        /// <value>IStringLocalizer&lt;SharedResource&gt;</value>
        protected readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        
        /// <value>string</value>
        protected string _cultureName;
        /// <value>string</value>
        protected string _invalidModelState;

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="logger">ILogger</param>
        /// <param name="webHostEnvironment">IWebHostEnvironment</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        /// <param name="localizer">IStringLocalizer&lt;T&gt;</param>
        /// <param name="sharedLocalizer">IStringLocalizer&lt;SharedResource&gt;</param>
        /// <method>WebBaseController(ILogger&lt;T&gt; logger, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)</method>
        protected WebBaseController(
            ILogger<T> logger, 
            IWebHostEnvironment webHostEnvironment, 
            IHttpContextAccessor httpContextAccessor, 
            IStringLocalizer<T> localizer, 
            IStringLocalizer<SharedResource> sharedLocalizer
        )
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
            
            _cultureName = string.Empty;
            _invalidModelState = string.Empty;
        }

        /// <summary>
        /// Called before the action method is invoked
        /// </summary>
        /// <param name="context">ActionExecutingContext</param>
        /// <exception>ArgumentNullException</exception>
        /// <exception>NullReferenceException</exception>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            IRequestCultureFeature? requestCultureFeature = context.HttpContext.Features.Get<IRequestCultureFeature>();
            if (requestCultureFeature == null)
                throw new NullReferenceException();

            _cultureName = CultureHelper.GetImplementedCulture(requestCultureFeature.RequestCulture.Culture.Name);
            ViewBag.CultureName = _cultureName;

            _invalidModelState = _cultureName switch
            {
                "nl" => "Ongeldige modelstatus",
                "fr" => "État du modèle non valide",
                "es" => "Estado de modelo no válido",
                "ja" => "無効なモデル状態",
                "ar" => "حالة النموذج غير صالحة",
                _ => "Invalid Model State"
            };

            string currentUrl = UriHelper.GetDisplayUrl(Request).Trim('/');
            currentUrl += (currentUrl.Contains('?')) ? "&" : "?";
            ViewBag.CurrentUrl = currentUrl;

            base.OnActionExecuting(context);
        }

        /// <summary>
        /// Return BadRequestObjectResult containing ModelState error messages
        /// </summary>
        /// <returns>BadRequestObjectResult</returns>
        protected BadRequestObjectResult InvalidModelState()
        {
            string message = _invalidModelState;
            List<ModelErrorCollection> errors = ModelState.Values
                .Where(x => x.RawValue != null)
                .Select(x => x.Errors)
                .Where(x => x.Any())
                .ToList();

            foreach (ModelErrorCollection errorCollection in errors)
                foreach (ModelError error in errorCollection)
                    message += AsciiCodes.CRLF + error.ErrorMessage;

            return new BadRequestObjectResult(message);
        }

        /// <summary>
        /// Global model validation method (View found in HomeSite.ClassLibrary.Razor)
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>KeyValuePair&lt;int, string&gt;</returns>
        /// <method>ValidateModel&lt;M&gt;(M model)</method>
        /// <exception>ArgumentNullException</exception>
        protected KeyValuePair<int, string> ValidateModel<M>(M model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            int key = 0;
            string value = string.Empty;

            bool isValid = TryValidateModel(model);
            if (!isValid)
            {
                foreach (var modelValue in ModelState.Values)
                {
                    var errors = modelValue.Errors;
                    if (errors.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            key++;
                            value += Tags.Brackets(error.ErrorMessage) + Tags.LineBreak();
                        }
                    }
                }
            }

            return new KeyValuePair<int, string>(key, value);
        }

        /// <summary>
        /// Exception Message
        /// </summary>
        /// <returns>string</returns>
        protected string ExceptionMessage(Exception exception)
        {
            _logger.LogError(exception, $"ExceptionMessage");

            string errorMessage = $"Exception: {exception.Message}";
            if (exception.InnerException != null)
                errorMessage += $"{AsciiCodes.CRLF}Inner Exception: {exception.InnerException.Message}";

            return errorMessage;
        }

        /// <summary>
        /// Set current culture language
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpPost]
        protected IActionResult SetCulture(string culture)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Response.Cookies.Append(
                        CookieRequestCultureProvider.DefaultCookieName,
                        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                        new CookieOptions
                        {
                            SameSite = SameSiteMode.Strict,
                            IsEssential = true,
                            Secure = true,
                            HttpOnly = true
                        }
                    );

                    return Ok();
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, $"SetCulture(string {culture})");
                    return BadRequest(ExceptionMessage(exception));
                }
            }

            return InvalidModelState();
        }
    }
}
