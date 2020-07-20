import React, { useContext } from 'react';
import classNames from 'classnames';
import useComponentVisible from '../common/hooks/useComponentVisible';
import AppContext from '../apps/AppContext';
import { useTranslation } from 'react-i18next';
import { appTextAdminAppId, currentLanguageStorageKey } from '../config/constants';

const LanguageSelector: React.FunctionComponent = () => {  
  const { ref, isComponentVisible, setIsComponentVisible } = useComponentVisible(false);

  const { i18n } = useTranslation();

  const toggleLanguageSelector = (e: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
    e.preventDefault();
    setIsComponentVisible(true);
  };

  const changeLanguage = (e: React.MouseEvent<HTMLAnchorElement, MouseEvent>, language: string) => {
    e.preventDefault();
    i18n.changeLanguage(language)
      .then(() => { 
        setIsComponentVisible(false);
        localStorage.setItem(currentLanguageStorageKey, language);
      });
  }

  const currentLanguage = i18n.language;

  const { apps} = useContext(AppContext);
  const adminApp = apps.find(a => a.id === appTextAdminAppId);
  const languages = adminApp?.languages.filter(l => l !== currentLanguage) || [];

  return (
    <li className="nav-item dropdown">
      <a className="nav-link dropdown-toggle" id="languageselector" href="#" aria-haspopup="true" aria-expanded="false" onClick={toggleLanguageSelector}>
        {currentLanguage}
      </a>
      <div ref={ref} className={classNames('dropdown-menu', 'dropdown-menu-right', { show: isComponentVisible })} aria-labelledby="languageselector">
        {languages.map(language =>
          <a key={language} className="dropdown-item" href="#" onClick={(e) => changeLanguage(e, language)}>{language}</a>
        )}
      </div>
    </li>
  );
};

export default LanguageSelector;
