import React from 'react';
import { RouteComponentProps } from 'react-router';

import ProtectedRoute from '../auth/ProtectedRoute';
import Create from './Create';
import Edit from './Edit';

interface CollectionRoutesProps extends RouteComponentProps<{}> {
}

const CollectionRoutes: React.FC<CollectionRoutesProps> = ({ match }) => {
  return (
    <>
      <ProtectedRoute path={`${match.url}/create`} component={Create} />
      <ProtectedRoute path={`${match.url}/edit/:id`} component={Edit} />
    </>
  );
}

export default CollectionRoutes;