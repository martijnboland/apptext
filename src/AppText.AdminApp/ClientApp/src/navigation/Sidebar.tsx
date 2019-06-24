import React from 'react';

import AppSelector from './AppSelector';
import MainMenu from './MainMenu';

interface SidebarProps {
  close: () => void
};

const Sidebar: React.FunctionComponent<SidebarProps> = ({ close }) => {
  return (
    <>
      <div className="d-md-none p-2 clearfix">
        <button type="button" className="close" aria-label="Close" onClick={close}>
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <AppSelector />
      <MainMenu />
    </>
  );
};

export default Sidebar;