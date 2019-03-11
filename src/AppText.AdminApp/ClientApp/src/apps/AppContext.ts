import React from 'react';

import { App } from './models';

const AppContext = React.createContext<Partial<App>>({
  
});

export default AppContext;