import React, { useContext } from 'react';

import LanguageSelector from './LanguageSelector';
import UserContext from '../auth/UserContext';
import AppContext from '../apps/AppContext';

interface HeaderProps {
  sidebarToggled: boolean,
  toggleSidebar: () => void
};

const Header: React.FunctionComponent<HeaderProps> = ({ sidebarToggled, toggleSidebar }) => {
  const userContext = useContext(UserContext);
  const appContext = useContext(AppContext);

  return (
    <nav className="navbar navbar-expand navbar-light bg-light flex-md-nowrap shadow">
      <button className="navbar-toggler btn-link d-block mr-auto" type="button">
        <span className="navbar-toggler-icon" onClick={toggleSidebar}></span>
      </button>
      <ul className="navbar-nav">
        <LanguageSelector />      
        {userContext.isAuthenticated && 
          <li className="nav-item">
            <a className="nav-link">{userContext.userName}</a>
          </li>      
        }
      </ul>
    </nav>
  );
};

export default Header;