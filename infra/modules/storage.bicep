@description('Azure region for resources.')
param location string

@description('Name of the Azure Storage Account (must be globally unique, lowercase, 3-24 characters).')
@minLength(3)
@maxLength(24)
param storageAccountName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
    minimumTlsVersion: 'TLS1_2'
    accessTier: 'Hot'
  }
}

resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2023-05-01' = {
  parent: storageAccount
  name: 'default'
  properties: {
    // staticWebsite is a valid ARM property but absent from Bicep type definitions (type gap)
    // See: https://aka.ms/bicep-type-issues
    #disable-next-line BCP037
    staticWebsite: {
      enabled: true
      indexDocument: 'index.html'
      error404Document: 'index.html'
    }
  }
}

@description('Primary web endpoint for the static website.')
output primaryWebEndpoint string = storageAccount.properties.primaryEndpoints.web
