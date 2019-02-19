import { appConfig, AuthType } from '../config/AppConfig';

import { UserManager, User } from 'oidc-client';

export interface UserContext {
  authType: string,
  isLoggedIn: boolean,
  claims: any,
  accessToken?: string
}

let oidcUserManager: UserManager = null;

let userContext: UserContext = {
  authType: appConfig.authType,
  isLoggedIn: false,
  claims: {}
}

function updateUserContextFromOidcUser(user: User) {
  console.log('OIDC user => ', user);
  if (user) {
    userContext.isLoggedIn = true;
    userContext.claims = user.profile;
    userContext.accessToken = user.access_token;  
  }
}

export function initUserContext(): Promise<any> {
  if (appConfig.authType === AuthType.Oidc) {
    oidcUserManager = new UserManager(appConfig.oidcSettings);
    return oidcUserManager.getUser()
      .then(updateUserContextFromOidcUser);
  }
  else {
    userContext.isLoggedIn = true;
    return Promise.resolve();
  }
}

export function startAuthentication(): Promise<any> {
  if (appConfig.authType == AuthType.Oidc && oidcUserManager) {
    return oidcUserManager.signinRedirect();
  }

}

export function completeAuthentication(): Promise<any> {
  if (oidcUserManager !== null) {
    return oidcUserManager.signinRedirectCallback()
      .then(updateUserContextFromOidcUser);
  }
}

export function getCurrentContext(): UserContext {
  return userContext;
}