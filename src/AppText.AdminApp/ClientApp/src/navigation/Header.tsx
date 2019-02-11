import React from 'react';

interface HeaderProps {
  sidebarToggled: boolean,
  toggleSidebar: () => void
};

const Header: React.FunctionComponent<HeaderProps> = ({ sidebarToggled, toggleSidebar }) => {
  return (
    <nav className="navbar navbar-dark bg-dark flex-md-nowrap shadow">
      <button className="navbar-toggler btn-link" type="button">
        <span className="navbar-toggler-icon" onClick={toggleSidebar}></span>
      </button>
    </nav>
  );
};

export default Header;