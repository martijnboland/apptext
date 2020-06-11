import React, { useContext } from 'react';
import H from 'history';

import UserContext from './UserContext';
import { returnUrlStorageKey } from '../config/constants';
import { useTranslation } from 'react-i18next';

interface ILoginCallbackProps {
  history: H.History
}

const LoginCallback: React.FunctionComponent<ILoginCallbackProps> = (props) => {
  const { t } = useTranslation('Messages');
  const userContext = useContext(UserContext);
  const returnUrl = window.sessionStorage.getItem(returnUrlStorageKey) || '/';
  userContext.completeAuthentication()
    .then(() => {
      this.props.history.push(returnUrl);
    });

  return (<div>{t('Messages:CompletingAuthentication')}</div>);
}

export default LoginCallback;