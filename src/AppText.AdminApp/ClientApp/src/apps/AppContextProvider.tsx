import React from 'react';

import AppContext from './AppContext';
import { Redirect, withRouter } from 'react-router-dom';
import { App } from './models';
import { getApps } from './api';

interface AppContextProviderState {
  apps?: App[],
  currentApp?: App
}

class AppContextProvider extends React.Component<any, AppContextProviderState>
{  
  constructor(props) {
    super(props);

    this.state = {};
  }
  
  componentWillMount() {
    this.initApps();
  }

  initApps(): Promise<any> {
    return getApps()
      .then(apps => {
        const currentApp = (apps.length === 1) ? apps[0] : undefined;
        this.setState({ apps: apps, currentApp: currentApp });
      });
  }

  render() {
    const { apps, currentApp } = this.state;
    const { location } = this.props;
    const shouldRender = currentApp !== undefined || location.pathname === '/applications/select' || location.pathname === '/applications/create';
    return (
      <>
      {shouldRender
      ?
        <AppContext.Provider value={currentApp}>
          {this.props.children}
        </AppContext.Provider>
      : apps !== undefined
        ?
          apps.length == 0 
            ?
              <Redirect to="/applications/create" />
            :
              <Redirect to="/applications/select" />
        :
          <div>Loading app context...</div>
      }
      </>
    );
  }
}

export default withRouter(AppContextProvider);