using System.Net.Http;
using Xunit;
using AccountAPI.Tests.T2.Framework.Helpers;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;

namespace AccountAPI.Tests.T2.Framework
{
    public abstract class ControllerBase
    {
        protected readonly HttpClient _client;
        protected readonly WebHelper _webHelper;
        protected readonly UserHelper _userHelper;

        protected ControllerBase()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:80/")
            };
            _webHelper = new WebHelper(_client);
            _userHelper = new UserHelper();
        }
    }
}