using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Globomantics.Models;
using Globomantics.Services;
using Globomantics.Core.Models;
using Globomantics.Constraints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http;
using IdentityModel.Client;
using System.Net;

namespace Globomantics.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private IRateService rateService;

       
        public HomeController(IRateService rateService, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            this.rateService = rateService;
        }

        public async Task<IActionResult> Index()
        {
            //var statusCode = await GetAuthors();

            //if (statusCode == System.Net.HttpStatusCode.Unauthorized
            //    || statusCode == System.Net.HttpStatusCode.Forbidden)
            //    return RedirectToAction("AccessDenied", "Account");
            await WriteOutIdentityInformation();
            return View();
        }

        [IsMobile]
        [ActionName("Index")]
        public IActionResult IndexMobile()
        {
            return Content("Mobile Page");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task WriteOutIdentityInformation()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            Debug.WriteLine($"Identity Token: {identityToken}");

            foreach(var claim in User.Claims)
            {
                Debug.WriteLine($"Claim Type: {claim.Type}  -  Claim Value: {claim.Value}");
            }
        }

        private async Task<HttpStatusCode> GetAuthors()
        {
            var authorClient = _httpClientFactory.CreateClient("AuthorClient");
            var response = await authorClient.GetAsync("api/authors");
            //response.EnsureSuccessStatusCode();
            return response.StatusCode;
           
            //if(response.Content.Headers.ContentType.MediaType == "application/vnd.khadoos.full+json")
            //var authors = JsonConvert.DeserializeObject<IEnumerable<AuthorFull>>(content);
        }
        private async Task<string> UserInfoAddress()
        {
            var idpClient = _httpClientFactory.CreateClient("IDPClient");

            var meteDataResponse = await idpClient.GetDiscoveryDocumentAsync();
            if (meteDataResponse.IsError)
                throw new Exception("Problem accessing discovery document", meteDataResponse.Exception);

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var userInfo = await idpClient.GetUserInfoAsync(
                new UserInfoRequest
                {
                    Address = meteDataResponse.UserInfoEndpoint,
                    Token = accessToken
                });

            if (userInfo.IsError)
                throw new Exception("Problem accessing user info endpoints", userInfo.Exception);

            var address = userInfo.Claims.FirstOrDefault(a => a.Type == "address")?.Value;
            
            return address;

        }
    }
}
