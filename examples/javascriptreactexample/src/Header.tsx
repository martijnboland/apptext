import React from 'react';
import { useTranslation } from 'react-i18next';

const Header: React.FC = () => {
  const { t } = useTranslation('Labels');

  return (
    <header className="header-top">
      <div className="app-title">
        {t('AppText JavaScript React demo')}
      </div>
    </header>
  )
}

export default Header;