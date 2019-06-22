import React, { useContext } from 'react';

import UserContext from '../auth/UserContext';

interface HeaderProps {
  sidebarToggled: boolean,
  toggleSidebar: () => void
};

const Header: React.FunctionComponent<HeaderProps> = ({ sidebarToggled, toggleSidebar }) => {
  const userContext = useContext(UserContext);

  return (
    <nav className="navbar navbar-light bg-light flex-md-nowrap shadow">
      <button className="navbar-toggler btn-link" type="button">
        <span className="navbar-toggler-icon" onClick={toggleSidebar}></span>
      </button>
      <ul className="navbar-nav">
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