import React, { useContext } from 'react';
import AppContext from '../apps/AppContext';
import { useTranslation } from 'react-i18next';
import useDocumentTitle from '../common/hooks/useDocumentTitle';
import Collections from '../collections/Collections';

const Dashboard: React.FunctionComponent = () => {
  const { t } = useTranslation(['Labels']);
  const { currentApp } = useContext(AppContext);
  useDocumentTitle(currentApp.displayName, true);

  return (
    <React.Fragment>
      <h2 className="mb-5">{currentApp.displayName}</h2>
      <div className="row">
        <div className="col-md-6">
          <Collections currentApp={currentApp} />
        </div>
        <div className="col-md-6">
          <h3>{t('Labels:LatestChanges')}</h3>
          <small className="text-muted"></small>
        </div>
      </div>
    </React.Fragment>
  );
};

export default Dashboard;