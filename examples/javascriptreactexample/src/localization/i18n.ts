import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import Backend from 'i18next-http-backend';
import { appTextApiBaseUrl, appTextAppId, currentLanguageStorageKey, appTextApiKey } from './config';

const language = localStorage.getItem(currentLanguageStorageKey) || 'en';

i18n
  .use(Backend)
  .use(initReactI18next)
  .init({
    lng: language,
    fallbackLng: 'en',
    returnNull: false,
    backend: {
      loadPath: `${appTextApiBaseUrl}/${appTextAppId}/translations/public/{{lng}}/{{ns}}`,
      customHeaders: {
        'X-Api-Key': appTextApiKey,
        // ...
      },
    },
    debug: true,
    ns: ['labels', 'messages'],
    nsSeparator: ':',
    defaultNS: 'labels'
  });


export default i18n;