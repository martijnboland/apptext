import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import Backend from 'i18next-http-backend';

import { appConfig } from './config/AppConfig';

i18n
  .use(Backend)
  .use(initReactI18next)
  .init({
    fallbackLng: 'en',
    backend: {
      loadPath: `${appConfig.apiBaseUrl}/apptext-admin/translations/{{lng}}/{{ns}}`
    },
    debug: true,
    ns: ['Labels', 'Messages', 'Errors']
  });


export default i18n;