import React from 'react';
import { FaTimes } from 'react-icons/fa';
import { useTranslation } from 'react-i18next';

interface IListHeaderProps {
  allLanguages: string[],
  activeLanguages: string[],
  onLanguageAdded: (language: string) => void,
  onLanguageRemoved: (language: string) => void
}

const ListHeader: React.FunctionComponent<IListHeaderProps> = ({ allLanguages, activeLanguages, onLanguageAdded, onLanguageRemoved }) => {
  const { t } = useTranslation('Labels');
  const availableLanguages =  allLanguages.filter(l => ! activeLanguages.includes(l));

  const languageSelected = (ev: React.ChangeEvent<HTMLSelectElement>) => {
    if (ev.currentTarget.value) {
      onLanguageAdded(ev.currentTarget.value);
    }
  }

  return (
    <div className="card-body">
      <div className="row align-items-center">
        <div className="col-3"></div>

        {activeLanguages.map(lang => 
          <div key={lang} className="col"><button type="button" className="btn btn-light" onClick={() => onLanguageRemoved(lang)}>{lang}<FaTimes className="ml-1" /></button></div>
        )}
        <div className="col-2">
          {availableLanguages.length > 0 &&
            <select className="form-control" onChange={languageSelected}>
              <option>{t('Labels:ChooseLanguage')}</option>
              {availableLanguages.map(lang =>
                <option key={lang} value={lang}>{lang}</option>
              )}
            </select>
          }
        </div>
      </div>
    </div>
  );
};

export default ListHeader;
