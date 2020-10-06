export interface App {
  id: string,
  displayName?: string,
  languages: string[],
  defaultLanguage?: string,
  isSystemApp: boolean
}

export interface Language {
  code: string,
  description: string
}

export interface ApiKey {
  id: string,
  appId: string,
  name: string,
  createdAt: string
}

export interface CreateApiKeyCommand {
  appId: string,
  name: string
}