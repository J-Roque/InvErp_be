using System.Text.Json;
using Mail.Constants;
using Mail.Models;
using RazorLight;

namespace Mail.Services;

public static class MailService
{
    private static readonly RazorLightEngine Engine = new RazorLightEngineBuilder()
        .UseFileSystemProject(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates"))
        .UseMemoryCachingProvider()
        .Build();

    public static async Task<string> GetTemplateData(string templateType, string jsonSerializedData)
    {
        var templatePath = GetTemplatePath(templateType);
        return await RenderTemplate(templatePath, templateType, jsonSerializedData);
    }

    private static string GetTemplatePath(string template)
    {
        var templatePath = template.ToLower() switch
        {
            TemplateTypes.ChangePasswordRequest => "ChangePasswordRequest.cshtml",
            TemplateTypes.NewUserCreated => "NewUserCreated.cshtml",
            TemplateTypes.PasswordChangeSuccess => "PasswordChangeSuccess.cshtml",
            //TemplateTypes.ProfileCreationSuccess => "ProfileCreationSuccess.cshtml",
            //TemplateTypes.ProviderRegistrationSuccess => "ProviderRegistrationSuccess.cshtml",
            //TemplateTypes.RoleCreationSuccess => "RoleCreationSuccess.cshtml",
            //TemplateTypes.SendCartProvider => "SendCartProvider.cshtml",
            //TemplateTypes.QuoteEmail => "QuoteEmail.cshtml",
            //TemplateTypes.RequirementApproved => "RequirementApproved.cshtml",
            _ => throw new ArgumentException("Template no válido")
        };

        if (templatePath == null)
        {
            throw new ArgumentException("No se encontró el template");
        }
    
        return templatePath;
    }

    private static async Task<string> RenderTemplate(string templatePath, string templateType, string jsonSerializedData)
    {
        object? newData = templateType switch
        {
            TemplateTypes.ChangePasswordRequest => JsonSerializer.Deserialize<ChangePasswordRequestModel>(jsonSerializedData),
            TemplateTypes.NewUserCreated => JsonSerializer.Deserialize<NewUserCreatedModel>(jsonSerializedData),
            TemplateTypes.PasswordChangeSuccess => JsonSerializer.Deserialize<PasswordChangeSuccessModel>(jsonSerializedData),
            //TemplateTypes.ProfileCreationSuccess => JsonSerializer.Deserialize<ProfileCreationSuccessModel>(jsonSerializedData),
            //TemplateTypes.ProviderRegistrationSuccess => JsonSerializer.Deserialize<ProviderRegistrationSuccessModel>(jsonSerializedData),
            //TemplateTypes.RoleCreationSuccess => JsonSerializer.Deserialize<RoleCreationSuccessModel>(jsonSerializedData),
            //TemplateTypes.SendCartProvider => JsonSerializer.Deserialize<HomologacionInvitacionModel>(jsonSerializedData),
            //TemplateTypes.QuoteEmail => JsonSerializer.Deserialize<QuoteEmailModel>(jsonSerializedData),
            //TemplateTypes.RequirementApproved => JsonSerializer.Deserialize<RequirementApprovedModel>(jsonSerializedData),
            _ => throw new ArgumentException("templateType no válido")
        };

        if (newData == null)
        {
            throw new ArgumentException("No se pudo deserializar el modelo");
        }

        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", templatePath);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"No se encontró el template: {fullPath}");

        var templateContent = await File.ReadAllTextAsync(fullPath);
        return await Engine.CompileRenderStringAsync(templateType, templateContent, newData);
    }
}