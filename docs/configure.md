# Configure the HoloLens Application

The HoloLens Application uses the **appsettings.json** file in **Assets\StreamingAssets\Resources** to read the settings. This file is not in the repo and has to be added in the development environment and when building the package for the application.

The contents of the file is like shown below, where you need to replace the placeholders with the real information:

```json
{
    "ClientId": "<app-merck-ale-client ClientId>",
    "TenantId": "<app-merck-ale-client TenantId>",
    "Scopes": "<app-merck-ale-backend Scope>",
    "Resource": "<app-merck-ale-client Application ID URI>",
    "BaseEndPointUrl": "<Merck ALE backend url>",
    "SpeechServiceApiKey": "<speech service api key>",
    "SpeechServiceRegion": "<speech service region>"    
}
```

Let's have a look what these settings are:

| Setting             | Purpose                                                      |
| ------------------- | ------------------------------------------------------------ |
| ClientId            | This is the Client Id registered in the Azure AD under *Application Registrations* specific for the HoloLens Application. Example: "c3db312d-50ca-41ea-867d-01f9de720ab2". |
| TenantId            | This is the Tenant Id of the Azure AD used for authentication. This information is visible with the application registration as well. Example: "b8c9e247-c10e-4d9b-b652-4aa6ccb6845b". |
| Scopes              | This is the scope of the *Merck backend service* as configured in the Azure AD under *Application Registrations* specific for the Backend. Example: "api://ad0cdb4a-2a5d-4994-8fc3-15f75f2a7331/user_impersonation". |
| Resource            | This is the *Application ID URI* as configured in the Azure AD under *Application Registrations* specific for the Backend. Example: "api://ad0cdb4a-2a5d-4994-8fc3-15f75f2a7331". |
| BaseEndPointUrl     | This is the endpoint of the *Backend API App Service* to be used to retrieve data. This endpoint is protected with the application registration used above. Example: "https://app-aleapi-ale-test.azurewebsites.net". |
| SpeechServiceApiKey | This is the API Key used for access of a Cognitive Server setup in Azure for the Speech Service. Example: "e280f85abc57407c8f6746a94d563507". |
| SpeechServiceRegion | This is the region of the Cognitive Server setup in Azure for the Speech Service. Example: "eastus". |

## Generation of appsettings.json in CI/CD

In the CI/DC pipeline **.pipelines\holo-app.yml** the script **.pipelines\scripts\generate-holo-app-settings.sh** is used to generate the **appsettings.json** file in **src\Merck.ALE.HoloApp\Assets\StreamingAssets\Resources** for use in an automatic publish of the application package.
