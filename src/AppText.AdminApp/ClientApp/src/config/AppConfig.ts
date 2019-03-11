export const AuthType = {
  DefaultCookie: 'DefaultCookie',
  Oidc: 'Oidc'
}

export interface AppConfig {
  clientBaseRoute: string,
  apiBaseUrl: string,
  authType: string,
  oidcSettings: {
    [key: string]: string
  }
}

declare global {
  interface Window { __AppTextConfig__: AppConfig; }
}

export const appConfig = window.__AppTextConfig__;