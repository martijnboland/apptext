import React, { useState } from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import classNames from 'classnames';

import Header from './navigation/Header';
import Sidebar from './navigation/Sidebar';
import LoginCallback from './auth/LoginCallback';
import ProtectedRoute from './auth/ProtectedRoute';
import Dashboard from './dashboard/Dashboard';

import './App.scss';

export const App: React.FunctionComponent = () => {

  const [sidebarToggled, setSidebarToggled] = useState(false);

  const onToggleSidebar = () => {
    setSidebarToggled(!sidebarToggled);
  };

  const onSidebarClose = () => {
    setSidebarToggled(!sidebarToggled);
  };

  return (
    <BrowserRouter>
      <div className={classNames('page-wrapper', { 'toggled': sidebarToggled })}>
        <nav className="sidebar-wrapper">
          <Sidebar close={onSidebarClose} />
        </nav>
        <main className="page-content">
          <div className="overlay"></div>
          <Header sidebarToggled={sidebarToggled} toggleSidebar={onToggleSidebar} />
          <div className="container-fluid">
            <Switch>
              <Route exact path="/login-callback" component={LoginCallback} />
              <ProtectedRoute path="/" component={Dashboard} />
            </Switch>
          </div>  
        </main>
      </div>
    </BrowserRouter>
  );

};