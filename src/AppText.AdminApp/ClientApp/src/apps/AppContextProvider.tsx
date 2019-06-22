import React from 'react';

import AppContext, { AppContextState } from './AppContext';
import { RouteComponentProps, Redirect, withRouter } from 'react-router-dom';
import { App } from './models';
import { getApps } from './api';

const CURRENT_APP_KEY = 'CURRENT_APP';

interface AppContextProviderProps extends RouteComponentProps<any> {
}

interface AppContextProviderState extends AppContextState {
}

class AppContextProvider extends React.Component<AppContextProviderProps, AppContextProviderState>
{  
  constructor(props) {
    super(props);

    this.state = {
      initApps: this.initApps,
      setCurrentApp: this.setCurrentApp
    };
  }
  
  componentWillMount() {
    this.initApps();
  }

  initApps = (): Promise<any> => {
    return getApps()
      .then(apps => {
        let currentApp: App;
        const currentAppFromStorage = JSON.parse(sessionStorage.getItem(CURRENT_APP_KEY));
        if (currentAppFromStorage && apps.some(a => a.id === currentAppFromStorage.id)) {
          currentApp = currentAppFromStorage;
        } else if (apps.length === 1) {
          currentApp = apps[0];
        }
        this.setState({ apps: apps, currentApp: currentApp });
      });
  }

  setCurrentApp = (app: App): void => {
    this.setState({ currentApp: app });
    sessionStorage.setItem(CURRENT_APP_KEY, JSON.stringify(app));
    this.props.history.push('/');
  }

  render() {
    const { apps, currentApp } = this.state;
    const { location } = this.props;
    const shouldRender = currentApp !== undefined || location.pathname === '/apps/select' || location.pathname === '/apps/create';

    return (
      <>
      {shouldRender
      ?
        <AppContext.Provider value={this.state}>
          {this.props.children}
        </AppContext.Provider>
      : apps !== undefined
        ?
          apps.length == 0 
            ?
              <Redirect to="/apps/create" />
            :
              <Redirect to="/apps/select" />
        :
          <div>Loading app context...</div>
      }
      </>
    );
  }
}

export default withRouter(AppContextProvider);