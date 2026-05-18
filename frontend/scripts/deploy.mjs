import { BlobServiceClient } from '@azure/storage-blob'
import { DefaultAzureCredential } from '@azure/identity'
import { readdir, readFile } from 'fs/promises'
import { join, extname, relative } from 'path'
import { fileURLToPath } from 'url'

const ACCOUNT_NAME = process.env.AZURE_STORAGE_ACCOUNT_NAME
if (!ACCOUNT_NAME) {
  console.error('AZURE_STORAGE_ACCOUNT_NAME environment variable is required')
  process.exit(1)
}

const DIST_DIR = fileURLToPath(new URL('../dist', import.meta.url))
const CONTAINER = '$web'

const CONTENT_TYPES = {
  '.html': 'text/html',
  '.css': 'text/css',
  '.js': 'application/javascript',
  '.mjs': 'application/javascript',
  '.json': 'application/json',
  '.png': 'image/png',
  '.jpg': 'image/jpeg',
  '.jpeg': 'image/jpeg',
  '.gif': 'image/gif',
  '.svg': 'image/svg+xml',
  '.ico': 'image/x-icon',
  '.woff': 'font/woff',
  '.woff2': 'font/woff2',
  '.ttf': 'font/ttf',
  '.txt': 'text/plain',
}

async function* walk(dir) {
  const entries = await readdir(dir, { withFileTypes: true })
  for (const entry of entries) {
    const fullPath = join(dir, entry.name)
    if (entry.isDirectory()) {
      yield* walk(fullPath)
    } else {
      yield fullPath
    }
  }
}

async function main() {
  const credential = new DefaultAzureCredential()
  const serviceClient = new BlobServiceClient(
    `https://${ACCOUNT_NAME}.blob.core.windows.net`,
    credential,
  )
  const containerClient = serviceClient.getContainerClient(CONTAINER)

  for await (const filePath of walk(DIST_DIR)) {
    const blobName = relative(DIST_DIR, filePath).replace(/\\/g, '/')
    const ext = extname(filePath).toLowerCase()
    const contentType = CONTENT_TYPES[ext] ?? 'application/octet-stream'
    const content = await readFile(filePath)

    await containerClient.getBlockBlobClient(blobName).uploadData(content, {
      blobHTTPHeaders: { blobContentType: contentType },
    })

    console.log(`Uploaded: ${blobName}`)
  }

  console.log('Deployment complete.')
}

main().catch((err) => {
  console.error(err)
  process.exit(1)
})
