import React, { useState, Suspense } from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import classNames from 'classnames';
import { ModalProvider } from 'react-modal-hook';
import { toast } from 'react-toastify';
import './i18n';

import 'react-toastify/dist/ReactToastify.css';

import UserContextProvider from './auth/UserContextProvider';
import AppContextProvider from './apps/AppContextProvider';
import Header from './navigation/Header';
import Sidebar from './navigation/Sidebar';
import LoginCallback from './auth/LoginCallback';
import ProtectedRoute from './auth/ProtectedRoute';
import Dashboard from './dashboard/Dashboard';
import Apps from './apps/Apps';
import ContentTypes from './contenttypes/ContentTypes';
import CollectionRoutes from './collections/CollectionRoutes';
import ContentList from './content/ContentList';

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

  toast.configure();

  return (
    <Suspense fallback="Loading...">
      <BrowserRouter basename={appConfig.clientBaseRoute}>
        <ModalProvider>
          <UserContextProvider>
            <AppContextProvider>
              <div className={classNames('page-wrapper', { 'toggled': sidebarToggled })}>
                <nav className="sidebar-wrapper">
                  <Sidebar close={onSidebarClose} />
                </nav>
                <main className="page-content">
                  <div className="overlay"></div>
                  <Header sidebarToggled={sidebarToggled} toggleSidebar={onToggleSidebar} />
                  <div className="container-fluid ml-0 mt-3">
                    <Switch>
                      <Route exact path="/login-callback" component={LoginCallback} />
                      <ProtectedRoute path="/apps" component={Apps} />
                      <ProtectedRoute path="/content/:collectionId?/:contentKey?" component={ContentList} currentApp />
                      <ProtectedRoute path="/collections" component={CollectionRoutes} currentApp />
                      <ProtectedRoute path="/contenttypes" component={ContentTypes} currentApp />
                      <ProtectedRoute exact path="/" component={Dashboard} />
                    </Switch>
                  </div>  
                </main>
              </div>
            </AppContextProvider>
          </UserContextProvider>
        </ModalProvider>
      </BrowserRouter>
    </Suspense>
  );

};