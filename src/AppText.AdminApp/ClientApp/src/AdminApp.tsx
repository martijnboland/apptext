import React, { useState } from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import classNames from 'classnames';

import UserContextProvider from './auth/UserContextProvider';
import AppContextProvider from './apps/AppContextProvider';
import Header from './navigation/Header';
import Sidebar from './navigation/Sidebar';
import LoginCallback from './auth/LoginCallback';
import ProtectedRoute from './auth/ProtectedRoute';
import Dashboard from './dashboard/Dashboard';
import Apps from './apps/Apps';
import { appConfig } from './config/AppConfig'; 

import './AdminApp.scss';

export const AdminApp: React.FunctionComponent = () => {

  const [sidebarToggled, setSidebarToggled] = useState(false);

  const onToggleSidebar = () => {
    setSidebarToggled(!sidebarToggled);
  };

  const onSidebarClose = () => {
    setSidebarToggled(!sidebarToggled);
  };

  return (
    <BrowserRouter basename={appConfig.clientBaseRoute}>
      <UserContextProvider>
        <AppContextProvider>
          <div className={classNames('page-wrapper', { 'toggled': sidebarToggled })}>
            <nav className="sidebar-wrapper">
              <Sidebar close={onSidebarClose} />
            </nav>
            <main className="page-content">
              <div className="overlay"></div>
              <Header sidebarToggled={sidebarToggled} toggleSidebar={onToggleSidebar} />
              <div className="container-fluid mt-3">
                <Switch>
                  <Route exact path="/login-callback" component={LoginCallback} />
                  <ProtectedRoute path="/apps" component={Apps} />
                  <ProtectedRoute path="/" component={Dashboard} />
                </Switch>
              </div>  
            </main>
          </div>
        </AppContextProvider>
      </UserContextProvider>
    </BrowserRouter>
  );

};