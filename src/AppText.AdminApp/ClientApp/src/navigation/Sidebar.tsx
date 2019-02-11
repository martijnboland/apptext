import React from 'react';

interface SidebarProps {
  close: () => void
};

const Sidebar: React.FunctionComponent<SidebarProps> = ({ close }) => {
  return (
    <>
      <div className="float-right d-block d-md-none p-2">
        <button type="button" className="close" aria-label="Close" onClick={close}>
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div>
        Sidebar
      </div>
    </>
  );
};

export default Sidebar;