targetScope = 'resourceGroup'

@description('Azure region for all resources. Defaults to the resource group location.')
param location string = resourceGroup().location

@description('Name of the Azure App Service (backend API).')
param appServiceName string

@description('Name of the Azure App Service Plan.')
param appServicePlanName string

@description('SKU for the App Service Plan (e.g. B1, S1, P1v3).')
@allowed(['B1', 'B2', 'B3', 'S1', 'S2', 'S3', 'P1v3', 'P2v3', 'P3v3'])
param appServicePlanSku string = 'B1'

@description('Name of the Azure Storage Account for the frontend static website (globally unique, lowercase, 3-24 characters).')
@minLength(3)
@maxLength(24)
param storageAccountName string

@description('Name of the Azure SQL Server.')
param sqlServerName string

@description('Name of the Azure SQL Database.')
param sqlDatabaseName string = 'CheckMate'

@description('SQL Server administrator login name.')
param sqlAdminLogin string

@description('SQL Server administrator login password.')
@secure()
param sqlAdminPassword string

@description('Name of the Log Analytics Workspace.')
param logAnalyticsWorkspaceName string

@description('Name of the Application Insights component.')
param appInsightsName string

module monitoring 'modules/monitoring.bicep' = {
  name: 'monitoring'
  params: {
    location: location
    workspaceName: logAnalyticsWorkspaceName
    appInsightsName: appInsightsName
  }
}

module appService 'modules/appservice.bicep' = {
  name: 'appservice'
  params: {
    location: location
    planName: appServicePlanName
    appName: appServiceName
    planSku: appServicePlanSku
    appInsightsConnectionString: monitoring.outputs.appInsightsConnectionString
  }
}

module sql 'modules/sql.bicep' = {
  name: 'sql'
  params: {
    location: location
    serverName: sqlServerName
    databaseName: sqlDatabaseName
    adminLogin: sqlAdminLogin
    adminPassword: sqlAdminPassword
  }
}

module storage 'modules/storage.bicep' = {
  name: 'storage'
  params: {
    location: location
    storageAccountName: storageAccountName
  }
}

@description('Default HTTPS URL of the backend App Service.')
output appServiceUrl string = appService.outputs.appServiceUrl

@description('Primary web endpoint for the frontend static website.')
output frontendWebEndpoint string = storage.outputs.primaryWebEndpoint

@description('Fully qualified domain name of the SQL Server.')
output sqlServerFqdn string = sql.outputs.sqlServerFqdn

@description('Application Insights connection string.')
output appInsightsConnectionString string = monitoring.outputs.appInsightsConnectionString
