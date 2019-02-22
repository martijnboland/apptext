import React from 'react';

export type UserContextState = {
  isAuthenticated: boolean,
  userId?: string
  userName?: string,
  claims?: any,
  startAuthentication?: (redirectUrl: string) => Promise<any>,
  completeAuthentication?: () => Promise<any>
};

const UserContext = React.createContext<Partial<UserContextState>>({
  isAuthenticated: false
});

export default UserContext;