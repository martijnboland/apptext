import React, { useState } from 'react';
import classNames from 'classnames';

import Header from './navigation/Header';
import Sidebar from './navigation/Sidebar';

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
    <div className={classNames('page-wrapper', { 'toggled': sidebarToggled })}>
      <nav className="sidebar-wrapper">
        <Sidebar close={onSidebarClose} />
      </nav>
      <main className="page-content">
        <div className="overlay"></div>
        <Header sidebarToggled={sidebarToggled} toggleSidebar={onToggleSidebar} />
        <div className="container-fluid">
          
        </div>  
      </main>
    </div>
  );

};