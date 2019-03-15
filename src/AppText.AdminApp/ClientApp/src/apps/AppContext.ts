import React from 'react';

import { App } from './models';

export type AppContextState = {
  apps?: App[]
  currentApp?: App,
  setCurrentApp(app: App): void
};


const AppContext = React.createContext<Partial<AppContextState>>({
  
});

export default AppContext;