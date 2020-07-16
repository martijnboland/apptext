import React, { useContext } from 'react';
import AppContext from '../apps/AppContext';
import { useTranslation } from 'react-i18next';
import useDocumentTitle from '../common/hooks/useDocumentTitle';

const Dashboard: React.FunctionComponent = () => {
  const { t } = useTranslation(['Labels']);
  const { currentApp } = useContext(AppContext);
  useDocumentTitle(currentApp.displayName, true);

  return (
    <React.Fragment>
      <h2 className="mb-4">{currentApp.displayName}</h2>
      <div className="row">
        <div className="col">
          <h3>{t('Labels:Collections')}</h3>
          <small className="text-muted"></small>
        </div>
        <div className="col">
          <h3>{t('Labels:LatestChanges')}</h3>
          <small className="text-muted"></small>
        </div>
      </div>
    </React.Fragment>
  );
};

export default Dashboard;