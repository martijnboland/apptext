import * as React from 'react';
import { FaTimes } from 'react-icons/fa';

interface IListHeaderProps {
  allLanguages: string[],
  activeLanguages: string[],
  onLanguageAdded: (language: string) => void,
  onLanguageRemoved: (language: string) => void
}

const ListHeader: React.FunctionComponent<IListHeaderProps> = ({ allLanguages, activeLanguages, onLanguageAdded, onLanguageRemoved }) => {

  const availableLanguages =  allLanguages.filter(l => ! activeLanguages.includes(l));

  const languageSelected = (ev: React.ChangeEvent<HTMLSelectElement>) => {
    if (ev.currentTarget.value) {
      onLanguageAdded(ev.currentTarget.value);
    }
  }

  return (
    <div className="row">
      <div className="col-3 align-self-center">key</div>

      {activeLanguages.map(lang => 
        <div key={lang} className="col"><button type="button" className="btn btn-light" onClick={() => onLanguageRemoved(lang)}>{lang}<FaTimes className="ml-1" /></button></div>
      )}

      {availableLanguages.length > 0 &&
        <div className="col">
          <select className="form-control" onChange={languageSelected}>
            <option></option>
            {availableLanguages.map(lang =>
              <option key={lang} value={lang}>{lang}</option>
            )}
          </select>
        </div>
      }
    </div>
  );
};

export default ListHeader;
