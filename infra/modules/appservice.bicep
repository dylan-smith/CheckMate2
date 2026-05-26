@description('Azure region for resources.')
param location string

@description('Name of the App Service Plan.')
param planName string

@description('Name of the App Service (web app).')
param appName string

@description('SKU for the App Service Plan.')
param planSku string = 'B1'

@description('Application Insights connection string.')
param appInsightsConnectionString string

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: planName
  location: location
  sku: {
    name: planSku
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource appService 'Microsoft.Web/sites@2023-12-01' = {
  name: appName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|10.0'
      alwaysOn: true
      appSettings: [
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsightsConnectionString
        }
        {
          name: 'UseInMemoryDatabase'
          value: 'false'
        }
      ]
    }
  }
}

@description('Default HTTPS URL of the App Service.')
output appServiceUrl string = 'https://${appService.properties.defaultHostName}'
