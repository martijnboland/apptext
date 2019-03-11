import '../styles/styles.scss';

import React from 'react';
import ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';

const renderApp = () => {
  const AdminApp = require('./AdminApp').AdminApp;

  ReactDOM.render(
    <AppContainer>
      <AdminApp />
    </AppContainer>,
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