import React from 'react';
import { RouteComponentProps } from 'react-router';
import ProtectedRoute from '../auth/ProtectedRoute';
import List from './List';

interface ContentProps extends RouteComponentProps<{}> {
}

const Content: React.FC<ContentProps> = ({ match }) => {
  return (
    <>
      <ProtectedRoute exact path={match.url} component={List} />
    </>
  );
};

export default Content;
