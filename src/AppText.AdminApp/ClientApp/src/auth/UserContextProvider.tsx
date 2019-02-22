import React from 'react';
import { UserManager, User } from 'oidc-client';

import { appConfig, AuthType } from '../config/AppConfig';
import UserContext, { UserContextState } from './UserContext';
import { returnUrlStorageKey } from '../config/constants';
import { getMe } from './api';

class UserContextProvider extends React.Component<any, UserContextState>
{
  private oidcUserManager: UserManager = null;
  
  constructor(props) {
    super(props);

    this.state = {
      isAuthenticated: false,
      startAuthentication: this.startAuthentication,
      completeAuthentication: this.completeAuthentication
    }    
  }
  
  componentWillMount() {
    this.initUserContext();
  }

  initUserContext(): Promise<any> {
    if (appConfig.authType === AuthType.Oidc) {
      this.oidcUserManager = new UserManager(appConfig.oidcSettings);
      return this.oidcUserManager.getUser()
        .then(this.updateUserContextFromOidcUser);
    }
    else {
      // Call api for user info
      return getMe()
        .then(me => {
          this.setState({ isAuthenticated: true, userId: me.identifier, userName: me.name, claims: me.claims })
        });
    }
  }

  startAuthentication(redirectUrl?: string): Promise<any> {
    if (appConfig.authType == AuthType.Oidc && this.oidcUserManager) {
      return this.oidcUserManager.signinRedirect()
        .then(() => {
          if (redirectUrl) {
            window.sessionStorage.setItem(returnUrlStorageKey, redirectUrl);
          }
        });
    }
  }  

  updateUserContextFromOidcUser(user: User) {
    console.log('OIDC user => ', user);
    if (user) {
      this.setState({ isAuthenticated: true, userId: user.profile.sub, userName: user.profile.name, claims: user.profile });
    }
  }

  completeAuthentication(): Promise<any> {
    if (this.oidcUserManager !== null) {
      return this.oidcUserManager.signinRedirectCallback()
        .then(this.updateUserContextFromOidcUser);
    }
  }

  render() {
    return (
      <UserContext.Provider value={this.state}>
        {this.props.children}
      </UserContext.Provider>
    );
  }
}

export default UserContextProvider;