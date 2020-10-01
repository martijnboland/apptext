import React, { Component } from 'react';
import { RouteComponentProps, Redirect, withRouter } from 'react-router-dom';
import { withTranslation, WithTranslationProps } from 'react-i18next';

import AppContext, { AppContextState } from './AppContext';
import { App } from './models';
import { getApps } from './api';
import { currentAppStorageKey } from '../config/constants';

interface AppContextProviderProps extends RouteComponentProps<any> {
}

interface AppContextProviderState extends AppContextState {
}

class AppContextProvider extends React.Component<AppContextProviderProps & WithTranslationProps, AppContextProviderState>
{  
  constructor(props) {
    super(props);

    this.state = {
      apps: [],
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
        const currentAppFromStorage = JSON.parse(sessionStorage.getItem(currentAppStorageKey));
        if (currentAppFromStorage && apps.some(a => a.id === currentAppFromStorage.id)) {
          currentApp = apps.find(a => a.id ===currentAppFromStorage.id);
          // Always ensure that the storage is up to date
          sessionStorage.setItem(currentAppStorageKey, JSON.stringify(currentApp));
        } else if (apps.length === 1) {
          // Just set first non-system app as currentApp when none is set.
          currentApp = apps.find(a => !a.isSystemApp);
        }
        this.setState({ apps: apps, currentApp: currentApp });
      });
  }

  setCurrentApp = (app: App): void => {
    this.setState({ currentApp: app });
    sessionStorage.setItem(currentAppStorageKey, JSON.stringify(app));
    this.props.history.push('/');
  }

  render() {
    const { apps, currentApp } = this.state;
    const nonSystemApps = apps !== undefined
      ? apps.filter(a => !a.isSystemApp)
      : [];
    const { location, i18n } = this.props;
    const shouldRender = currentApp !== undefined || location.pathname === '/apps/select' || location.pathname === '/apps/create';

    return (
      <>
      {shouldRender
      ?
        <AppContext.Provider value={this.state}>
          {this.props.children}
        </AppContext.Provider>
      : nonSystemApps !== undefined
        ?
        nonSystemApps.length == 0 
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