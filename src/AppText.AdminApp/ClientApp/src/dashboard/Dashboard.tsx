import React, { useContext } from 'react';
import AppContext from '../apps/AppContext';

interface DashboardProps {
}

const Dashboard: React.FunctionComponent<DashboardProps> = (props) => {
  const { currentApp } = useContext(AppContext);
  return (
    <h2>{currentApp.displayName}</h2>
  );
};

export default Dashboard;