import React, { Component } from 'react';
import { RouteComponentProps, Redirect, withRouter } from 'react-router-dom';
import { withTranslation, WithTranslationProps } from 'react-i18next';

import AppContext, { AppContextState } from './AppContext';
import { App } from './models';
import { getApps } from './api';

const CURRENT_APP_KEY = 'CURRENT_APP';

interface AppContextProviderProps extends RouteComponentProps<any> {
}

interface AppContextProviderState extends AppContextState {
}

class AppContextProvider extends React.Component<AppContextProviderProps & WithTranslationProps, AppContextProviderState>
{  
  constructor(props) {
    super(props);

    this.state = {
      initApps: this.initApps,
      setCurrentApp: this.setCurrentApp
    };
  }
  
  componentDidMount() {
    this.initApps();
  }

  initApps = (): Promise<any> => {
    return getApps()
      .then(apps => {
        let currentApp: App;
        const currentAppFromStorage = JSON.parse(sessionStorage.getItem(CURRENT_APP_KEY));
        if (currentAppFromStorage && apps.some(a => a.id === currentAppFromStorage.id)) {
          currentApp = apps.find(a => a.id ===currentAppFromStorage.id);
          // Always ensure that the storage is up to date
          sessionStorage.setItem(CURRENT_APP_KEY, JSON.stringify(currentApp));
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
    const { location, i18n } = this.props;
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
          <div>{i18n.t('Messages:LoadingAppContext')}</div>
      }
      </>
    );
  }
}

export default withRouter(withTranslation()(AppContextProvider) as any);