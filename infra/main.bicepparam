using './main.bicep'

// Azure region - choose one closest to your users
param location = 'eastus'

// App Service (backend API)
param appServiceName = 'checkmate-api'
param appServicePlanName = 'checkmate-plan'
param appServicePlanSku = 'B1'

// Storage Account (frontend static website)
// Must be globally unique, lowercase, 3-24 characters, no hyphens
param storageAccountName = 'checkmatefrontend'

// SQL Server and Database
param sqlServerName = 'checkmate-sql'
param sqlDatabaseName = 'CheckMate'
param sqlAdminLogin = 'checkmate-admin'
// sqlAdminPassword is intentionally omitted here - pass it at deploy time:
//   az deployment group create ... --parameters sqlAdminPassword=<password>

// Monitoring
param logAnalyticsWorkspaceName = 'checkmate-law'
param appInsightsName = 'checkmate-ai'
