import React from 'react';

import AppSelector from './AppSelector';
import MainMenu from './MainMenu';
import { useTranslation } from 'react-i18next';

interface SidebarProps {
  close: () => void
};

const Sidebar: React.FunctionComponent<SidebarProps> = ({ close }) => {
  const { t } = useTranslation('Labels')

  return (
    <>
      <div className="d-md-none p-2 clearfix">
        <button type="button" className="close" aria-label={t('Labels:CloseButton')} onClick={close}>
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <AppSelector />
      <MainMenu />
    </>
  );
};

export default Sidebar;