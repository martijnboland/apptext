import * as React from 'react';
import * as H from 'history';

import { completeAuthentication } from './userContext';
import { returnUrlStorageKey } from '../config/constants';

interface ILoginCallbackProps {
  history: H.History
}

export default class LoginCallback extends React.Component<ILoginCallbackProps, object> {

  constructor(props: ILoginCallbackProps) {
    super(props);
  }

  componentWillMount(): void {
    var returnUrl = window.sessionStorage.getItem(returnUrlStorageKey) || '/';
    completeAuthentication()
      .then(() => {
        this.props.history.push(returnUrl);
      });
  }

  render(): any {
    return null;
  }
}
