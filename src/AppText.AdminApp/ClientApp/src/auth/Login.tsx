import React, { useContext } from 'react';

import UserContext from './UserContext';

interface ILoginProps {
  redirectUrl: string
}

const Login : React.FunctionComponent<ILoginProps> = ({ redirectUrl }) => {
  const userContext = useContext(UserContext);
  userContext.startAuthentication(redirectUrl);
  return (
    <div>Authenticating...</div>
  );
}

export default Login;