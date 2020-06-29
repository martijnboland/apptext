import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import Backend from 'i18next-http-backend';

import { appConfig } from './config/AppConfig';
import { appTextAdminAppId } from './config/constants';

i18n
  .use(Backend)
  .use(initReactI18next)
  .init({
    lng: 'en',
    fallbackLng: 'en',
    returnNull: false,
    backend: {
      loadPath: `${appConfig.apiBaseUrl}/${appTextAdminAppId}/translations/{{lng}}/{{ns}}`
    },
    debug: true,
    ns: ['Labels', 'Messages', 'Errors'],
    defaultNS: 'Labels'
  });


export default i18n;