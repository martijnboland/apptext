import '../styles/styles.scss';
import { initUserContext } from './auth/userContext';

import React from 'react';
import ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';

const renderApp = () => {
  const App = require('./App').App;

  ReactDOM.render(
    <AppContainer>
      <App />
    </AppContainer>,
    document.getElementById('app')
  );
};

if (module.hot) {
  const reRenderApp = () => {
    renderApp();
  };

  module.hot.accept('./App', () => {
    setImmediate(() => {
      reRenderApp();
    });
  });
}

initUserContext()
  .then(() => {
    renderApp();
  });