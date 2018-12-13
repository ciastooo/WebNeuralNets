﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using WebNeuralNets.BusinessLogic;

namespace WebNeuralNets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly string _requestLanugage;

        private readonly ITranslationHelper _translationHelper;

        public BaseController(ITranslationHelper translationHelper)
        {
            _requestLanugage = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.EnglishName;
            _translationHelper = translationHelper;
        }

        //private bool IsLoggedIn()
        //{

        //}

        //private string GetTranslation(string key)
        //{

        //}
    }
}