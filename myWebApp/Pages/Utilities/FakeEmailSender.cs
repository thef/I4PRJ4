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
    public class FakeEmailSender : IEmailSender
    {
        public int nrCalled = 0;

        public FakeEmailSender()
        {
              
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            nrCalled ++;
        
            return null;
        }
    }
}