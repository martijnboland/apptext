import React from 'react';

import ProtectedRoute from '../auth/ProtectedRoute';
import CreateApp from './CreateApp';
import SelectApp from './SelectApp';

const Apps: React.FunctionComponent = () => {
  return (
    <>
      <ProtectedRoute path="/applications/create" component={CreateApp} />
      <ProtectedRoute path="/applications/select" component={SelectApp} />
    </>
  );
};

export default Apps;