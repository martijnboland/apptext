import React, { useContext } from 'react';

import UserContext from './UserContext';
import { useTranslation } from 'react-i18next';

interface ILoginProps {
  redirectUrl: string
}

const Login : React.FunctionComponent<ILoginProps> = ({ redirectUrl }) => {
  const { t } = useTranslation('Messages');
  const userContext = useContext(UserContext);
  
  userContext.startAuthentication(redirectUrl);
  
  return (
    <div>{t('Messages:Authenticating')}</div>
  );
}

export default Login;