import React, { useContext } from 'react';
import { RouteComponentProps } from 'react-router';

import ProtectedRoute from '../auth/ProtectedRoute';
import List from './List';
import Create from './Create';
import Edit from './Edit';
import { ContentType } from './models';
import AppContext from '../apps/AppContext'
import { useApiGet } from '../common/api';
import { appConfig } from '../config/AppConfig';

interface ContentTypesProps extends RouteComponentProps<{}> {
}

const ContentTypes: React.FC<ContentTypesProps> = ({ match }) => {
  const { currentApp } = useContext(AppContext);
  const url = `${appConfig.apiBaseUrl}/${currentApp.id}/contenttypes`;
  const { data, isLoading } = useApiGet<ContentType[]>(url, []);
  return (
    <>
      {isLoading 
        ? 
        <div>Loading...</div>
        :
        <>
          <ProtectedRoute path={match.url} component={List} contentTypes={data} />
          <ProtectedRoute path={`${match.url}/create`} component={Create} />
          <ProtectedRoute path={`${match.url}/edit/:id`} component={Edit} />
        </>
      }
    </>
  );
}

export default ContentTypes;