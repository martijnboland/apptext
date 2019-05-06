import React from 'react';
import { RouteComponentProps } from 'react-router';

import ProtectedRoute from '../auth/ProtectedRoute';
import List from './List';
import Create from './Create';
import Edit from './Edit';

interface ContentTypesProps extends RouteComponentProps<{}> {
}

const ContentTypes: React.FC<ContentTypesProps> = ({ match }) => {
  return (
    <>
      <ProtectedRoute exact path={match.url} component={List} />
      <ProtectedRoute path={`${match.url}/create`} component={Create} />
      <ProtectedRoute path={`${match.url}/edit/:id`} component={Edit} />
    </>
  );
}

export default ContentTypes;