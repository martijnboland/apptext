import * as React from 'react';
import { Route } from 'react-router-dom';

import Login from './Login';
import AccessDenied from './AccessDenied';

import { getCurrentContext } from './userContext';

export default ({ component: Component, ...rest }) => (
  <Route {...rest} render={props => {

    const userContext = getCurrentContext();

    const isInRole = (role) => {
      if (!role) {
        return true; // return true when no specific role is demanded
      }
      const claims = userContext.claims;
      const roles = claims.role;

      if (! roles || roles.length === 0) {
        return false;
      }

      return roles === role || (Array.isArray(roles) && roles.some(r => r === role));
    }

    const isAllowed = userContext.isLoggedIn; //&& isInRole(allowedRole);
    
    return (
      userContext.isLoggedIn
        ? isAllowed
          ? 
          (
            <Component {...props}/>
          )
          : (
            <AccessDenied />
          )
        : (
          <Login redirectUrl={props.match.url}/>
        )
    );
  }}
  />
)