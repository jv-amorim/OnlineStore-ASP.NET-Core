using System.ComponentModel.DataAnnotations;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Libraries.Language;

namespace OnlineStore.Libraries.Validation.Collaborator
{
    public class UniqueEmailAddressAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string emailAddress = (value as string).Trim();
            var collaboratorToValidate = (Models.Collaborator) validationContext.ObjectInstance;

            var collaboratorRepository = 
                (ICollaboratorRepository) validationContext.GetService(typeof(ICollaboratorRepository));
            var collaboratorWithTheWantedEmail = 
                collaboratorRepository.GetCollaboratorByEmail(emailAddress);

            if (collaboratorWithTheWantedEmail != null)
                if (collaboratorToValidate.Id != collaboratorWithTheWantedEmail.Id)
                    return new ValidationResult(Message.MSG_ERROR_009);

            return ValidationResult.Success;
        }
    }
}