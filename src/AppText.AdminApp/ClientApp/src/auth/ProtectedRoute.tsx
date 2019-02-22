import React, { useContext } from 'react';
import { Route } from 'react-router-dom';

import UserContext from './UserContext';
import Login from './Login';
import AccessDenied from './AccessDenied';

export default ({ component: Component, ...rest }) => {
  const userContext = useContext(UserContext);
  return (
    <Route {...rest} render={props => {

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

      const isAllowed = userContext.isAuthenticated; //&& isInRole(allowedRole);
      
      return (
        userContext.isAuthenticated
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
  );
};