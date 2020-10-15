import React from 'react';
import { useTranslation } from 'react-i18next';
import LanguageSelector from './localization/LanguageSelector';

const Header: React.FC = () => {
  const { t } = useTranslation();

  return (
    <header className="header-top">
      <div className="app-title">
        {t('AppText JavaScript React demo')}
      </div>
      <LanguageSelector />
    </header>
  )
}

export default Header;