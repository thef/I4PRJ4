using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

//For using folders
using myWebApp.Pages.Account;

namespace myWebApp.Pages.Utilities
{
    public class FakeLogger : ILogger<RegisterModel>
    {
        public FakeLogger()
        {
              
        }

        public IDisposable BeginScope<TState>(TState state) {return null;}
    
        public bool IsEnabled(LogLevel logLevel) {return false;}

         public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter){}
         
    }
}