import React, { useContext } from 'react';
import AppContext from '../apps/AppContext';
import { useTranslation } from 'react-i18next';
import useDocumentTitle from '../common/hooks/useDocumentTitle';
import Collections from '../collections/Collections';
import LatestContentList from '../content/LatestContentList';

const Dashboard: React.FunctionComponent = () => {
  const { t } = useTranslation(['Labels']);
  const { currentApp } = useContext(AppContext);
  const itemsToShow = 5;

  useDocumentTitle(currentApp.displayName, true);

  return (
    <React.Fragment>
      <h2 className="mb-5">{currentApp.displayName}</h2>
      <div className="row">
        <div className="col-md-6">
          <Collections currentApp={currentApp} />
        </div>
        <div className="col-md-6">
          <LatestContentList currentApp={currentApp} itemsToShow={itemsToShow} />
        </div>
      </div>
    </React.Fragment>
  );
};

export default Dashboard;