using Microsoft.Security.Application;

namespace MVC.SkillsForce.Custom
{
    public class InputSanitizer
    {
        //TODO
        public string SanitizeInput(string userInput)
        {
            return Sanitizer.GetSafeHtml(userInput);
        }
    }
}