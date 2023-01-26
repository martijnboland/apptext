import '../styles/styles.scss';

import React from 'react';
import { createRoot } from 'react-dom/client';
import { AdminApp } from './AdminApp';

const container = document.getElementById('app');
const root = createRoot(container!);

root.render(<AdminApp />);