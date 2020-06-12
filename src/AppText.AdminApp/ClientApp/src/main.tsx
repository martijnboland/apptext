import '../styles/styles.scss';

import React from 'react';
import ReactDOM from 'react-dom';

const renderApp = () => {
  const AdminApp = require('./AdminApp').AdminApp;

  ReactDOM.render(
    <AdminApp />,
    document.getElementById('app')
  );
};

if (module.hot) {
  const reRenderApp = () => {
    renderApp();
  };

  module.hot.accept('./AdminApp', () => {
    setImmediate(() => {
      reRenderApp();
    });
  });
}

renderApp();