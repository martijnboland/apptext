import React from 'react';
import { useTranslation } from 'react-i18next';
import { useQuery } from 'urql';
import Loader from '../loader/Loader';
import { currentLanguageStorageKey } from './config';

const LanguagesQuery = `
  query {
    languages
    defaultLanguage
  }
`;

const LanguageSelector: React.FunctionComponent = () => {
  const { i18n, t } = useTranslation();

  const [{ data, fetching, error }] = useQuery({
    query: LanguagesQuery,
  });

  const currentLanguage = i18n.language;

  const changeLanguage = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newLanguage = e.target.value;
    i18n.changeLanguage(newLanguage)
      .then(() => { 
        localStorage.setItem(currentLanguageStorageKey, newLanguage);
      });
  }

  return fetching
    ?
      <Loader />
    :
      error
      ?
        <span>{error.message}</span>
      :
        <form className="form-inline">
          <label htmlFor="language" style={{marginRight: '0.5em'}}>{t('Language')}</label>
          <select value={currentLanguage} onChange={changeLanguage}>
            {data.languages.map((lang:string) => <option key={lang}>{lang}</option>)}
          </select>
        </form>
};

export default LanguageSelector;
