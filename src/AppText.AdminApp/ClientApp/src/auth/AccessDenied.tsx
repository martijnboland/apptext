import React from 'react';
import { useTranslation } from 'react-i18next';

export default () => {
  const { t } = useTranslation('Messages'); 

  return (
    <h1>{t('Messages:AccessDenied')}</h1>
  );
}