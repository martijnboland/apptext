import React, { useContext } from 'react';
import H from 'history';

import UserContext from './UserContext';
import { returnUrlStorageKey } from '../config/constants';

interface ILoginCallbackProps {
  history: H.History
}

const LoginCallback: React.FunctionComponent<ILoginCallbackProps> = (props) => {
  const userContext = useContext(UserContext);
  const returnUrl = window.sessionStorage.getItem(returnUrlStorageKey) || '/';
  userContext.completeAuthentication()
    .then(() => {
      this.props.history.push(returnUrl);
    });

  return (<div>Completing authentication...</div>);
}

export default LoginCallback;