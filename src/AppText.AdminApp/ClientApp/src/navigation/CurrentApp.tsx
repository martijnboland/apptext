import React, { useContext } from 'react';
import { RouteComponentProps, withRouter } from 'react-router';

import AppContext from '../apps/AppContext';

import './CurrentApp.scss';

const CurrentApp: React.FunctionComponent = () => {
  
  var appContext = useContext(AppContext);

  return (
    <div className="p-3">
      {appContext.currentApp && 
        <>
          <div className="current-appid">{appContext.currentApp.id}</div>
          <div>{appContext.currentApp.displayName}</div>
        </>
      }
    </div>
  );
};

export default CurrentApp;