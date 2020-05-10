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
        private bool administratorsOnly;

        public CollaboratorAuthorizationAttribute(bool administratorsOnly = false) => this.administratorsOnly = administratorsOnly;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            collaboratorSession = 
                (CollaboratorSession)context.HttpContext.RequestServices.GetService(typeof(CollaboratorSession));
            
            Collaborator collaboratorFromSession = collaboratorSession.GetLoggedInCollaborator();

            if (collaboratorFromSession == null)
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }
            else if (administratorsOnly && !collaboratorFromSession.IsAdministrator)
            {
                // TODO - Configuration of ForbidResult() page;
                // context.Result = new ForbidResult();
                context.Result = new ContentResult() { Content = "Access denied." };
            }
        }
    }
}