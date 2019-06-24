export interface App {
  id: string,
  displayName?: string,
  languages: string[],
  defaultLanguage?: string
}

export interface Language {
  code: string,
  description: string
}