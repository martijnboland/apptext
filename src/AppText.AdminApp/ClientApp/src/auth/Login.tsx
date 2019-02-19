import * as React from 'react';

import { startAuthentication } from './userContext';
import { returnUrlStorageKey } from '../config/constants';

interface ILoginProps {
  redirectUrl: string
}

export default class Login extends React.Component<ILoginProps, object> {

  constructor(props: ILoginProps) {
    super(props);
  }

  componentWillMount(): void {
    startAuthentication()
      .then(() => {
        window.sessionStorage.setItem(returnUrlStorageKey, this.props.redirectUrl);
      });
  }

  render(): any {
    return null;
  }
}
