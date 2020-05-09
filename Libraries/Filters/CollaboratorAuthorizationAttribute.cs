using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineStore.Models;
using OnlineStore.Libraries.Session;

namespace OnlineStore.Libraries.Filters
{
    public class CollaboratorAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private CollaboratorSession collaboratorSession;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            collaboratorSession = 
                (CollaboratorSession)context.HttpContext.RequestServices.GetService(typeof(CollaboratorSession));
            
            Collaborator collaboratorFromSession = collaboratorSession.GetLoggedInCollaborator();

            if (collaboratorFromSession == null)
                context.Result = new RedirectToActionResult("Login", "Home", null);
        }
    }
}