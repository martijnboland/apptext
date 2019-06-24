import React, { useContext, useState } from 'react';
import classNames from 'classnames';
import { FaEdit, FaCaretDown } from 'react-icons/fa';
import { Link } from 'react-router-dom';

import AppContext from '../apps/AppContext';

import './AppSelector.scss';
import { App } from '../apps/models';

const AppSelector: React.FunctionComponent = () => {
  
  const [expandOtherApps, setExpandOtherApps] = useState(false);
  const { currentApp, apps, setCurrentApp } = useContext(AppContext);
  const otherApps = currentApp 
    ? apps.filter(app => app.id !== currentApp.id)
    : apps;

  const onToggleExpand = () => {
    setExpandOtherApps(! expandOtherApps);
  };

  const collapseOtherApps = () => {
    setExpandOtherApps(false);
  }

  const onChangeCurrentApp = (app: App) => {
    setCurrentApp(app);
    collapseOtherApps();
  }

  return (
    <div className="app-selector">
      {currentApp && 
        <>
          <div className={classNames('current-app', { 'with-other-apps': otherApps.length > 0, 'expand': expandOtherApps })}>
            <div className="current-app-inner">
              <div className="d-flex justify-content-center">
                <div className="app-id">{currentApp.id}</div>
                <div className="ml-auto">
                  <Link to="/apps/editcurrent">
                    <FaEdit />
                  </Link>
                  {otherApps.length > 0 &&
                    <FaCaretDown className="expander ml-1"  onClick={onToggleExpand} />
                  }
                </div>
              </div>
              <div className="app-name">{currentApp.displayName}</div>
            </div>
            <ul className="other-apps">
              {otherApps.map(app => {
                return (
                  <li key={app.id} className="p-3" onClick={() => onChangeCurrentApp(app)}>
                    <div className="app-id">{app.id}</div>
                    <div className="app-name">{app.displayName}</div>
                  </li>
                );
              })}
            </ul>
          </div>
        </>
      }
    </div>
  );
};

export default AppSelector;