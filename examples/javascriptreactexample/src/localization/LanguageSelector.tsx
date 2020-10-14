import React from 'react';
import { useTranslation } from 'react-i18next';
import { appTextAppId, currentLanguageStorageKey } from './config';

const LanguageSelector: React.FunctionComponent = () => {
  const { i18n } = useTranslation();

  const currentLanguage = i18n.language;

  return (
    <form>

    </form>
  );
};

export default LanguageSelector;
