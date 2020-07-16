import React, { useContext } from 'react';
import AppContext from './AppContext';
import { useTranslation } from 'react-i18next';

const SelectApp: React.FunctionComponent = () => {

  const { t } = useTranslation('Labels');

  var appContext = useContext(AppContext);


  return (
    <>
      <h2>Select app</h2>
      <div className="card-columns">
        {appContext.apps && appContext.apps.map(app => (
          <div key={app.id} className="card">
            <div className="card-body">
              <h5 className="card-title">{app.id}</h5>
              <p className="card-text">{app.displayName}</p>
              <button className="btn btn-primary" onClick={() => appContext.setCurrentApp(app)}>{t('Labels:SelectButton')}</button>
            </div>
          </div>
        ))}
      </div>
    </>
  );
};

export default SelectApp;